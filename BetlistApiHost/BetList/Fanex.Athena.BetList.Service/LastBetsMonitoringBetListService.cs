// -----------------------------------------------------------------------
// <copyright file="LastBetsMonitoringBetListService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Constant;
    using Common.Utility.Helpers;
    using Common.Utility.Services.Interfaces;
    using Fanex.Athena.BetList.DataAccess.Interfaces;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Interfaces;

    public class LastBetsMonitoringBetListService : ILastBetsMonitoringBetListService
    {
        private const int CacheTimeout = 300;
        private const string MatchListCacheKey = "LastBetsMatchList";
        private const string OutrightMatchListCacheKey = "LastBetsOutrightMatchList";
        private const string MatchListSpKey = "Lrf_Monitor_MatchSel";
        private const string OutrightMatchListSpKey = "Lrf_Monitor_OutrightSel";

        private static readonly IList<int> ConvertingDecimalOddsBetType = new List<int> { 1, 2, 3, 7, 8, 12, 20 };

        private readonly ILastBetsMonitoringBetListDataAccess _lastBetsMonitoringBetListDataAccess;

        private readonly IMemoryCacheService _cache;

        public LastBetsMonitoringBetListService(
            ILastBetsMonitoringBetListDataAccess lastBetsMonitoringBetListDataAccess,
            IMemoryCacheService cache)
        {
            _lastBetsMonitoringBetListDataAccess = lastBetsMonitoringBetListDataAccess;
            _cache = cache;
        }

        public async Task<TicketResult> GetLastBetsMonitoringData(LastBetsMonitoringMetaModel metaModel)
        {
            var ticketResult = await this._lastBetsMonitoringBetListDataAccess.GetLastBetsMonitoringData(metaModel).ConfigureAwait(false);

            await UpdateMatchInfo(metaModel, ticketResult).ConfigureAwait(false);

            return ticketResult;
        }

        #region Update Odds

        private static bool IsConvertToOdd(int betTypeId)
        {
            return ConvertingDecimalOddsBetType.Contains(betTypeId);
        }

        /// <summary>
        /// Convert odds of ticket  to Decimal odd.
        /// In case:
        /// Odd-type equals one or two ,converting HK to DEC.
        /// Odd-type equals three , converting MY to DEC.
        /// Odd-type equals four , converting INDO to DEC.
        /// Odd-type equals five , converting American to DEC.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        private static void ConvertToDecimalOdds(LrfTicket ticket)
        {
            switch (ticket.OddsType)
            {
                case OddsType.Decimal:
                case OddsType.Hongkong:
                    {
                        ticket.Odds++;
                        break;
                    }

                case OddsType.Malay:
                case OddsType.Indo:
                    {
                        ticket.Odds = ticket.Odds < 0 ? 1 - (1 / ticket.Odds) : ticket.Odds + 1;
                        break;
                    }

                case OddsType.American:
                    {
                        const int EvenOdds = 2;
                        var odds = ticket.Odds * 100;
                        if (odds < 0)
                        {
                            ticket.Odds = 1 - (1 / ticket.Odds);
                        }
                        else if (odds.IsEvenNumber())
                        {
                            ticket.Odds = EvenOdds;
                        }
                        else
                        {
                            ticket.Odds++;
                        }

                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        /// <summary>
        /// Converting odd type.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        private static void ConvertToMyOdds(LrfTicket ticket)
        {
            switch (ticket.OddsType)
            {
                case OddsType.Decimal:
                case OddsType.Hongkong:
                    {
                        if (ticket.Odds > 1)
                        {
                            ticket.Odds = -1 / ticket.Odds;
                        }

                        break;
                    }

                case OddsType.Malay:
                case OddsType.American: // AMERICAN To DEC
                    {
                        if (ticket.Odds < -1 || ticket.Odds > 1)
                        {
                            ticket.Odds = -1 / ticket.Odds;
                        }

                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        #endregion Update Odds

        #region Reload Match Info Cache

        private static bool IsOutright(int betType)
        {
            return betType == (int)BetType.Outright
                        || betType == (int)BetType.Win
                        || betType == (int)BetType.Place
                        || betType == (int)BetType.WinPlace;
        }

        /// <summary>
        /// Reload match list once if the match is not found.
        /// </summary>
        private async Task<IList<BetListMatch>> GetMatchList()
        {
            var data = _cache.Get<IList<BetListMatch>>(MatchListCacheKey);
            if (data == null)
            {
                data = await this._lastBetsMonitoringBetListDataAccess.GetMatchList(MatchListSpKey).ConfigureAwait(false);
                _cache.Set(MatchListCacheKey, data, CacheTimeout);
            }

            return data;
        }

        /// <summary>
        /// Reload outright match list once if the match is not found.
        /// </summary>
        private async Task<IList<BetListMatch>> GetOutrightMatchList()
        {
            var data = _cache.Get<IList<BetListMatch>>(OutrightMatchListCacheKey);
            if (data == null)
            {
                data = await this._lastBetsMonitoringBetListDataAccess.GetMatchList(OutrightMatchListSpKey).ConfigureAwait(false);
                _cache.Set(OutrightMatchListCacheKey, data, CacheTimeout);
            }

            return data;
        }

        #endregion Reload Match Info Cache

        /// <summary>
        /// Update match data for last bet monitoring.
        /// Update time zone.
        /// Update odds.
        /// </summary>
        /// <param name="metaModel">The metal mode.</param>
        /// <param name="ticketResult">The ticket result.</param>
        private async Task UpdateMatchInfo(LastBetsMonitoringMetaModel metaModel, TicketResult ticketResult)
        {
            var mainTicketList = ticketResult.MainTicketList;
            var timeZoneDelta = metaModel.TimeZone - (-4);
            var matchList = await GetMatchList().ConfigureAwait(false);
            var outrightMatchList = await GetOutrightMatchList().ConfigureAwait(false);
            foreach (LrfTicket ticket in mainTicketList)
            {
                var isOutright = IsOutright(ticket.BetTypeId);

                var match = (isOutright ? outrightMatchList : matchList).FirstOrDefault(x => x.MatchId == ticket.MatchId);

                if (match != null)
                {
                    ticket.SportTypeId = match.SportType;
                    ticket.LeagueId = match.LeagueId;
                    ticket.MatchCode = match.MatchCode;
                    ticket.HomeId = match.HomeId;
                    ticket.AwayId = match.AwayId;
                    ticket.Race = match.Race;
                    ticket.EventDate = match.EventDate;
                }

                // Update time zone
                if (timeZoneDelta > 0)
                {
                    ticket.TransDate = ticket.TransDate.ToUniversalTime().AddHours(timeZoneDelta).ToLocalTime();
                }

                if (metaModel.OddsType.ToLowerInvariant() == "dec" && IsConvertToOdd(ticket.BetTypeId))
                {
                    ConvertToDecimalOdds(ticket);
                }

                if (metaModel.OddsType.ToLowerInvariant() == "my" && IsConvertToOdd(ticket.BetTypeId))
                {
                    ConvertToMyOdds(ticket);
                }
            }
        }
    }
}