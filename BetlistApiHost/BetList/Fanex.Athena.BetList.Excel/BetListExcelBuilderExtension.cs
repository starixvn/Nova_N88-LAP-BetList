namespace Fanex.Athena.BetList.Excel
{
    using System.Globalization;
    using BetList.Model.DTO;
    using BetListNPOI.HSSF.UserModel;
    using BetListNPOI.SS.UserModel;
    using BetListNPOI.SS.Util;
    using Common.Utility.Helpers;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Resources;
    using Service.Helpers;

    public static class BetListExcelBuilderExtension
    {
        private const string NumberFormat = "{0:#,##0.00}";

        public static IBetListExcelBuilder AddCustomerWL(this IBetListExcelBuilder builder, ITicket ticket, bool showPlayerComm, IFont font)
        {
            var isContribution = BetListHelper.IsContribution(ticket.BetTypeId);

            var settledWL = showPlayerComm ? string.Format("{0:#,##0.00}\n{1:#,##0.00}", ticket.Winlost, ticket.PlayerComm) : string.Format("{0:#,##0.00}", ticket.Winlost);
            var winloss = BetListHelper.IsRunning(ticket.Status) || isContribution
                ? "-"
                : settledWL;

            var rtfCustomerWl = new HSSFRichTextString(winloss);
            rtfCustomerWl.ApplyFont(font);
            builder.AddCell(rtfCustomerWl);

            return builder;
        }

        public static IBetListExcelBuilder AddBetEffective(this IBetListExcelBuilder builder, LrfTicket ticket, IFont font)
        {
            var betEffective = string.Format("{0:#,##0.00}\n", ticket.BetEffective);

            var rtfBetEffective = new HSSFRichTextString(betEffective);
            rtfBetEffective.ApplyFont(font);
            builder.AddCell(rtfBetEffective);

            return builder;
        }

        public static IBetListExcelBuilder AddExternalId(
                this IBetListExcelBuilder builder,
                ITicketHelper ticketHelper,
                string externalId,
                ICellStyle oddStyle,
                ICellStyle evenStyle)
        {
            var rtfExternalId = new HSSFRichTextString(externalId);
            builder.AddCell(rtfExternalId, ticketHelper.Index.IsEvenNumber() ? oddStyle : evenStyle);

            return builder;
        }

        public static IBetListExcelBuilder AddFooterForSumStake(
            this IBetListExcelBuilder builder,
            decimal sumStake)
        {
            const int SumStakeColIndex = 6;

            var footerStyle = builder.CreateFooterCellStyle();

            var rtfTotal = new HSSFRichTextString(ViewResources.GrandTotal);
            for (int i = 0; i < SumStakeColIndex; i++)
            {
                builder.AddCell(rtfTotal, footerStyle);
            }

            builder.AddCell(new HSSFRichTextString(string.Format(NumberFormat, sumStake)), footerStyle);

            // add the rest columns
            var emptyText = new HSSFRichTextString();
            builder.AddCell(emptyText, footerStyle);
            builder.AddCell(emptyText, footerStyle);
            builder.AddCell(emptyText, footerStyle);
            builder.AddCell(emptyText, footerStyle);

            var lastRowIndex = builder.LastContentRow;
            builder.MergeCell(new CellRangeAddress(lastRowIndex, lastRowIndex, 0, SumStakeColIndex - 1), footerStyle);

            return builder;
        }

        public static IBetListExcelBuilder AddFooter(
            this IBetListExcelBuilder builder,
            decimal sumCustomerWL,
            decimal sumBetEffective,
            decimal sumLicenseeWLComm,
            IFont redFont,
            IFont blackFont)
        {
            const int StartFooterContentIndex = 7;

            var footerStyle = builder.CreateFooterCellStyle();

            var rtfTotal = new HSSFRichTextString(ViewResources.GrandTotal);
            for (int i = 0; i < StartFooterContentIndex; i++)
            {
                builder.AddCell(rtfTotal, footerStyle);
            }

            builder.AddCell(new HSSFRichTextString(string.Format(NumberFormat, sumCustomerWL)), footerStyle);

            builder.AddCell(new HSSFRichTextString(string.Format(NumberFormat, sumBetEffective)), footerStyle);

            var rtfSumLicenseeWl = new HSSFRichTextString(string.Format(NumberFormat, sumLicenseeWLComm));
            rtfSumLicenseeWl.ApplyFont(sumLicenseeWLComm >= 0 ? blackFont : redFont);
            builder.AddCell(rtfSumLicenseeWl, footerStyle);

            // add the rest column
            builder.AddCell(new HSSFRichTextString(), footerStyle);

            var lastRowIndex = builder.LastContentRow;
            builder.MergeCell(new CellRangeAddress(lastRowIndex, lastRowIndex, 0, StartFooterContentIndex - 1), footerStyle);

            return builder;
        }

        public static IBetListExcelBuilder AddHeader(this IBetListExcelBuilder builder, string custWLHeader)
        {
            var headerStyle = builder.CreateHeaderCellStyle();
            var captions = new[]
            {
                "#", ViewResources.ExternalId, ViewResources.SystemId, ViewResources.TransTime, ViewResources.Choice, ViewResources.Odds, ViewResources.Stake,
                custWLHeader, ViewResources.EffectiveBettingValue, ViewResources.LicenseeWL + "/" + ViewResources.Comm, ViewResources.Status
            };
            var columWidths = new short[] { 1500, 4800, 5800, 5400, 15500, 2600, 4200, 4200, 5400, 5400, 4000 };

            for (int i = 0; i < captions.Length; i++)
            {
                var cell = builder.AddCell(new HSSFRichTextString(captions[i]), headerStyle);
                builder.SetColumnWidth(cell.ColumnIndex, columWidths[i]);
            }

            return builder;
        }

        public static IBetListExcelBuilder AddIndex(this IBetListExcelBuilder builder, ITicketHelper ticketHelper, int index)
        {
            var rtfNo = new HSSFRichTextString(index.ToString(CultureInfo.InvariantCulture));
            builder.AddCell(
                            rtfNo,
                            ticketHelper.Index.IsEvenNumber() ? builder.OddCellStyleCenterAligned : builder.EvenCellStyleCenterAligned);

            return builder;
        }

        public static IBetListExcelBuilder AddLicenseeWL(
            this IBetListExcelBuilder builder,
            LrfTicket ticket,
            IFont redFont,
            IFont blackFont)
        {
            var isRunning = BetListHelper.IsRunning(ticket.Status);
            var isBoughtTicket = BetListHelper.IsBoughtTicket(ticket.BetTypeId);
            if (isBoughtTicket)
            {
                var licenseeWl = isRunning
                                    ? "-"
                                    : string.Format(NumberFormat, ticket.LicenseeWL);
                var comm = isRunning
                              ? "-"
                              : string.Format(NumberFormat, ticket.Comm);

                var licenseeWlText = string.Format("{0}\n{1}", licenseeWl, comm);

                var rtfLicenseeWl = new HSSFRichTextString(licenseeWlText);
                rtfLicenseeWl.ApplyFont(0, licenseeWl.Length, ticket.LicenseeWL >= 0 ? blackFont : redFont);
                rtfLicenseeWl.ApplyFont(licenseeWl.Length, licenseeWlText.Length, blackFont);

                builder.AddCell(rtfLicenseeWl);
            }
            else
            {
                var isContribution = BetListHelper.IsContribution(ticket.BetTypeId);

                var licenseeWl = isRunning || isContribution
                                    ? "-"
                                    : string.Format(NumberFormat, ticket.LicenseeWL + ticket.Comm);
                var rtfLicenseeWl = new HSSFRichTextString(licenseeWl);
                rtfLicenseeWl.ApplyFont(ticket.LicenseeWL + ticket.Comm >= 0 ? blackFont : redFont);
                builder.AddCell(rtfLicenseeWl);
            }

            return builder;
        }

        public static IBetListExcelBuilder AddSystemId(
            this IBetListExcelBuilder builder,
            ITicketHelper ticketHelper,
            string systemId,
            ICellStyle oddStyle,
            ICellStyle evenStyle)
        {
            var rtfSystemId = new HSSFRichTextString(systemId);
            builder.AddCell(rtfSystemId, ticketHelper.Index.IsEvenNumber() ? oddStyle : evenStyle);

            return builder;
        }

        public static ICellStyle CreateHeaderCellStyle(this IBetListExcelBuilder builder)
        {
            const short HeaderForegroundColor = 44;

            var style = builder.CreateCellStyle();
            style.CloneStyleFrom(builder.EvenCellStyle);
            style.FillForegroundColor = HeaderForegroundColor;
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.SetFont(builder.RTFHelper.PosFont);
            return style;
        }

        public static ICellStyle CreateFooterCellStyle(this IBetListExcelBuilder builder)
        {
            const short FooterForegroundColor = 47;

            var style = builder.CreateCellStyle();
            style.CloneStyleFrom(builder.EvenCellStyle);
            style.FillForegroundColor = FooterForegroundColor;
            style.Alignment = HorizontalAlignment.Right;
            style.BorderTop = BorderStyle.None;
            style.BorderLeft = BorderStyle.None;
            style.BorderBottom = BorderStyle.None;
            style.SetFont(builder.RTFHelper.PosFont);
            return style;
        }
    }
}