using System.Threading.Tasks;
using System.Web.Mvc;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.BetList.Core.Entities;

namespace Fanex.Athena.BetList.Controller
{
    public class SystemParlayBetListController : BaseController
    {
        private readonly ITicketHelper _ticketHelper;

        private readonly ISystemParlayDetailService _systemParlayDetailService;

        public SystemParlayBetListController(
            ITicketHelper ticketHelper,
            ISystemParlayDetailService systemParlayDetailService)
        {
            _ticketHelper = ticketHelper;
            _systemParlayDetailService = systemParlayDetailService;
        }

        [HttpPost]
        public async Task<string> Detail(SystemParlayDetailMetaModel metaModel)
        {
            var viewPath = Server.MapPath("~/Views/SystemParlayDetail/Index.cshtml");

            return await _systemParlayDetailService.BuildHtmlContent(metaModel, _ticketHelper, viewPath);
        }
    }
}