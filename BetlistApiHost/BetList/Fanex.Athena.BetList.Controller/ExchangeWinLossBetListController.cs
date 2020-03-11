using System.Threading.Tasks;
using System.Web.Mvc;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Interfaces;

namespace Fanex.Athena.BetList.Controller
{
    public class ExchangeWinLossBetListController : BaseController
    {
        private readonly IExchangeBetListService _service;

        public ExchangeWinLossBetListController(IExchangeBetListService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<JsonResult> GetData(ExchangeBetListMetaModel metaModel)
        {
            return Json(await _service.GetBetListDataAsync(metaModel).ConfigureAwait(false));
        }

        [HttpPost]
        public async Task<JsonResult> Detail(ExchangeBetListDetailMetaModel metaModel)
        {
            return Json(await _service.GetTicketDetailListAsync(metaModel).ConfigureAwait(false));
        }
    }
}