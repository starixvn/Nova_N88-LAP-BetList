namespace Fanex.Athena.BetList.Service.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BetList.Model.DTO;
    using BetList.Model.MetaModel;
    using BetList.Model.ViewModel;

    public interface IExchangeBetListService
    {
        Task<ExchangeBetListViewModel> GetBetListDataAsync(ExchangeBetListMetaModel metaModel);

        Task<IEnumerable<ExchangeTicketDetail>> GetTicketDetailListAsync(ExchangeBetListDetailMetaModel metaModel);
    }
}