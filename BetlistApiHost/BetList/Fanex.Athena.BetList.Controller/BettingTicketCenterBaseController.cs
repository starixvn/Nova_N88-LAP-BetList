namespace Fanex.Athena.BetList.Controller
{
    using System;
    using System.Web.Mvc;
    using Fanex.Athena.BetList.Controller.CustomActionResult;
    using Fanex.Athena.BetList.Excel;
    using Fanex.Athena.BetList.Excel.Model;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Athena.BetList.Service.Helpers;
    using Fanex.Athena.BetList.Service.Interfaces;
    using Fanex.Athena.Common.Constant;
    using Fanex.Athena.Resources;

    public class BettingTicketCenterBaseController : BaseController
    {
        private readonly IBettingTicketCenterExcel _bettingTicketCenterExcel;

        private readonly ITicketBuilderService _ticketBuilderService;

        public BettingTicketCenterBaseController(
            ITicketBuilderService ticketBuilderService,
            IBettingTicketCenterExcel bettingTicketCenterExcel)
        {
            _ticketBuilderService = ticketBuilderService;
            _bettingTicketCenterExcel = bettingTicketCenterExcel;
        }

        protected static string GetTitle(BaseBettingTicketMetaModel metaModel)
        {
            var date = metaModel.FromDate == DateTime.MinValue
                ? string.Empty
                : $", {metaModel.FromDate.ToString(FormatKey.DateFormat)}-->{metaModel.ToDate.ToString(FormatKey.DateFormat)}";

            var baseCurrencyName = metaModel.IsConvertAll ? metaModel.BaseCurrencyName : string.Empty;
            var baseCurrencyText = !string.IsNullOrEmpty(metaModel.BaseCurrency) ? metaModel.BaseCurrency.ToUpperInvariant() : baseCurrencyName;

            var baseCurrency = string.IsNullOrEmpty(baseCurrencyText)
                ? string.Empty
                : $", {ViewResources.BaseCurrency}: {baseCurrencyText}";

            return ViewResources.BettingTicketCenter + date + baseCurrency;
        }

        protected internal virtual ExcelResult ExportExcel(BettingTicketCenterExcelModel excelModel)
        {
            var workbook = _bettingTicketCenterExcel.Export(excelModel);
            BetListHelper.ClearData(excelModel.Data);
            excelModel.Data = null;
            return new ExcelResult(workbook);
        }

        protected internal virtual JsonResult ReturnData(TicketResult ticket, BettingTicketCenterOption option, ICommonValueMetaModel metaModel)
        {
#pragma warning disable S1067 // Expressions should not be too complex
            var result = new
            {
                Data = _ticketBuilderService.BuildTicketText(ticket, metaModel, option.IsAllowedCustomerComm),
                Total = new
                {
                    WinLoss = (option.ShowTotal && option.ShowWinLoss) ? _ticketBuilderService.BuildSumWinLoss(ticket, option.IsAllowedCustomerComm) : null,
                    LicenseeWL = (option.ShowTotal && option.ShowLicenseeWl) ? (ticket.SumLicenseeWL + ticket.SumComm).ToString(FormatKey.DecimalFormat) : null,
                    Stake = option.ShowSumStake ? ticket.SumStake.ToString(FormatKey.DecimalFormat) : null
                },
                ticket.TotalRows
            };
#pragma warning restore S1067 // Expressions should not be too complex

            BetListHelper.ClearData(ticket);
            return Json(result);
        }
    }
}