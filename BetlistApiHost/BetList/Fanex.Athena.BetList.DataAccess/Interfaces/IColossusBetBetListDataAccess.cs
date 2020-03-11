namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Model.ViewModel;

    public interface IColossusBetBetListDataAccess
    {
        Task<ColossusBetDetailViewModel> GetData(ColossusBetDetailMetaModel metaModel);

        int GetSiteId(int custId);
    }
}