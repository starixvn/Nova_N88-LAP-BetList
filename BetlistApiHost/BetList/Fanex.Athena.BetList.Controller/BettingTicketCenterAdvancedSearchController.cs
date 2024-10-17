namespace Fanex.Athena.BetList.Controller
{
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
    using Newtonsoft.Json;

    public class BettingTicketCenterAdvancedSearchController : BettingTicketCenterBaseController
    {
        private const int RowsExceedLimitErrorCode = 1001;

        private readonly IAppSettings _appSettings;

        private readonly IBettingTicketCenterAdvancedSearchDataAccess _dataAccess;

        private readonly IBettingTicketCenterZipBuilder _zipBuilder;

        private readonly ITempFileService _tempFileService;

        public BettingTicketCenterAdvancedSearchController(
            IAppSettings appSettings,
            IBettingTicketCenterAdvancedSearchDataAccess dataAccess,
            IIPService ipService,
            ITicketBuilderService ticketBuilderService,
            IBettingTicketCenterZipBuilder zipBuilder,
            IBettingTicketCenterExcel bettingTicketCenterExcel,
            ITempFileService tempFileService) : base(ticketBuilderService, bettingTicketCenterExcel)
        {
            _appSettings = appSettings;
            _dataAccess = dataAccess;
            _zipBuilder = zipBuilder ?? new BettingTicketCenterZipBuilder(_appSettings, null, bettingTicketCenterExcel, ipService);
            _tempFileService = tempFileService;
        }

        private int MaxRowSupportExportExcel => int.Parse(_appSettings.GetValue("MaxRowSupportExportExcel"));

        private int MaxRowPerFile => int.Parse(_appSettings.GetValue("MaxRowPerExcelFile"));

        [HttpPost]
        public async Task<ActionResult> GetData(AdvancedSearchBetListMetaModel metaModel)
        {
            if (metaModel.IsExport)
            {
                var totalRow = await _dataAccess.GetTotalRowForMultipleCustomersAsync(metaModel);
                if (totalRow > MaxRowSupportExportExcel)
                {
                    return Json(new
                    {
                        ErrorCode = RowsExceedLimitErrorCode
                    });
                }

                return await Export(metaModel).ConfigureAwait(false);
            }

            var ticketResult = await _dataAccess.GetBetListForMultipleCustomersAsync(metaModel);

            return ReturnData(ticketResult, GetBetListOption(metaModel.HasPlayerCom, metaModel.IsShowIpvn), metaModel);
        }

        private async Task<ActionResult> Export(AdvancedSearchBetListMetaModel metaModel)
        {
            var title = GetTitle(metaModel);
            var totalText = $"{ViewResources.Totalin} {metaModel.BaseCurrencyName}";

            var ticketResult = await _dataAccess.GetBetListForMultipleCustomersAsync(metaModel, MaxRowPerFile);

            if (ticketResult.TotalRows <= MaxRowPerFile)
            {
                if (ticketResult.TotalRows == 0)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NoContent);
                }

                var tickets = GetTicketFromFile(ticketResult);
                return ExportXls(tickets, title, metaModel.HasPlayerCom, metaModel.IsShowIpvn, totalText);
            }

            var zipFile = _zipBuilder.BuildZip(HttpContext.Response, ticketResult, title, metaModel, totalText);

            return zipFile == null ? null : ZipContentResult(zipFile);
        }

        private ExcelResult ExportXls(TicketResult ticketResult, string title, bool hasPlayerComm, bool isShowIpvn, string totalText)
        {
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = ticketResult,
                Option = GetBetListOption(hasPlayerComm, isShowIpvn),
                Title = title,
                TotalText = totalText
            };

            return ExportExcel(excelModel);
        }

        protected FileContentResult ZipContentResult(MemoryStream stream)
        {
            var bytes = stream.ToArray();
            return File(bytes, "application/zip", "BetHistory_AdvancedSearch.zip");
        }

        private static BettingTicketCenterOption GetBetListOption(bool hasPlayerComm, bool isShowIpvn) => new BettingTicketCenterOption
        {
            ShowTotal = true,
            ShowSumStake = true,
            ShowLicenseeWl = true,
            ShowWinLoss = true,
            ShowExternalId = true,
            ShowSystemId = true,
            IsAllowedCustomerComm = hasPlayerComm,
            IsShowIpvn = isShowIpvn
        };

        private TicketResult GetTicketFromFile(GroupTicketResult groupTicket)
        {
            var contentData = _tempFileService.ReadString(groupTicket.DataPath[0]);
            _tempFileService.Delete(groupTicket.DataPath[0]);
            var ticketResult = JsonConvert.DeserializeObject<TicketResult>(contentData);

            ticketResult.SumStake = groupTicket.SumStake;
            ticketResult.SumWinLoss = groupTicket.SumWinLoss;
            ticketResult.SumLicenseeWL = groupTicket.SumLicenseeWL;
            ticketResult.SumComm = groupTicket.SumComm;
            ticketResult.SumPlayerComm = groupTicket.SumPlayerComm;
            ticketResult.TotalRows = groupTicket.TotalRows;
            return ticketResult;
        }
    }
}