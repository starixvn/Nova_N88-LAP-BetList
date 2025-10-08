namespace Fanex.Athena.BetList.Service.Extensions
{
    using System;
    using System.Collections.Generic;
    using Common.Constant;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.BetList.Core.App_GlobalResources;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;

    using Helpers;
    using Resources;

    [CLSCompliant(false)]
    public static class LrfBetListHtmlBuilder
    {
        public static string BuildLrfStatus(
            this IBetListHTMLBuilder builder,
            LrfTicket ticket,
            ITicketHelper ticketHelper,
            List<ITicketData> ticketData,
            BetListMetaModel metaModel,
            bool isShowIpvn = false)
        {
            return builder.BuildStatus(ticket, ticketHelper, ticketData, isShowIpvn);
        }

        public static string BuildOddsUpperLower(this IBetListHTMLBuilder builder, decimal odds)
        {
            return string.Format("<div class='oddstype'>{0}</div>", Fanex.BetList.Core.Utils.ConvertByBetType.Odds(odds));
        }

        public static string BuildBookedBetStatus(this IBetListHTMLBuilder builder, LrfTicket ticket, Func<string, string, string> getResourceData, bool isShowIp = false)
        {
            const string PatternStatusBetlist = "<div class='status'>{0}</div>";
            const string PatternReason = "<div class='reason'>{0}:<br>{1}</div>";
            const string PatternIp = "<div class='ip'><br><div class='iplink'>{0}</div></div>";
            string ipHtml = isShowIp ? string.Format(PatternIp, ticket.IP) : string.Empty;

            switch (ticket.StatusId)
            {
                case BookedBetStatusTypes.Booking:
                case BookedBetStatusTypes.ProcressingBet:
                    return string.Format(PatternStatusBetlist, ViewResources.Booking) + ipHtml;

                case BookedBetStatusTypes.Successful:
                    return string.Format(PatternStatusBetlist, ViewResources.Successful) + ipHtml;

                case BookedBetStatusTypes.Unsuccessful:
                    return string.Format(PatternStatusBetlist, ViewResources.Unsuccessful) +
                           string.Format(PatternReason, ViewResources.Reason, getResourceData("BookedBet_Reason", ticket.Reason)) +
                           ipHtml;

                case BookedBetStatusTypes.Cancelled:
                    return string.Format(PatternStatusBetlist, ViewResources.Cancelled) + ipHtml;

                default:
                    return string.Format(PatternStatusBetlist, string.Empty) + ipHtml;
            }
        }

        // when updating this, remember to update Fanex.Athena.Excel.BetList.LrfBetListExcelBuilder as well
        public static string BuildLicenseeWL(this IBetListHTMLBuilder builder, LrfTicket ticket)
        {
            var isRunning = BetListHelper.IsRunning(ticket.Status);

            if (isRunning)
            {
                return "-";
            }

            return BetListHelper.IsBoughtTicket(ticket.BetTypeId)
                ? $"<div class ='lic-win-loss'>{FormatLicenseeWl(ticket.LicenseeWL)}<br/>{string.Format("{0:#,##0.00}", ticket.Comm)}</div>"
                : $"<div class='lic-win-loss'>{FormatLicenseeWl(ticket.LicenseeWL + ticket.Comm)}</div>";
        }

        public static string BuildCustomerWL(this IBetListHTMLBuilder builder, Ticket ticket, bool showPlayerComm)
        {
            var isRunning = BetListHelper.IsRunning(ticket.Status);
            var winloss = isRunning
                            ? "-"
                            : string.Format("<div class='underdog'>{0:#,##0.00}</div>", ticket.Winlost);

            if (!showPlayerComm)
            {
                return winloss;
            }

            winloss += isRunning
                ? string.Empty
                : string.Format("<div class='underdog'>{0:#,##0.00}</div>", ticket.PlayerComm);

            return winloss;
        }

        public static string BuildBetEffective(this IBetListHTMLBuilder builder, LrfTicket ticket)
        {
            return string.Format("<div class='underdog'>{0:#,##0.00}</div>", ticket.BetEffective);
        }

        public static string BuildLiveStatus(this IBetListHTMLBuilder builder, ITicket ticket, bool isShowIp = false)
        {
            const string PatternStatusBetlist = "<label style='color:{0}'>{1}</label>";
            const string PatternIp = "<div class='ip'><br><div class='iplink'>{0}</div></div>";
            string ipHtml = isShowIp ? string.Format(PatternIp, ticket.IP) : string.Empty;

            if (ticket.BetTypeId == (int)BetType.MixParlay || ticket.BetTypeId == (int)BetType.CombinationMixParlay)
            {
                switch (ticket.LiveHomeScore)
                {
                    case LiveStatusTypes.Live:
                        return string.Format(PatternStatusBetlist, "#cc0033", ViewResources.Live) + ipHtml;

                    case LiveStatusTypes.Mix:
                        return string.Format(PatternStatusBetlist, "#0033ff", ViewResources.MixLiveAndNoneLive) + ipHtml;

                    default:
                        return string.Format(PatternStatusBetlist, "#000", ViewResources.NoneLive) + ipHtml;
                }
            }

            return ticket.IsLive
                ? string.Format(PatternStatusBetlist, "#cc0033", ViewResources.Live) + ipHtml
                : string.Format(PatternStatusBetlist, "#000", ViewResources.NoneLive) + ipHtml;
        }

        public static string BuildSingleMatchParlaySelectionStatus(this IBetListHTMLBuilder builder, string status)
        {
            return GetStatusText(status);
        }

        #region Private Utility

        /// <summary>
        /// Format licensee win lost, show red text if the licensee win lost is negative.
        /// </summary>
        /// <param name="licenseeWl">Licensee win loss value.</param>
        /// <returns>Licensee win loss text.</returns>
        private static string FormatLicenseeWl(decimal licenseeWl)
        {
            return licenseeWl < 0
                ? string.Join(
                    null,
                    new[] { "<span class='negative-number'>", string.Format("{0:#,##0.00}", licenseeWl), "</span>" })
                : string.Format("{0:#,##0.00}", licenseeWl);
        }

        /// <summary>
        /// Get status text by status id.
        /// </summary>
        /// <param name="ticketStatus">LRF ticket status.</param>
        /// <returns>Status text.</returns>
        private static string GetStatusText(string ticketStatus)
        {
            switch (ticketStatus.ToLowerInvariant())
            {
                case BetStatus.Running:
                    return CoreBetList.running;

                case BetStatus.Reject:
                    return CoreBetList.reject;

                case BetStatus.Won:
                    return CoreBetList.won;

                case BetStatus.Lose:
                    return CoreBetList.lose;

                case BetStatus.Draw:
                    return CoreBetList.draw;

                case BetStatus.Waiting:
                    return CoreBetList.waiting;

                case BetStatus.Void:
                    return CoreBetList.voided;

                case BetStatus.Refund:
                    return CoreBetList.refund;

                default:
                    return string.Empty;
            }
        }

        #endregion Private Utility
    }
}