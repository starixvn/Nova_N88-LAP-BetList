using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Extensions;
using Fanex.Athena.BetList.Service.Helpers;
using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.Athena.Common.Constant;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace Fanex.Athena.BetList.Controller
{
    public class LastBetMonitoringBetListController : BaseController
    {
        private static readonly IEnumerable<int> ConvertingDecimalOddsBetType = new[] { 1, 2, 3, 7, 8, 12, 20 };

        private readonly ILastBetsMonitoringBetListService _lastBetsMonitoringBetListService;

        private readonly ITicketHelper _ticketHelper;

        public LastBetMonitoringBetListController(
            ILastBetsMonitoringBetListService lastBetsMonitoringBetListService,
            ITicketHelper ticketHelper)
        {
            _lastBetsMonitoringBetListService = lastBetsMonitoringBetListService;
            _ticketHelper = ticketHelper;
        }

        [HttpPost]
        public async Task<JsonResult> GetData(LastBetsMonitoringMetaModel metaModel)
        {
            var ticketResult = await _lastBetsMonitoringBetListService.GetLastBetsMonitoringData(
                metaModel).ConfigureAwait(false);

            if (ticketResult.MainTicketList.Any())
            {
                var rows = GetLastMonitorBetListContent(metaModel, ticketResult);
                var result = new
                {
                    NewRow = true,
                    Rows = rows,
                    LastTransId = ticketResult.MainTicketList.Max(t => t.TransId),
                };
                BetListHelper.ClearData(ticketResult);
                return Json(result);
            }
            else
            {
                return Json(new
                {
                    NewRow = false
                });
            }
        }

        private string GetLastMonitorBetListContent(LastBetsMonitoringMetaModel metaModel, TicketResult ticketResult)
        {
            const string EndTag = "</td>";

            var builder = new BetListHTMLBuilder();
            foreach (var ticket in ticketResult.MainTicketList)
            {
                _ticketHelper.ClientDetailSystemParlayFunction = GetClientDetailSystemParlayFunction(metaModel, ticket);

                var row = $"<tr class='content-row monitor-hightlight' id='trans-{ticket.TransId}' data-transid='{ticket.TransId}'>";
                builder.Append(row);

                builder.Append($"<td data-name='Platform'>{PlatformType.GetPlatformName(ticket.BetFrom.Trim())}</td>");
                builder.Append($"<td data-name='ExternalId' class='special-color-{ticket.SpecialLevel}'>{ticket.ExternalId}</td>");
                builder.Append($"<td data-name='SystemId' class='special-color-{ticket.SpecialLevel}'>{ticket.UserName}</td>");
                builder.AddTrans(ticket, "<td data-name='TransTime' class='special-color-0'>", EndTag);
                builder.AddChoice(
                    ticket,
                    _ticketHelper,
                    ticketResult.TicketDataList,
                    "<td data-name='Choice' class='bl-event'>",
                    EndTag,
                    false);
                BuildOdds(metaModel, ticketResult, ticket, builder);
                builder.AddStake(ticket, "<td data-name='Stake' class='bl-stake'>", EndTag);
                builder.Append($"<td data-name='Status' class='bl-status'>{builder.BuildLiveStatus(ticket)}</td></tr>");
            }

            return builder.GetBetList();
        }

        private string GetClientDetailSystemParlayFunction(LastBetsMonitoringMetaModel metaModel, LrfTicket ticket)
        {
            if (ticket.BetTypeId == (int)BetType.CombinationMixParlay)
            {
                var baseCurrency = metaModel == null ? string.Empty : metaModel.BasedCurrency;
                if (!SystemConstants.BaseCurrencyList.Contains(baseCurrency.ToUpperInvariant()))
                {
                    baseCurrency = string.Empty;
                }

                return BetListHelper.GenerateClientDetailSystemParlayFunction(_ticketHelper, baseCurrency, ticket, 0);
            }

            return string.Empty;
        }

        private void BuildOdds(LastBetsMonitoringMetaModel metaModel, TicketResult ticketResult, LrfTicket ticket, BetListHTMLBuilder builder)
        {
            const string OddStr = "<td data-name=\"Odds\" class=\"bl-odds\"><span class=\"underdog {1}\">{0}</span><br><span class=\"oddstype\">{2}</span></td>";
            if (metaModel == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(metaModel.OddsType)
                || !ConvertingDecimalOddsBetType.Contains(ticket.BetTypeId))
            {
                builder.AddOdds(
                    ticket,
                    ticketResult.TicketDataList,
                    "<td data-name=\"Odds\" class='bl-odds'>",
                    "</td>",
                    _ticketHelper.GetValidOddsTypeNameById);
            }
            else
            {
                builder.Append(
                    string.Format(
                        OddStr,
                        Fanex.BetList.Core.Utils.ConvertByBetType.Odds(ticket.Odds),
                        ticket.Odds < 0 ? "negative" : string.Empty,
                        metaModel.OddsType));
            }
        }
    }
}