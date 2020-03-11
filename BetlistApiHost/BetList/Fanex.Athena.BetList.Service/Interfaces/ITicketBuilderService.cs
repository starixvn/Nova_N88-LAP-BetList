namespace Fanex.Athena.BetList.Service.Interfaces
{
    using System.Collections.Generic;

    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;

    public interface ITicketBuilderService
    {
        IList<TicketText> BuildTicketText(TicketResult ticketResult, ICommonValueMetaModel metaModel,
            bool isAllowedCustomerComm);
        List<TicketText> BuildTicketText(
           TicketResult ticketResult,
           BetListMetaModel metaModel = null,
           int startIndex = 1,
           bool showOdds = false,
           bool isAllowedCustomerComm = false);

        string BuildSumWinLoss(TicketResult ticketResult, bool isAllowedCustomerComm);
    }
}
