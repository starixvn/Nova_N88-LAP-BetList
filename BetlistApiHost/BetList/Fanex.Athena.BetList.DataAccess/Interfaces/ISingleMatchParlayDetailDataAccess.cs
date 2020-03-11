namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;

    public interface ISingleMatchParlayDetailDataAccess
    {
        Task<IList<SingleMatchParlayDetail>> GetSingleMatchParlayDetail(SingleMatchParlayDetailMetaModel metaModel);
    }
}
