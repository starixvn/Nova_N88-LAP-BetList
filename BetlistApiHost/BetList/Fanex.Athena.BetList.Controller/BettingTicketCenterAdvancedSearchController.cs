namespace Fanex.Athena.BetList.Controller
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Fanex.Athena.BetList.Controller.CustomActionResult;
    using Fanex.Athena.BetList.Controller.Utility;
    using Fanex.Athena.BetList.DataAccess.Interfaces;
    using Fanex.Athena.BetList.Excel;
    using Fanex.Athena.BetList.Excel.Model;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Athena.BetList.Service.Interfaces;
    using Fanex.Athena.Common.Utility.Services.Interfaces;
    using Fanex.Athena.Resources;

    public class BettingTicketCenterAdvancedSearchController : BettingTicketCenterBaseController
    {
        private const int RowsExceedLimitErrorCode = 1001;

        private readonly IAppSettings _appSettings;

        private readonly IBettingTicketCenterAdvancedSearchDataAccess _dataAccess;

        private readonly IBettingTicketCenterZipBuilder _zipBuilder;

        public BettingTicketCenterAdvancedSearchController(
            IAppSettings appSettings,
            IBettingTicketCenterAdvancedSearchDataAccess dataAccess,
            ITicketBuilderService ticketBuilderService,
            IBettingTicketCenterZipBuilder zipBuilder,
            IBettingTicketCenterExcel bettingTicketCenterExcel) : base(ticketBuilderService, bettingTicketCenterExcel)
        {
            _appSettings = appSettings;
            _dataAccess = dataAccess;
            _zipBuilder = zipBuilder ?? new BettingTicketCenterZipBuilder(_appSettings, null, bettingTicketCenterExcel);
        }

        private int MaxRowSupportExportExcel => int.Parse(_appSettings.GetValue("MaxRowSupportExportExcel"));

        private int MaxRowPerFile => int.Parse(_appSettings.GetValue("MaxRowPerExcelFile"));

        [HttpPost]
        public async Task<ActionResult> GetData(AdvancedSearchBetListMetaModel metaModel)
        {
            if (metaModel.IsExport)
            {
                var totalRow = await _dataAccess.GetTotalRowForMultipleCustomersAsync(metaModel).ConfigureAwait(false);
                if (totalRow > MaxRowSupportExportExcel)
                {
                    return Json(new
                    {
                        ErrorCode = RowsExceedLimitErrorCode
                    });
                }

                return await Export(metaModel, totalRow).ConfigureAwait(false);
            }

            var ticketResult = await _dataAccess.GetBetListForMultipleCustomersAsync(metaModel
                ).ConfigureAwait(false);

            return ReturnData(ticketResult, GetBetListOption(metaModel.HasPlayerCom), metaModel);
        }

        private async Task<ActionResult> Export(AdvancedSearchBetListMetaModel metaModel,
            int totalRow)
        {
            var title = GetTitle(metaModel);
            var totalText = $"{ViewResources.Totalin} {metaModel.BaseCurrencyName}";

            if (totalRow <= MaxRowPerFile)
            {
                var normalTicketResult = await _dataAccess.GetBetListForMultipleCustomersAsync(metaModel).ConfigureAwait(false);
                if (normalTicketResult.MainTicketList.Count == 0)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NoContent);
                }

                return ExportXls(normalTicketResult, title, metaModel.HasPlayerCom, totalText);
            }

            var ticketResult = await _dataAccess.GetBetListForMultipleCustomersAsync(metaModel, MaxRowPerFile
                ).ConfigureAwait(false);

            var zipFile = _zipBuilder.BuildZip(HttpContext.Response,
                ticketResult, title, metaModel.HasPlayerCom, totalText);

            return zipFile == null ? null : ZipContentResult(zipFile);
        }

        private ExcelResult ExportXls(TicketResult ticketResult, string title, bool hasPlayerComm, string totalText)
        {
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = ticketResult,
                Option = GetBetListOption(hasPlayerComm),
                Title = title,
                TotalText = totalText
            };

            return ExportExcel(excelModel);
        }

        protected FileContentResult ZipContentResult(MemoryStream stream)
        {
            var bytes = stream.ToArray();
            return File(bytes, "application/zip", "BettingTicketsCenter_AdvancedSearch.zip");
        }

        private static BettingTicketCenterOption GetBetListOption(bool hasPlayerComm) => new BettingTicketCenterOption
        {
            ShowTotal = true,
            ShowSumStake = true,
            ShowLicenseeWl = true,
            ShowWinLoss = true,
            ShowExternalId = true,
            ShowSystemId = true,
            IsAllowedCustomerComm = hasPlayerComm
        };

    }
}