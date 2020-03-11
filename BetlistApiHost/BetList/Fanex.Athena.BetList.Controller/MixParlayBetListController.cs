using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Helpers;
using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.Athena.Common.Utility.Extensions;
using Fanex.Athena.Common.Utility.Services.Interfaces;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace Fanex.Athena.BetList.Controller
{
    public class MixParlayBetListController : BaseController
    {
        private readonly ICacheRefAdapter _cacheRefAdapter;

        private readonly IBetListService _betListService;

        private readonly ITicketHelper _ticketHelper;

        private readonly IBetListHTMLBuilder _betlistHtmlBuilder;
        private readonly ITicketBuilderService _ticketBuilderService;

        public MixParlayBetListController(
            ICacheRefAdapter cacheRefAdapter,
            IBetListService betListService,
            ITicketHelper ticketHelper,
            IBetListHTMLBuilder betlistHtmlBuilder,
             ITicketBuilderService ticketBuilderService)
        {
            _cacheRefAdapter = cacheRefAdapter;
            _betListService = betListService;
            _ticketHelper = ticketHelper;
            _betlistHtmlBuilder = betlistHtmlBuilder;
            _ticketBuilderService = ticketBuilderService;

        }

        [HttpPost]
        public async Task<JsonResult> Detail(MixParlayDetailMetaModel metaModel)
        {
            var ticketResult = await _betListService.GetMixParlayDetailTicket(metaModel).ConfigureAwait(false);
            var result = _ticketBuilderService.BuildTicketText(ticketResult: ticketResult, showOdds: true);
            BetListHelper.ClearData(ticketResult);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> NewParlayDetail(NewParlayDetailMetaModel metaModel)
        {
            var texts = _cacheRefAdapter.GetCombinationTypeCombo(metaModel.BetTeam, metaModel.BetCheck, metaModel.Handicap1, metaModel.Handicap2);
            if (texts == null)
            {
                return null;
            }

            texts = (string[])texts.Clone(); // must do this weird step
            var ticketResult = await _betListService.GetMixParlayDetailTicket(metaModel);
            var tickets = ticketResult.MainTicketList.Where(x => x.Stake != 0).ToList();

            const string TextRenderPattern = "<p class='system-parlay-ticket-infor'>"
                                    + "<span class='system-parlay-combo-type'>{0}</span> x "
                                    + "<span class='system-parlay-unit-stake'>{1} {2}</span>"
                                    + "</p>";
            if (tickets.Any())
            {
                if (metaModel.Handicap1 == 0)
                {
                    // System Parlay.
                    var ticket = tickets.FirstOrDefault();
                    texts[0] = string.Format(TextRenderPattern, texts[0], ticket.Currency, ticket.Stake.ToString("N0"));
                }
                else
                {
                    // Combo parlay.
                    tickets = tickets.OrderBy(x => x.MatchId).ToList();
                    var length = Math.Min(texts.Count(), tickets.Count);
                    for (int i = 0; i < length; i++)
                    {
                        var text = texts[i];
                        var ticket = tickets[i];
                        texts[i] = string.Format(TextRenderPattern, text, ticket.Currency, ticket.Stake.ToString("N0"));
                    }
                }
            }

            var result = new
            {
                Data = BuildParlayDetailTickets(ticketResult: ticketResult, showOdds: true),
                Texts = texts
            };
            BetListHelper.ClearData(ticketResult);
            return Json(result);
        }

        protected internal virtual IList<string> BuildParlayDetailTickets(TicketResult ticketResult, bool showOdds = false)
        {
            var ticketTextList = new List<string>();

            _ticketHelper.IsShowOddsWithChoice = showOdds;

            foreach (var ticket in ticketResult.MainTicketList)
            {
                var statusTemplate = _betlistHtmlBuilder.CreateStatusBuilder(ticket.BetTypeId).Render(ticket, _ticketHelper, ticketResult.TicketDataList, isShowVNIP: false);
                statusTemplate.ShowIP = null;
                statusTemplate.StatusResult = null;

                var choice = _betlistHtmlBuilder.BuildChoice(ticket, _ticketHelper, ticketResult.TicketDataList, false);
                var transDateAndStatus = _betlistHtmlBuilder.BuildTransEventDateAndStatusResult(ticket, ticketResult.TicketDataList);

                ticketTextList.Add(choice + transDateAndStatus);
            }

            return ticketTextList;
        }
    }
}