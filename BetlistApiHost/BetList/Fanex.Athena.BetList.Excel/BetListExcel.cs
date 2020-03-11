// --------------------------------------------------------------------------
// <copyright file="BaseBetList.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.BetList.Excel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Athena.BetList.Model.DTO;
    using Athena.BetList.Model.MetaModel;
    using Common.Constant;
    using Common.Utility.Services;
    using Common.Utility.Services.Interfaces;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Helpers;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using Resources;
    using Service.Extensions;

    public class BetListExcel
    {
        private const int ColumnSystemIdIndex = 2;

        private const int ColumnWinlossIndex = 7;

        private const int NumberOfColumn = 11;

        private const int ContentRowIndex = 2;

        private readonly IEnumerable<int> _hiddenColumnIndexes;

        private readonly int _licenseeId;

        private readonly ITicketHelper _ticketHelper;

        private readonly IAppSettings _appSettings;

        private readonly bool _isShowSumStake;

        private readonly BetListShowingOption _showingOption;

        private ICellStyle _evenCellStyleLeftAligned;

        // Default Fonts
        private IFont _negativeWinLossFont;

        private ICellStyle _oddCellStyleLeftAligned;

        private IFont _winLossFont;

        public BetListExcel(BetListShowingOption showingOption, int licenseeId, IEnumerable<int> hiddenColumnIndexes, bool isShowSumStake = false)
        {
            _appSettings = new AppSettings();
            _licenseeId = licenseeId;
            _ticketHelper = new TicketHelper();
            _isShowSumStake = isShowSumStake;
            _showingOption = showingOption;
            _hiddenColumnIndexes = hiddenColumnIndexes;
        }

        protected internal BetListExcel(
            IAppSettings appSettings,
            ITicketHelper ticketHelper,
            BetListShowingOption showingOption,
            int licenseeId,
            IEnumerable<int> hiddenColumnIndexes,
            bool isShowSumStake = false)
        {
            _appSettings = appSettings;
            _licenseeId = licenseeId;
            _ticketHelper = ticketHelper;
            _isShowSumStake = isShowSumStake;
            _showingOption = showingOption;
            _hiddenColumnIndexes = hiddenColumnIndexes;
        }

        public IWorkbook Export(TicketResult ticketResult, string title, int startIndex = 1)
        {
            var builder = this.CreateExcelBuilder();
            Build(builder, ticketResult, title, startIndex);
            var workbook = builder.GetBetList();
            ProcessUi(workbook);
            return workbook;
        }

        protected internal virtual IBetListExcelBuilder CreateExcelBuilder()
        {
            return new BetListExcelBuilder();
        }

        private static void AddNoInfoRow(IBetListExcelBuilder builder)
        {
            var titleStyle = builder.CreateCellStyle();
            titleStyle.Alignment = HorizontalAlignment.Center;

            builder.AddCell(new HSSFRichTextString(ViewResources.NoInfo), titleStyle);
            builder.MergeCell(new CellRangeAddress(ContentRowIndex, ContentRowIndex, 0, NumberOfColumn - 1));
        }

        private static void AddTitle(IBetListExcelBuilder builder, string title)
        {
            var font = builder.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;
            font.Color = HSSFColor.Orange.Index;

            var titleStyle = builder.CreateCellStyle();
            titleStyle.VerticalAlignment = VerticalAlignment.Center;
            titleStyle.SetFont(font);

            builder.AddCell(new HSSFRichTextString(title), titleStyle);
            builder.MergeCell(new CellRangeAddress(0, 0, 0, NumberOfColumn - 1));
        }

        protected internal virtual void Build(IBetListExcelBuilder builder, TicketResult ticketResult, string title, int startIndex = 1)
        {
            InitializeFonts(builder);
            InitializeCellStyles(builder);

            // Content
            if (ticketResult.MainTicketList.Any())
            {
                AssignSheetsData(builder, ticketResult, title, startIndex);
            }
            else
            {
                builder.BuildWorksheet(ViewResources.BetList, NumberOfColumn);
                AddTitle(builder, title);
                AddHeader(builder);
                AddNoInfoRow(builder);
            }
        }

        protected internal virtual void AssignSheetsData(IBetListExcelBuilder builder, TicketResult ticketResult, string title, int startIndex)
        {
            var sumStake = _isShowSumStake ? ticketResult.MainTicketList.Sum(x => x.Stake) : 0;

            var isShowWinLoss = !_isShowSumStake && _showingOption.GrandTotalVisible && !_hiddenColumnIndexes.Contains(ColumnWinlossIndex);
            decimal sumCustomerWl = 0;
            decimal sumLicenseeWl = 0;
            decimal sumBetEffective = 0;

            if (isShowWinLoss)
            {
                sumCustomerWl = ticketResult.SumWinLoss + ticketResult.SumPlayerComm;
                sumLicenseeWl = ticketResult.SumLicenseeWL + ticketResult.SumComm;
                sumBetEffective = ticketResult.SumBetEffective;
            }

            _ticketHelper.Index = startIndex;
            var numOfSheets = (int)Math.Ceiling((double)ticketResult.MainTicketList.Count / SystemConstants.SheetSizeExcel);
            for (int sheetIndex = 0; sheetIndex < numOfSheets; sheetIndex++)
            {
                builder.BuildWorksheet(ViewResources.Page + (sheetIndex + 1), NumberOfColumn);
                AddTitle(builder, title);
                AddHeader(builder);

                var endIndex = Math.Min((sheetIndex + 1) * SystemConstants.SheetSizeExcel, ticketResult.MainTicketList.Count);

                for (var i = sheetIndex * SystemConstants.SheetSizeExcel; i < endIndex; i++)
                {
                    BuildTicket(builder, ticketResult, ticketResult.MainTicketList[i]);
                    _ticketHelper.Index++;
                }

                if (_isShowSumStake)
                {
                    builder.AddFooterForSumStake(sumStake);
                }
                else
                {
                    if (isShowWinLoss)
                    {
                        builder.AddFooter(sumCustomerWl, sumBetEffective, sumLicenseeWl, _negativeWinLossFont, _winLossFont);
                    }
                }

                builder.GetBetList(); // Adjust row height for current sheet
            }
        }

        private void AddHeader(IBetListExcelBuilder builder)
        {
            var custWlheader = ViewResources.WinLoss + (_showingOption.CustomerCommVisible ? "/" + ViewResources.Comm : string.Empty);
            builder.AddHeader(custWlheader);
        }

        protected internal virtual void BuildTicket(IBetListExcelBuilder builder, TicketResult ticketResult, LrfTicket ticket)
        {
            builder.AddIndex(_ticketHelper, _ticketHelper.Index);
            builder.AddExternalId(_ticketHelper, ticket.ExternalId, _oddCellStyleLeftAligned, _evenCellStyleLeftAligned);
            builder.AddSystemId(_ticketHelper, ticket.UserName, _oddCellStyleLeftAligned, _evenCellStyleLeftAligned);
            builder.AddTrans(ticket, _ticketHelper);
            builder.AddChoice(ticket, _ticketHelper, ticketResult.TicketDataList);
            builder.AddOdds(ticket, ticketResult.TicketDataList, _ticketHelper.GetValidOddsTypeNameById);
            builder.AddStake(ticket);
            builder.AddCustomerWL(ticket, _showingOption.CustomerCommVisible, _winLossFont);
            builder.AddBetEffective(ticket, _winLossFont);
            builder.AddLicenseeWL(ticket, _licenseeId == LicenseeIdentity.OneTwoBet, _negativeWinLossFont, _winLossFont);
            builder.AddStatus(ticket, _ticketHelper, ticketResult.TicketDataList, false);
        }

        private void InitializeCellStyles(IBetListExcelBuilder builder)
        {
            _oddCellStyleLeftAligned = builder.CreateCellStyle();
            _oddCellStyleLeftAligned.CloneStyleFrom(builder.OddCellStyle);
            _oddCellStyleLeftAligned.Alignment = HorizontalAlignment.Left;
            _oddCellStyleLeftAligned.SetFont(builder.RTFHelper.NormalFont);
            _oddCellStyleLeftAligned.WrapText = true;

            _evenCellStyleLeftAligned = builder.CreateCellStyle();
            _evenCellStyleLeftAligned.CloneStyleFrom(builder.EvenCellStyle);
            _evenCellStyleLeftAligned.Alignment = HorizontalAlignment.Left;
            _evenCellStyleLeftAligned.SetFont(builder.RTFHelper.NormalFont);
            _evenCellStyleLeftAligned.WrapText = true;
        }

        protected internal virtual void InitializeFonts(IBetListExcelBuilder builder)
        {
            _negativeWinLossFont = builder.CreateFont();
            _negativeWinLossFont.Boldweight = (short)FontBoldWeight.Bold;
            _negativeWinLossFont.Color = (short)FontColor.Red;

            _winLossFont = builder.CreateFont();
            _winLossFont.Boldweight = (short)FontBoldWeight.Bold;
            _winLossFont.Color = (short)FontColor.None;
        }

        protected internal virtual void ProcessUi(IWorkbook workbook)
        {
            if (_hiddenColumnIndexes != null)
            {
                var columns = _hiddenColumnIndexes;
                if (_appSettings.FakeLicenseeList.Contains(_licenseeId) && !_hiddenColumnIndexes.Contains(ColumnSystemIdIndex))
                {
                    columns = columns.Concat(new[] { ColumnSystemIdIndex });
                }

                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    var sheet = workbook.GetSheetAt(i);

                    ExcelHelper.HideColumn(sheet, columns.ToArray());
                }
            }
        }
    }
}