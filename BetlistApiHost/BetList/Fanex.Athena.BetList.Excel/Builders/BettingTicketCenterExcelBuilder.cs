using System;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Extensions;
using Fanex.Athena.Common.Constant;
using Fanex.Athena.Common.Utility.Helpers;
using Fanex.Athena.Resources;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.TicketHelper;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Fanex.Athena.BetList.Excel.Builders
{
    public class BettingTicketCenterExcelBuilder : IBettingTicketCenterExcelBuilder, IDisposable
    {
        private const int NumberOfColumns = 14;

        private const int ColumnTotalStart = 8;

        private const int ContentRowIndex = 2;

        private readonly ITicketHelper _ticketHelper;

        private bool disposed;

        private IBetListExcelBuilder _builder;

        private ICellStyle _evenCellLeftAlign;

        private ICellStyle _oddCellLeftAlign;

        private ICellStyle _evenCellCenterAlign;

        private ICellStyle _oddCellCenterAlign;

        // Default Fonts
        private IFont _negativeWinLossFont;

        private IFont _winLossFont;

        private IFont _liveFont;

        private IFont _mixLiveFont;

        public BettingTicketCenterExcelBuilder(IBetListExcelBuilder builder, ITicketHelper ticketHelper)
        {
            _builder = builder ?? new BetListExcelBuilder();
            _ticketHelper = ticketHelper ?? new TicketHelper();
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

        public void AddHeader(BettingTicketCenterOption option)
        {
            var columnWidths = new short[] { 1500, 4800, 4800, 5400, 5400, 15500, 2600, 2600, 4200, 4200, 5400, 5400, 3400, 3400 };
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

            AddHeaderCell(ViewResources.RefNo, columnWidths[index++], headerStyle);
            AddHeaderCell(ViewResources.TransTime, columnWidths[index++], headerStyle);
            AddHeaderCell(ViewResources.Choice, columnWidths[index++], headerStyle);
            AddHeaderCell(ViewResources.Odds, columnWidths[index++], headerStyle);
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
            AddHeaderCell(ViewResources.Live, columnWidths[index], headerStyle);
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
            var totalColIndex = ColumnTotalStart - (!option.ShowExternalId).ToInt() - (!option.ShowSystemId).ToInt();
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

        public void AssignRowData(TicketResult ticketResult, LrfTicket ticket, int licenseeId, BettingTicketCenterOption option)
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

            _builder.AddRefNo(ticket, _ticketHelper);
            _builder.AddTransTime(ticket, _ticketHelper);
            _builder.AddChoice(ticket, _ticketHelper, ticketResult.TicketDataList);
            _builder.AddOddsValue(ticket, ticketResult.TicketDataList, _ticketHelper.GetValidOddsTypeNameById);
            _builder.AddOddsType(ticket, ticketResult.TicketDataList, _ticketHelper.GetValidOddsTypeNameById);
            _builder.AddStake(ticket);

            if (option.ShowWinLoss)
            {
                _builder.AddCustomerWL(ticket, option.IsAllowedCustomerComm, _winLossFont);
            }

            if (option.ShowLicenseeWl)
            {
                _builder.AddLicenseeWL(ticket, licenseeId == LicenseeIdentity.OneTwoBet, _negativeWinLossFont, _winLossFont);
            }

            _builder.AddStatus(ticket, _ticketHelper, ticketResult.TicketDataList, false);
            AddPlatfromCell(ticket);
            AddLiveCell(ticket);
        }

        public void InitializeCellStyles()
        {
            _oddCellLeftAlign = CreateCellStyle(HorizontalAlignment.Left, false);
            _evenCellLeftAlign = CreateCellStyle(HorizontalAlignment.Left, true);
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
        }

        private static int GetActualCols(BettingTicketCenterOption option)
            => NumberOfColumns - (!option.ShowWinLoss).ToInt() - (!option.ShowLicenseeWl).ToInt() - (!option.ShowExternalId).ToInt() - (!option.ShowSystemId).ToInt();

        private void AddPlatfromCell(LrfTicket ticket)
        {
            var rtfPlatfrom = new HSSFRichTextString(PlatformType.GetPlatformName(ticket.BetFrom));
            BuilDataAlignedCenterCell(rtfPlatfrom);
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
            _builder.AddCell(text, _ticketHelper.Index.IsEvenNumber() ? _oddCellCenterAlign : _evenCellCenterAlign);
        }

        private ICellStyle CreateCellStyle(HorizontalAlignment align, bool isEvenCell)
        {
            var cellStyle = _builder.CreateCellStyle();
            cellStyle.CloneStyleFrom(isEvenCell ? _builder.EvenCellStyle : _builder.OddCellStyle);
            cellStyle.Alignment = align;
            cellStyle.SetFont(_builder.RTFHelper.NormalFont);
            cellStyle.WrapText = true;
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
                _evenCellCenterAlign = null;
                _oddCellCenterAlign = null;
                _negativeWinLossFont = null;
                _winLossFont = null;
                _liveFont = null;
                _mixLiveFont = null;
            }

            disposed = true;
        }
    }
}