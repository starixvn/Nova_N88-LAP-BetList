using System;
using System.Collections.Generic;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Extensions;
using Fanex.Athena.BetList.Service.Helpers;
using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.Athena.Common.Constant;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace Fanex.Athena.BetList.Service
{
    public class TicketBuilderService : ITicketBuilderService
    {
        private readonly IBetListHTMLBuilder _betlistHtmlBuilder;

        private readonly ITicketHelper _ticketHelper;

        private readonly IIPService _ipService;

        private const string HyphenTextHTML = "<div class='underdog'>-</div>";

        [CLSCompliant(false)]
        public TicketBuilderService(
            IBetListHTMLBuilder betlistHtmlBuilder,
            ITicketHelper ticketHelper,
            IIPService ipService)
        {
            _betlistHtmlBuilder = betlistHtmlBuilder;
            _ticketHelper = ticketHelper;
            _ipService = ipService;
        }

        public IList<TicketText> BuildBookedBetTicketText(TicketResult ticketResult, ICommonValueMetaModel metaModel)
        {
            var startIndex = ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1;
            var ticketTextList = new List<TicketText>();

            _ticketHelper.IsVnIp = _ipService.IsVNIP;

            foreach (var ticket in ticketResult.MainTicketList)
            {
                var bookedBetRefno = ticket.RefNo;
                ticket.RefNo = SystemConstants.DefaultBookedBetRefno;

                var ticketText = new TicketText
                {
                    Index = startIndex++,
                    ExternalId = ticket.ExternalId,
                    SystemId = ticket.UserName,
                    BookedTime = _betlistHtmlBuilder.BuildTransTime(ticket),
                    RefNo = bookedBetRefno != "0" ? bookedBetRefno : string.Empty,
                    BookedId = ticket.TransId.ToString(),
                    Choice = _betlistHtmlBuilder.BuildChoice(ticket, _ticketHelper, ticketResult.TicketDataList, false),
                    OddsLower = ticket.OddsLower == 0 ? HyphenTextHTML : _betlistHtmlBuilder.BuildOddsUpperLower(ticket.OddsLower),
                    OddsUpper = ticket.OddsUpper == 0 ? HyphenTextHTML : _betlistHtmlBuilder.BuildOddsUpperLower(ticket.OddsUpper),
                    OddsType = BuildOddsTypeNameText(ticket, ticketResult),
                    Stake = _betlistHtmlBuilder.BuildStake(ticket, _ticketHelper),
                    Status = _betlistHtmlBuilder.BuildBookedBetStatus(ticket, _ticketHelper.GetResourceData, metaModel.IsShowIpvn),
                    Platform = PlatformType.GetPlatformName(ticket.BetFrom)
                };

                ticketTextList.Add(ticketText);
            }

            return ticketTextList;
        }

        public IList<TicketText> BuildTicketText(TicketResult ticketResult, ICommonValueMetaModel metaModel, bool isAllowedCustomerComm)
        {
            var startIndex = ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1;
            var ticketTextList = new List<TicketText>();
            _ticketHelper.IsVnIp = _ipService.IsVNIP;

            foreach (var ticket in ticketResult.MainTicketList)
            {
                if (ticket.BetTypeId == (int)BetType.CombinationMixParlay)
                {
                    _ticketHelper.ClientDetailSystemParlayFunction = BetListHelper.GenerateClientDetailSystemParlayFunction(
                        _ticketHelper,
                        metaModel.BaseCurrency,
                        ticket,
                        ticket.CustId);
                }

                var ticketText = new TicketText
                {
                    Index = startIndex++,
                    ExternalId = ticket.ExternalId,
                    SystemId = ticket.UserName,
                    TransTime = _betlistHtmlBuilder.BuildTransTime(ticket),
                    RefNo = _betlistHtmlBuilder.BuildRefNo(ticket),
                    Choice = _betlistHtmlBuilder.BuildChoice(ticket, _ticketHelper, ticketResult.TicketDataList, false),
                    Odds = BuildOddsText(ticket, ticketResult),
                    OddsType = BuildOddsTypeNameText(ticket, ticketResult),
                    Stake = _betlistHtmlBuilder.BuildStake(ticket, _ticketHelper),
                    LicenseeWL = _betlistHtmlBuilder.BuildLicenseeWL(ticket),
                    WinLoss = _betlistHtmlBuilder.BuildCustomerWL(ticket, isAllowedCustomerComm),
                    Status = _betlistHtmlBuilder.BuildStatus(ticket, _ticketHelper, ticketResult.TicketDataList, metaModel.IsShowIpvn),
                    Platform = PlatformType.GetPlatformName(ticket.BetFrom),
                    Live = _betlistHtmlBuilder.BuildLiveStatus(ticket)
                };

                if (BetListHelper.IsContribution(ticket.BetTypeId))
                {
                    BuildContributionTicket(ticketText);
                }

                ticketTextList.Add(ticketText);
            }

            return ticketTextList;
        }

        public List<TicketText> BuildTicketText(
            TicketResult ticketResult,
            BetListMetaModel metaModel = null,
            int startIndex = 1,
            bool showOdds = false,
            bool isAllowedCustomerComm = false,
            bool isShowIpvn = false)
        {
            var baseCurrency = metaModel == null ? string.Empty : metaModel.BaseCurrency;
            var ticketTextList = new List<TicketText>();

            _ticketHelper.IsShowOddsWithChoice = showOdds;
            _ticketHelper.IsVnIp = _ipService.IsVNIP;

            foreach (var ticket in ticketResult.MainTicketList)
            {
                if (ticket.BetTypeId == (int)BetType.CombinationMixParlay && metaModel != null)
                {
                    var custid = ticket.CustId != 0 ? ticket.CustId : metaModel.CustId;
                    _ticketHelper.ClientDetailSystemParlayFunction = BetListHelper.GenerateClientDetailSystemParlayFunction(_ticketHelper, baseCurrency, ticket, custid);
                }

                var ticketText = new TicketText
                {
                    Index = startIndex++,
                    ExternalId = ticket.ExternalId,
                    SystemId = ticket.UserName,
                    TransTime = _betlistHtmlBuilder.BuildTrans(ticket),
                    Status = _betlistHtmlBuilder.BuildStatus(ticket, _ticketHelper, ticketResult.TicketDataList, (metaModel != null) && metaModel.IsShowIpvn || isShowIpvn),
                    Choice = _betlistHtmlBuilder.BuildChoice(ticket, _ticketHelper, ticketResult.TicketDataList, false),
                    Odds = BuildOddsText(ticket, ticketResult),
                    Stake = _betlistHtmlBuilder.BuildStake(ticket, _ticketHelper),
                    LicenseeWL = _betlistHtmlBuilder.BuildLicenseeWL(ticket),
                    WinLoss = _betlistHtmlBuilder.BuildCustomerWL(ticket, isAllowedCustomerComm),
                    BetEffective = _betlistHtmlBuilder.BuildBetEffective(ticket),
                    TransId = ticket.TransId.ToString()
                };

                if (BetListHelper.IsContribution(ticket.BetTypeId))
                {
                    BuildContributionTicket(ticketText);
                }

                ticketTextList.Add(ticketText);
            }

            return ticketTextList;
        }

        private string BuildOddsText(LrfTicket ticket, TicketResult ticketResult)
        {
            return ticket.BetTypeId != (int)BetType.CombinationMixParlay || ticket.OddsSpread != BetListHelper.MixParlayOddsSpread
                ? _betlistHtmlBuilder.BuildOddsValue(ticket, ticketResult.TicketDataList, _ticketHelper.GetValidOddsTypeNameById)
                : _betlistHtmlBuilder.BuildOdds(ticket, ticketResult.TicketDataList, _ticketHelper.GetValidOddsTypeNameById);
        }

        private string BuildOddsTypeNameText(LrfTicket ticket, TicketResult ticketResult)
        {
            return ticket.BetTypeId != (int)BetType.CombinationMixParlay
                ? _betlistHtmlBuilder.BuildOddsTypeName(ticket, ticketResult.TicketDataList, _ticketHelper.GetValidOddsTypeNameById)
                : null;
        }

        public string BuildSumWinLoss(TicketResult ticketResult, bool isAllowedCustomerComm)
        {
            var ticket = new LrfTicket { Winlost = ticketResult.SumWinLoss, PlayerComm = ticketResult.SumPlayerComm };
            return _betlistHtmlBuilder.BuildCustomerWL(ticket, isAllowedCustomerComm);
        }

        private static void BuildContributionTicket(TicketText ticketText)
        {
            const string HyphenTextHTML = "<div class='underdog'>-</div>";

            ticketText.LicenseeWL = HyphenTextHTML;
            ticketText.WinLoss = HyphenTextHTML;
        }
    }
}