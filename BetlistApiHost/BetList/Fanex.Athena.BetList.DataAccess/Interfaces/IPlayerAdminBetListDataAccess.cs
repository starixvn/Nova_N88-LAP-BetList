namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;

    public interface IPlayerAdminBetListDataAccess
    {
        Task<TicketResult> GetRunningBetListAsync(PlayerAdminBetListMetaModel metaModel);
    }
}
