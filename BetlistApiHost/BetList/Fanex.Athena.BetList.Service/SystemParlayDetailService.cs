namespace Fanex.Athena.BetList.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.Athena.BetList.DataAccess.Interfaces;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Athena.BetList.Model.ViewModel;
    using Fanex.Athena.BetList.Service.Interfaces;
    using Fanex.Athena.Common.Utility.Services.Interfaces;
    using Fanex.BetList.Core.App_GlobalResources;
    using Fanex.BetList.Core.Entities;

    [CLSCompliant(false)]
    public class SystemParlayDetailService : ISystemParlayDetailService
    {
        private readonly ISystemParlayDetailDataAccess _dataAccess;

        private readonly ITemplateBuilder _templateBuilder;

        private const int UnknowStatusOrder = 100;

        private static readonly Dictionary<string, int> _statusOrder = new Dictionary<string, int>
        {
            { BetStatusId.Won, 1 },
            { BetStatusId.Lost, 2 },
            { BetStatusId.Draw, 3 },
            { BetStatusId.Void, 4 },
            { BetStatusId.Reject, 5 },
            { BetStatusId.Refund, 6 }
        };

        public SystemParlayDetailService(ISystemParlayDetailDataAccess dataAccess, ITemplateBuilder templateBuilder)
        {
            _dataAccess = dataAccess;
            _templateBuilder = templateBuilder;
        }

        public async Task<string> BuildHtmlContent(SystemParlayDetailMetaModel metaModel, ITicketHelper ticketHelper, string viewPath)
        {
            var data = await _dataAccess.GetData(metaModel);
            var ticketList = BuildTicketList(data, ticketHelper);

            var viewModel = new SystemParlayDetailViewModel
            {
                DataList = ticketList,
                Outstanding = data.Outstanding,
                Total = data.TicketList.Sum(x => x.WinLost)
            };

            var result = _templateBuilder.BuildContent(viewPath, viewModel);

            return result;
        }

        private static IEnumerable<IGrouping<DateTime, SystemParlayDetailItem>> BuildTicketList(SystemParlayDetailData data, ITicketHelper ticketHelper)
        {
            return data.TicketList
                .OrderBy(t =>
            {
                var statusId = t.StatusId.ToString();
                return _statusOrder.ContainsKey(statusId) ? _statusOrder[statusId] : UnknowStatusOrder;
            })
            .GroupJoin(data.SerialList,
                t => t.Serial,
                s => s.Serial,
                (ticket, serialList) => new SystemParlayDetailItem
                {
                    Date = ticket.WinLostDate,
                    Odds = ticket.MOdds,
                    Stake = ticket.Stake,
                    WinLoss = ticket.WinLost,
                    PlayerComm = ticket.PlayerComm,
                    Status = GetBetListStatusName(ticket.StatusId),
                    MatchList = serialList.Select(x => new SystemParlayDetailMatch
                    {
                        HomeTeam = ticketHelper.GetTeamNameById(x.HomeId),
                        AwayTeam = ticketHelper.GetTeamNameById(x.AwayId),
                        IsNeutral = x.IsNeutral
                    })
                })
                .GroupBy(p => p.Date)
                .OrderByDescending(t => t.Key);
        }

        private static string GetBetListStatusName(int status)
        {
            switch (status.ToString())
            {
                case BetStatusId.Won:
                    return CoreBetList.lblWon;

                case BetStatusId.Lost:
                    return CoreBetList.lblLose;

                case BetStatusId.Draw:
                    return CoreBetList.draw;

                case BetStatusId.Running:
                    return CoreBetList.lblRunning;

                case BetStatusId.Waiting:
                    return CoreBetList.lblWaiting;

                case BetStatusId.Reject:
                    return CoreBetList.reject;

                case BetStatusId.Void:
                    return CoreBetList.voided;

                case BetStatusId.Refund:
                    return CoreBetList.refund;

                default:
                    return string.Empty;
            }
        }
    }
}