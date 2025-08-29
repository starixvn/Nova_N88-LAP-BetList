using System;
using BetListNPOI.HSSF.UserModel;
using BetListNPOI.HSSF.Util;
using BetListNPOI.SS.UserModel;
using BetListNPOI.SS.Util;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Extensions;
using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.Athena.Common.Constant;
using Fanex.Athena.Common.Utility.Helpers;
using Fanex.Athena.Resources;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.TicketHelper;

namespace Fanex.Athena.BetList.Excel.Builders
{
    public class BettingTicketCenterExcelBuilder : IBettingTicketCenterExcelBuilder, IDisposable
    {
        private const int NumberOfColumns = 15;

        private const int ColumnTotalStart = 10;

        private const int ContentRowIndex = 2;

        private readonly ITicketHelper _ticketHelper;

        private readonly IIPService _ipService;

        private bool disposed;

        private IBetListExcelBuilder _builder;

        private ICellStyle _evenCellLeftAlign;

        private ICellStyle _oddCellLeftAlign;

        private ICellStyle _evenCellCenterAlign;

        private ICellStyle _oddCellCenterAlign;

        private ICellStyle _evenCellRightAlign;

        private ICellStyle _oddCellRightAlign;

        // Default Fonts
        private IFont _negativeWinLossFont;

        private IFont _winLossFont;

        private IFont _liveFont;

        private IFont _mixLiveFont;

        private IFont _oddsFont;

        public BettingTicketCenterExcelBuilder(IBetListExcelBuilder builder, ITicketHelper ticketHelper, IIPService ipService)
        {
            _builder = builder ?? new BetListExcelBuilder();
            _ticketHelper = ticketHelper ?? new TicketHelper();
            _ipService = ipService;
        }

        public IWorkbook GetWorkbook()
        {
            return _builder.GetBetList();
        }

        public void CreateSheet(string sheetName, BettingTicketCenterOption option)
        {
            var cols = GetActualCols(option);
            _builder.BuildWorksheet(sheetName, (short)cols);
        }

        public void AddNoInfoRow(BettingTicketCenterOption option)
        {
            var titleStyle = _builder.CreateCellStyle();
            titleStyle.Alignment = HorizontalAlignment.Center;

            var mergeCol = GetActualCols(option) - 1;
            _builder.AddCell(new HSSFRichTextString(ViewResources.NoInfo), titleStyle);
            _builder.MergeCell(new CellRangeAddress(ContentRowIndex, ContentRowIndex, 0, mergeCol));
        }

        public void AddTitle(string title, BettingTicketCenterOption option)
        {
            var font = _builder.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;
            font.Color = HSSFColor.Orange.Index;

            var titleStyle = _builder.CreateCellStyle();
            titleStyle.VerticalAlignment = VerticalAlignment.Center;
            titleStyle.SetFont(font);

            var mergeCol = GetActualCols(option) - 1;
            _builder.AddCell(new HSSFRichTextString(title), titleStyle);
            _builder.MergeCell(new CellRangeAddress(0, 0, 0, mergeCol));
        }

#pragma warning disable S1541 // Methods and properties should not be too complex

        public void AddHeader(BettingTicketCenterOption option)
#pragma warning restore S1541 // Methods and properties should not be too complex
        {
            var columnWidths = new short[] { 1500, 4800, 4800, 5200, 5400, 5400, 15500, 3400, 3400, 2600, 4200, 4200, 5400, 5400, 3400, 4000 };
            var index = 0;

            var headerStyle = _builder.CreateHeaderCellStyle();
            AddHeaderCell("#", columnWidths[index++], headerStyle);
            if (option.ShowExternalId)
            {
                AddHeaderCell(ViewResources.ExternalId, columnWidths[index], headerStyle);
            }
            ++index;

            if (option.ShowSystemId)
            {
                AddHeaderCell(ViewResources.SystemId, columnWidths[index], headerStyle);
            }
            ++index;

            if (option.IsBookedBet)
            {
                AddHeaderCell(ViewResources.BookedId, columnWidths[index], headerStyle);
            }
            ++index;

            AddHeaderCell(ViewResources.RefNo, columnWidths[index++], headerStyle);

            AddHeaderCell(option.IsBookedBet ? ViewResources.BookedTime : ViewResources.TransTime, columnWidths[index++], headerStyle);

            AddHeaderCell(ViewResources.Choice, columnWidths[index++], headerStyle);

            if (option.IsBookedBet)
            {
                AddHeaderCell(ViewResources.OddsLower, columnWidths[index++], headerStyle);
                AddHeaderCell(ViewResources.OddsUpper, columnWidths[index++], headerStyle);
            }
            else
            {
                AddHeaderCell(ViewResources.Odds, columnWidths[index++], headerStyle);
                ++index;
            }

            AddHeaderCell(ViewResources.OddsType, columnWidths[index++], headerStyle);
            AddHeaderCell(ViewResources.Stake, columnWidths[index++], headerStyle);

            if (option.ShowWinLoss)
            {
                AddHeaderCell(ViewResources.WinLoss + (option.IsAllowedCustomerComm ? "/" + ViewResources.Comm : string.Empty), columnWidths[index], headerStyle);
            }
            ++index;

            if (option.ShowLicenseeWl)
            {
                AddHeaderCell(ViewResources.LicenseeWL + "/" + ViewResources.Comm, columnWidths[index], headerStyle);
            }
            ++index;

            AddHeaderCell(ViewResources.Status, columnWidths[index++], headerStyle);
            AddHeaderCell(ViewResources.Platform, columnWidths[index++], headerStyle);

            if (!option.IsBookedBet)
            {
                AddHeaderCell(ViewResources.Live, columnWidths[index], headerStyle);
            }
        }

        public void AddFooter(
            decimal sumStake,
            decimal sumWinLoss,
            decimal sumPlayerComm,
            decimal sumLicenseeWL,
            string totalText,
            BettingTicketCenterOption option)
        {
            var footerStyle = _builder.CreateFooterCellStyle();
            var cols = GetActualCols(option);
            var totalColIndex = ColumnTotalStart - (!option.ShowExternalId).ToInt() - (!option.ShowSystemId).ToInt() - (!option.IsBookedBet).ToInt() * 2;
            var currentTotalColIndex = totalColIndex;

            var rtfTotal = new HSSFRichTextString(totalText);
            for (int i = 0; i < totalColIndex; i++)
            {
                _builder.AddCell(rtfTotal, footerStyle);
            }

            _builder.AddCell(
                new HSSFRichTextString(option.ShowSumStake ? sumStake.ToString(FormatKey.DecimalFormat) : string.Empty),
                footerStyle);
            currentTotalColIndex++;

            if (option.ShowWinLoss)
            {
                var sumWinLossString = option.IsAllowedCustomerComm
                    ? string.Format("{0:#,##0.00}\n{1:#,##0.00}", sumWinLoss, sumPlayerComm)
                    : string.Format("{0:#,##0.00}", sumWinLoss);
                _builder.AddCell(new HSSFRichTextString(sumWinLossString), footerStyle);
                currentTotalColIndex++;
            }

            if (option.ShowLicenseeWl)
            {
                var rtfSumLicenseeWl = new HSSFRichTextString(sumLicenseeWL.ToString(FormatKey.DecimalFormat));
                rtfSumLicenseeWl.ApplyFont(sumLicenseeWL < 0 ? _negativeWinLossFont : _winLossFont);
                _builder.AddCell(rtfSumLicenseeWl, footerStyle);
                currentTotalColIndex++;
            }

            // add the rest column
            for (int i = currentTotalColIndex; i < cols; i++)
            {
                _builder.AddCell(new HSSFRichTextString(), footerStyle);
            }

            var lastRowIndex = _builder.LastContentRow;
            _builder.MergeCell(new CellRangeAddress(lastRowIndex, lastRowIndex, 0, totalColIndex - 1), footerStyle);
        }

#pragma warning disable S1541 // Methods and properties should not be too complex

        public void AssignRowData(TicketResult ticketResult, LrfTicket ticket, int licenseeId, BettingTicketCenterOption option)
#pragma warning restore S1541 // Methods and properties should not be too complex
        {
            _ticketHelper.Index++;
            _builder.AddIndex(_ticketHelper, _ticketHelper.Index);

            if (option.ShowExternalId)
            {
                _builder.AddExternalId(_ticketHelper, ticket.ExternalId, _oddCellLeftAlign, _evenCellLeftAlign);
            }

            if (option.ShowSystemId)
            {
                _builder.AddSystemId(_ticketHelper, ticket.UserName, _oddCellLeftAlign, _evenCellLeftAlign);
            }

            if (option.IsBookedBet)
            {
                AddBookedIdBetCell(ticket);
                AddBookedBetRefNoCell(ticket);
            }
            else
            {
                _builder.AddRefNo(ticket, _ticketHelper);
            }

            _builder.AddTransTime(ticket, _ticketHelper);
            _builder.AddChoice(ticket, _ticketHelper, ticketResult.TicketDataList);

            if (option.IsBookedBet)
            {
                AddBookedBetOddstCell(ticket.OddsLower);
                AddBookedBetOddstCell(ticket.OddsUpper);
            }
            else
            {
                _builder.AddOddsValue(ticket, ticketResult.TicketDataList, _ticketHelper.GetValidOddsTypeNameById);
            }

            AddOddType(ticket, ticketResult);
            _builder.AddStake(ticket, _ticketHelper);

            if (option.ShowWinLoss)
            {
                _builder.AddCustomerWL(ticket, option.IsAllowedCustomerComm, _winLossFont);
            }

            if (option.ShowLicenseeWl)
            {
                _builder.AddLicenseeWL(ticket, _negativeWinLossFont, _winLossFont);
            }

            _ticketHelper.IsVnIp = _ipService.IsVNIP;

            if (option.IsBookedBet)
            {
                AddBookedBetStatusCell(ticket, option.IsShowIpvn);
            }
            else
            {
                _builder.AddStatus(ticket, _ticketHelper, ticketResult.TicketDataList, option.IsShowIpvn);
            }

            AddPlatfromCell(ticket);

            if (!option.IsBookedBet)
            {
                AddLiveCell(ticket);
            }
        }

        public void InitializeCellStyles()
        {
            _oddCellLeftAlign = CreateCellStyle(HorizontalAlignment.Left, false);
            _evenCellLeftAlign = CreateCellStyle(HorizontalAlignment.Left, true);
            _oddCellRightAlign = CreateCellStyle(HorizontalAlignment.Right, false);
            _evenCellRightAlign = CreateCellStyle(HorizontalAlignment.Right, true);
            _oddCellCenterAlign = CreateCellStyle(HorizontalAlignment.Center, false);
            _evenCellCenterAlign = CreateCellStyle(HorizontalAlignment.Center, true);
        }

        public void InitializeFonts()
        {
            const short MixLiveColor = 13311;

            _negativeWinLossFont = _builder.CreateFont();
            _negativeWinLossFont.Boldweight = (short)FontBoldWeight.Bold;
            _negativeWinLossFont.Color = (short)FontColor.Red;

            _winLossFont = _builder.CreateFont();
            _winLossFont.Boldweight = (short)FontBoldWeight.Bold;
            _winLossFont.Color = (short)FontColor.None;

            _liveFont = _builder.CreateFont();
            _liveFont.Color = (short)FontColor.Red;

            _mixLiveFont = _builder.CreateFont();
            _mixLiveFont.Color = MixLiveColor;

            _oddsFont = _builder.CreateFont();
            _winLossFont.IsBold = true;
        }

        private static int GetActualCols(BettingTicketCenterOption option)
            => NumberOfColumns - (!option.ShowWinLoss).ToInt() - (!option.ShowLicenseeWl).ToInt() - (!option.ShowExternalId).ToInt() -
                (!option.ShowSystemId).ToInt() - (!option.IsBookedBet).ToInt();

        private void AddBookedIdBetCell(LrfTicket ticket)
        {
            var rtfBookedId = new HSSFRichTextString(ticket.TransId.ToString());
            BuilDataAlignedCenterCell(rtfBookedId);
        }

        private void AddBookedBetRefNoCell(LrfTicket ticket)
        {
            var bookedBetRefno = ticket.RefNo;
            ticket.RefNo = SystemConstants.DefaultBookedBetRefno;

            var rtfRefNo = new HSSFRichTextString(bookedBetRefno != "0" ? bookedBetRefno : string.Empty);
            BuilDataAlignedCenterCell(rtfRefNo);
        }

        private void AddBookedBetOddstCell(decimal odds)
        {
            var rtfOdds = new HSSFRichTextString(odds == 0 ? "-" : odds.ToString("#,##0.###"));
            rtfOdds.ApplyFont(_oddsFont);
            BuilDataAlignedRightCell(rtfOdds);
        }

        private void AddBookedBetStatusCell(LrfTicket ticket, bool isShowIp)
        {
            const string statusTmp = "{0} \n {1}";
            var ipText = isShowIp ? ticket.IP : string.Empty;

            string statusText;
            switch (ticket.StatusId)
            {
                case BookedBetStatusTypes.Booking:
                case BookedBetStatusTypes.ProcressingBet:
                    statusText = ViewResources.Booking;
                    break;

                case BookedBetStatusTypes.Successful:
                    statusText = ViewResources.Successful;
                    break;

                case BookedBetStatusTypes.Unsuccessful:
                    statusText = ViewResources.Unsuccessful + " \n " + ViewResources.Reason + ": \n " + _ticketHelper.GetResourceData("BookedBet_Reason", ticket.Reason);
                    break;

                case BookedBetStatusTypes.Cancelled:
                    statusText = ViewResources.Cancelled;
                    break;

                default:
                    statusText = string.Empty;
                    break;
            }

            BuilDataAlignedCenterCell(new HSSFRichTextString(string.Format(statusTmp, statusText, ipText)));
        }

        private void AddPlatfromCell(LrfTicket ticket)
        {
            var rtfPlatfrom = new HSSFRichTextString(PlatformType.GetPlatformName(ticket.BetFrom));
            BuilDataAlignedCenterCell(rtfPlatfrom);
        }

        private void AddOddType(LrfTicket ticket, TicketResult ticketResult)
        {
            _builder.AddOddsType(ticket, ticketResult.TicketDataList, _ticketHelper.GetValidOddsTypeNameById);
        }

        private void AddLiveCell(LrfTicket ticket)
        {
            var liveStatus = ticket.IsLive ? LiveStatusTypes.Live : 0;

            var status = ticket.BetTypeId == (int)BetType.MixParlay || ticket.BetTypeId == (int)BetType.CombinationMixParlay
                ? ticket.LiveHomeScore
                : liveStatus;

            switch (status)
            {
                case LiveStatusTypes.Live:
                    var rtfLive = new HSSFRichTextString(ViewResources.Live);
                    rtfLive.ApplyFont(_liveFont);
                    BuilDataAlignedCenterCell(rtfLive);
                    return;

                case LiveStatusTypes.Mix:
                    var rtfMixLive = new HSSFRichTextString(ViewResources.MixLiveAndNoneLive);
                    rtfMixLive.ApplyFont(_mixLiveFont);
                    BuilDataAlignedCenterCell(rtfMixLive);
                    return;

                default:
                    BuilDataAlignedCenterCell(new HSSFRichTextString(ViewResources.NoneLive));
                    return;
            }
        }

        private void BuilDataAlignedCenterCell(IRichTextString text)
        {
            _builder.AddCell(text, _ticketHelper.Index.IsEvenNumber() ? _evenCellCenterAlign : _oddCellCenterAlign);
        }

        private void BuilDataAlignedRightCell(IRichTextString text)
        {
            _builder.AddCell(text, _ticketHelper.Index.IsEvenNumber() ? _evenCellRightAlign : _oddCellRightAlign);
        }

        private ICellStyle CreateCellStyle(HorizontalAlignment align, bool isEvenCell)
        {
            const int VerticalAlignmentTop = 0;
            var cellStyle = _builder.CreateCellStyle();

            cellStyle.CloneStyleFrom(isEvenCell ? _builder.EvenCellStyle : _builder.OddCellStyle);
            cellStyle.Alignment = align;
            cellStyle.SetFont(_builder.RTFHelper.NormalFont);
            cellStyle.WrapText = true;
            cellStyle.VerticalAlignment = VerticalAlignmentTop;

            return cellStyle;
        }

        private void AddHeaderCell(string caption, short width, ICellStyle style)
        {
            var cell = _builder.AddCell(new HSSFRichTextString(caption), style);
            _builder.SetColumnWidth(cell.ColumnIndex, width);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing && _builder != null)
            {
                (_builder as IDisposable)?.Dispose();
                _builder = null;
                _evenCellLeftAlign = null;
                _oddCellLeftAlign = null;
                _oddCellRightAlign = null;
                _evenCellRightAlign = null;
                _evenCellCenterAlign = null;
                _oddCellCenterAlign = null;
                _negativeWinLossFont = null;
                _winLossFont = null;
                _liveFont = null;
                _mixLiveFont = null;
                _oddsFont = null;
            }

            disposed = true;
        }
    }
}