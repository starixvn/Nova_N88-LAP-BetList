namespace Fanex.Athena.BetList.Service.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;

    public interface IBetListByProductService
    {
        Task<TicketResult> QueryAsync(BetListByProductMetaModel metaModel);
    }
}
