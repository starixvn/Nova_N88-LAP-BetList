namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BetList.Model.DTO;
    using BetList.Model.MetaModel;

    public interface IBettingTicketCenterDataAccess
    {
        Task<TicketResult> GetBetListForSingleCustomerAsync(
            SingleCustomersBetListMetaModel metaModel);

        Task<TicketResult> GetBetListByRefnoAsync(RefNoBetListMetaModel metaModel
           );

        Task<TicketResult> GetRunningBetListAsync(BaseBettingTicketMetaModel metaModel);
    }
}