namespace Fanex.Athena.BetList.Service
{
    using System;
    using System.Threading.Tasks;

    using Common.DataAccess.Interfaces;
    using Common.Utility.Services.Interfaces;

    using Fanex.Athena.BetList.DataAccess.Interfaces;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Athena.BetList.Service.Interfaces;

    [CLSCompliant(false)]
    public class BetListService : IBetListService
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly IBetListDataAccess _betlistDataAccess;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        public BetListService(
            IBalanceUpDataAccess balanceUpDataAccess,
            IBetListDataAccess betlistDataAccess,
            ICacheRefAdapter cacheRefAdapter)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
            _betlistDataAccess = betlistDataAccess;
            _cacheRefAdapter = cacheRefAdapter;
        }

        public string GetLeagueName(int leagueId)
        {
            return _cacheRefAdapter.GetLeagueName(leagueId);
        }

        public async Task<TicketResult> GetMixParlayDetailTicket(MixParlayDetailMetaModel metaModel)
        {
            return await _betlistDataAccess.GetMixParlayDetailTicket(metaModel);
        }

        public async Task<TicketResult> GetBetListData(BetListMetaModel metaModel)
        {
            AssignBalanceUpDates(metaModel);

            return await _betlistDataAccess.GetBetListData(metaModel).ConfigureAwait(false);
        }

        private void AssignBalanceUpDates(BetListMetaModel metaModel)
        {
            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            metaModel.MoveBetsDay = balanceUp.MoveBetsDay;
            metaModel.BalanceUpDay = balanceUp.BalanceUpDay;
        }
    }
}