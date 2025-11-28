using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Helpers;
using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.BetList.Core.Builder.HybridParlayBuilder;
using Fanex.BetList.Core.Entities;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fanex.Athena.BetList.Controller
{
    public class BetBuilderPlusBetListController : BaseController
    {
        private readonly IBetListService _betListService;

        private readonly ITicketHelper _ticketHelper;

        private readonly IHybridParlayBuilder _hybridParlayBuilder;

        private readonly IIPService _ipService;

        public BetBuilderPlusBetListController(
            IBetListService betListService,
            ITicketHelper ticketHelper,
            IHybridParlayBuilder hybridParlayBuilder,
             IIPService ipService)
        {
            _betListService = betListService;
            _ticketHelper = ticketHelper;
            _hybridParlayBuilder = hybridParlayBuilder;
            _ipService = ipService;
        }

        [HttpPost]
        public async Task<object> Detail(BetBuilderPlusDetailMetaModel metaModel)
        {
            var ticketResult = await _betListService.GetMixParlayDetailTicket(metaModel);

            BuildTicketHelper(true);

            var result = new
            {
                Data = BuildParlayDetailTickets(ticketResult, metaModel)
            };

            BetListHelper.ClearData(ticketResult);

            return Json(result);
        }

        private void BuildTicketHelper(bool showOdds = false)
        {
            _ticketHelper.IsShowOddsWithChoice = showOdds;
            _ticketHelper.IsVnIp = _ipService.IsVNIP;
        }

        protected internal virtual string BuildParlayDetailTickets(TicketResult ticketResult, BetBuilderPlusDetailMetaModel metaModel)
        {
            var ticketList = ticketResult.MainTicketList.Cast<ITicket>().ToList();
            var result = _hybridParlayBuilder.Build(
                _ticketHelper,
                ticketList,
                metaModel.TransDesc,
                (int)metaModel.Handicap1,
                (int)metaModel.Handicap2,
                metaModel.BetCheck,
                metaModel.BetTeam,
                metaModel.Currency,
                metaModel.Stake,
                false);

            return result;
        }
    }
}