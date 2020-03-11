namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;

    public interface IDailyStatementBetListDataAccess
    {
        Task<IEnumerable<DailyStatement>> GetDailyStatementData(BetListMetaModel metaModel);
    }
}