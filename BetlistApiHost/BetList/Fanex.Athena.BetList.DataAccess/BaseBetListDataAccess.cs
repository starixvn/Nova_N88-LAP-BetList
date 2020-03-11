// -----------------------------------------------------------------------
// <copyright file="BetListDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.DataAccess;
    using Common.Utility.Collections;
    using Common.Utility.Extensions;
    using Fanex.Athena.Common.DataAccess.Extensions;
    using Fanex.Athena.Common.DataAccess.Interfaces;
    using Fanex.BetList.Core.Entities;
    using Fanex.Common.Extensions;
    using Model.DTO;
    using Util;

    public class BaseBetListDataAccess : BaseDataAccess
    {
        private readonly ICurrencyDataAccess _currencyDataAccess;
        protected BaseBetListDataAccess(ICurrencyDataAccess currenyDataAccess)
        {
            _currencyDataAccess = currenyDataAccess;
        }

        protected async Task<IDictionary<int, string>> GetCurrencyListAsync()
        {
            return await _currencyDataAccess.GetCurrencyListAsync().ConfigureAwait(false);
        }


        [CLSCompliant(false)]
        protected internal static LrfTicket MapTicket(
                ISafeDataReader reader,
                IDictionary<string, string> columns,
                IDictionary<int, string> currencyList,
                long custId = 0)
        {
            var currencyId = reader.GetInt("currencyid");
            var ticket = new LrfTicket
            {
                CurrencyId = currencyId,
                CustId = reader.GetLong("custid", custId),
                Currency = currencyList?.GetValueOrDefault(currencyId, currencyId.ToString())
            };

            ReadStringData(ticket, reader);
            ReadBooleanData(ticket, reader);
            ReadDateTimeData(ticket, reader);
            ReadNumbericData(ticket, reader, columns);

            return ticket;
        }

        [CLSCompliant(false)]
        protected internal virtual SortableList<LrfTicket> ReadBetListTicket(
            IDataReader dataReader,
            IDictionary<int, string> currencyList,
            long custId = 0)
        {
            var columns = GetColumns();
            var tickets = new SortableList<LrfTicket>();

            var reader = this.GetSafeDataReader(dataReader, columns.Select(x => x.Value).ToList());
            while (reader.Read())
            {
                tickets.Add(MapTicket(reader, columns, currencyList, custId));
            }

            return tickets;
        }

        protected internal virtual ISafeDataReader GetSafeDataReader(IDataReader reader, IEnumerable<string> columns)
        {
            return new SafeDataReader(reader, columns);
        }

        protected static IDictionary<string, string> GetColumns(string baseCurrency = "")
        {
            var columns = new[]
             {
                "username", "refno", "matchcode", "status", "betteam", "betstatus", "showtime", "oddstype",
                "nickname", "race", "betfrom", "betcheck", "liveindicator", "isneutral", "sporttype", "bettype",
                "leagueid", "awayid", "homeid", "matchid", "custid", "currencyid", "livehomescore", "liveawayscore",
                "SpecialLevel", "StatusId", "transid", "betid", "winlostdate", "transdate",
                "odds", "modds", "hdp1", "hdp2", "transdesc", "currency", "betdaqid", "extrainfo", "originalbettype",
                "refcode", "wallettype", "oddsspread", "SettlementStatus", "eventdate"
            };
            var result = columns.ToDictionary(x => x, y => y);
            result.Add("playerCommCol", "playercomm");
            result.Add("commCol", "comm");
            result.Add("licenseeWlCol", "licenseewl");
            result.Add("winlostCol", "winlost");
            result.Add("beteffectiveCol", "beteffective");
            result.Add("stakeCol", "stake");
            result.Add("actualStakeCol", "actual_stake");
            result.Add("originalStakeCol", "originalstake");
            return result;
        }

        [CLSCompliant(false)]
        protected static IEnumerable<ITicketData> ReadReferneceTicket(IDataReader reader)
        {
            const string StakeColumn = "stake";
            const string BetteamColum = "betteam";

            var hasOrdinalBetTeam = reader.HasColumn(BetteamColum);
            var hasOrdinalStake = reader.HasColumn(StakeColumn);
            while (reader.Read())
            {
                var ticket = new TicketData
                {
                    BetTeam = hasOrdinalBetTeam ? reader.ToString(BetteamColum) : string.Empty,
                    RefNo = reader.ToString("refno"),
                    Status = reader.ToString("status"),
                    BetTypeId = reader.ToInt("bettype"),
                    Stake = hasOrdinalStake ? reader.ToDecimal(StakeColumn) : 0,
                    Odds = reader.ToDecimal("odds")
                };

                yield return ticket;
            }
        }

        private static void ReadStringData(LrfTicket ticket, ISafeDataReader reader)
        {
            ticket.UserName = reader.GetString("username");
            ticket.RefNo = reader.GetString("refno");
            ticket.MatchCode = reader.GetString("matchcode");
            ticket.Status = reader.GetString("status");
            ticket.BetTeam = reader.GetString("betteam");
            ticket.EventStatus = reader.GetString("betstatus");
            ticket.ShowTime = reader.GetString("showtime");
            ticket.OddsType = reader.GetString("oddstype", "0");
            ticket.ExternalId = reader.GetString("nickname");
            ticket.Race = reader.GetString("race");
            ticket.BetFrom = reader.GetString("betfrom");
            ticket.BetCheck = reader.GetString("betcheck");
            ticket.TransDesc = reader.GetString("transdesc");
            ticket.ExtraInfo = reader.GetString("extrainfo");
        }

        private static void ReadBooleanData(LrfTicket ticket, ISafeDataReader reader)
        {
            ticket.IsLive = reader.GetBool("liveindicator");
            ticket.IsNeutral = reader.GetBool("isneutral");
        }

        private static void ReadDateTimeData(LrfTicket ticket, ISafeDataReader reader)
        {
            ticket.WinlostDate = reader.GetDateTime("winlostdate", DateTime.Now);
            ticket.TransDate = reader.GetDateTime("transdate", DateTime.Now);
            ticket.EventDate = reader.GetDateTime("eventdate", DateTime.MinValue);
        }

        private static void ReadNumbericData(LrfTicket ticket, ISafeDataReader reader, IDictionary<string, string> columns)
        {
            // Int
            ticket.SportTypeId = reader.GetInt("sporttype");
            ticket.BetTypeId = reader.GetInt("bettype");
            ticket.LeagueId = reader.GetInt("leagueid");
            ticket.HomeId = reader.GetInt("homeid");
            ticket.AwayId = reader.GetInt("awayid");
            ticket.MatchId = reader.GetInt("matchid");
            ticket.LiveHomeScore = reader.GetInt("livehomescore");
            ticket.LiveAwayScore = reader.GetInt("liveawayscore");
            ticket.SpecialLevel = reader.GetInt("SpecialLevel");
            ticket.StatusId = reader.GetInt("StatusId");
            ticket.WalletType = (byte)reader.GetInt("wallettype");
            ticket.OriginalBetType = reader.GetInt("originalbettype");

            // Long
            ticket.TransId = reader.GetLong("transid");
            ticket.RefCode = reader.GetLong("refcode");
            ticket.BetId = reader.GetNullableLong("betid");
            ticket.BetdaqId = reader.GetLong("betdaqid", -1);

            // Money
            ticket.PlayerComm = reader.GetDecimal(columns["playerCommCol"]);
            ticket.Comm = reader.GetDecimal(columns["commCol"]);
            ticket.LicenseeWL = reader.GetDecimal(columns["licenseeWlCol"]);
            ticket.Winlost = reader.GetDecimal(columns["winlostCol"]);
            ticket.BetEffective = reader.GetDecimal(columns["beteffectiveCol"]);
            ticket.Stake = reader.GetDecimal(columns["stakeCol"]);
            ticket.ActualStake = reader.GetDecimal(columns["actualStakeCol"]);
            ticket.Odds = reader.GetDecimal("odds");
            ticket.MasterOdds = reader.GetDecimal("modds");
            ticket.Handicap1 = reader.GetDecimal("hdp1");
            ticket.Handicap2 = reader.GetDecimal("hdp2");
            ticket.OriginalStake = reader.GetDecimal(columns["originalStakeCol"], -1);
            ticket.OddsSpread = reader.GetDecimal("oddsspread");

            ticket.SettlementStatus = reader.GetByte("SettlementStatus");
        }
    }
}