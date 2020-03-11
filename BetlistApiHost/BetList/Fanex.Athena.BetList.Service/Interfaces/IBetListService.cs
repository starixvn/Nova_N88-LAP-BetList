// -----------------------------------------------------------------------
// <copyright file="IBetListService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.Service.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;

    public interface IBetListService
    {
        string GetLeagueName(int leagueId);

        Task<TicketResult> GetMixParlayDetailTicket(MixParlayDetailMetaModel metaModel);

        Task<TicketResult> GetBetListData(BetListMetaModel metaModel);
    }
}