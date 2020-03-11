namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BetList.Model.DTO;
    using BetList.Model.MetaModel;

    public interface IBettingTicketCenterAdvancedSearchDataAccess
    {
        Task<TicketResult> GetBetListForMultipleCustomersAsync(AdvancedSearchBetListMetaModel metaModel);

        Task<GroupTicketResult> GetBetListForMultipleCustomersAsync(AdvancedSearchBetListMetaModel metaModel, int rowPerGroup);

        Task<int> GetTotalRowForMultipleCustomersAsync(AdvancedSearchBetListMetaModel metaModel);
    }
}