using System.Threading.Tasks;
using System.Web.Mvc;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Helpers;
using Fanex.Athena.BetList.Service.Interfaces;

namespace Fanex.Athena.BetList.Controller
{
    public class BetListByProductController : BaseController
    {
        private readonly IBetListByProductService _service;

        private readonly ITicketBuilderService _ticketBuilderService;

        public BetListByProductController(IBetListByProductService betListByProductService, ITicketBuilderService ticketBuilderService)
        {
            _service = betListByProductService;
            _ticketBuilderService = ticketBuilderService;
        }

        public async Task<ActionResult> Index(BetListByProductMetaModel metaModel)
        {
            var ticketResult = await _service.QueryAsync(metaModel).ConfigureAwait(false);
            var result = new
            {
                Data = _ticketBuilderService.BuildTicketText(ticketResult, metaModel, metaModel.HasPlayerCom),
                ticketResult.TotalRows
            };
            BetListHelper.ClearData(ticketResult);
            return Json(result);
        }
    }
}
