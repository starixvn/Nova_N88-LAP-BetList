namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BetList.Model.ViewModel;
    using Fanex.Athena.BetList.Model.MetaModel;

    public interface ICashOutBetListDataAccess
    {
        Task<CashOutBetListDetailViewModel> GetDetail(CashOutBetListDetailMetaModel metaModel);
    }
}