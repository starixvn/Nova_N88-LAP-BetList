namespace Fanex.Athena.BetList.Controller
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Fanex.Athena.BetList.DataAccess.Interfaces;
    using Fanex.Athena.BetList.Excel;
    using Fanex.Athena.BetList.Excel.Model;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Athena.BetList.Service.Interfaces;
    using Fanex.Athena.Resources;

    public class BettingTicketCenterController : BettingTicketCenterBaseController
    {
        private readonly IBettingTicketCenterDataAccess _dataAccess;

        public BettingTicketCenterController(
            IBettingTicketCenterDataAccess dataAccess,
            ITicketBuilderService ticketBuilderService,
            IBettingTicketCenterExcel bettingTicketCenterExcel)
            : base (ticketBuilderService, bettingTicketCenterExcel)
        {
            _dataAccess = dataAccess;
        }

        [HttpPost]
        public async Task<ActionResult> GetBySingleCustomer(SingleCustomersBetListMetaModel metaModel)
        {
            var ticketResult = await _dataAccess.GetBetListForSingleCustomerAsync(metaModel
              ).ConfigureAwait(false);

            var option = new BettingTicketCenterOption
            {
                ShowTotal = true,
                ShowSumStake = true,
                ShowLicenseeWl = true,
                ShowWinLoss = true,
                ShowExternalId = true,
                ShowSystemId = true,
                IsAllowedCustomerComm = metaModel.HasPlayerCom
            };

            if (metaModel.IsExport)
            {
                return Export(ticketResult, option, metaModel, ViewResources.Total);
            }

            return ReturnData(ticketResult, option, metaModel);
        }

        [HttpPost]
        public async Task<ActionResult> GetByRefno(RefNoBetListMetaModel metaModel)
        {
            var ticketResult = await _dataAccess.GetBetListByRefnoAsync(metaModel
             ).ConfigureAwait(false);

            var option = new BettingTicketCenterOption
            {
                ShowLicenseeWl = true,
                ShowWinLoss = true,
                ShowTotal = false,
                ShowExternalId = true,
                ShowSystemId = true,
                IsAllowedCustomerComm = metaModel.HasPlayerCom
            };

            if (metaModel.IsExport)
            {
                return Export(ticketResult, option, metaModel, null);
            }

            return ReturnData(ticketResult, option, metaModel);
        }

        [HttpPost]
        public async Task<ActionResult> GetByRunning(BaseBettingTicketMetaModel metaModel)
        {
            var ticketResult = await _dataAccess.GetRunningBetListAsync(metaModel).ConfigureAwait(false);

            var option = new BettingTicketCenterOption
            {
                ShowTotal = true,
                ShowSumStake = true,
                IsAllowedCustomerComm = false,
                ShowExternalId = true,
                ShowSystemId = true
            };

            if (metaModel.IsExport)
            {
                return Export(ticketResult, option, metaModel, $"{ViewResources.Totalin} {metaModel.BaseCurrencyName}");
            }

            return ReturnData(ticketResult, option, metaModel);
        }

        private ActionResult Export(TicketResult ticket, BettingTicketCenterOption option, BaseBettingTicketMetaModel metaModel, string totalText)
        {
            if (ticket.MainTicketList.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }

            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = ticket,
                Option = option,
                Title = GetTitle(metaModel),
                TotalText = totalText
            };

            return ExportExcel(excelModel);
        }
    }
}