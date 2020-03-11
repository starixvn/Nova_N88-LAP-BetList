namespace Fanex.Athena.BetList.DataAccess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.BetList.DataAccess.Interfaces;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Athena.Common.DataAccess;
    using Fanex.Common.Extensions;

    public class SingleMatchParlayDetailDataAccess : BaseDataAccess, ISingleMatchParlayDetailDataAccess
    {
        public async Task<IList<SingleMatchParlayDetail>> GetSingleMatchParlayDetail(SingleMatchParlayDetailMetaModel metaModel)
        {
            const string UspName = "Lrf_BetList_BetBuilder_Detail";
            var result = new List<SingleMatchParlayDetail>();

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("TransId", metaModel.TransId);
                db.SetParameterValue("CustId", metaModel.CustId);
                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (reader.Read())
                    {
                        result.Add(new SingleMatchParlayDetail
                        {
                            SelectionType = reader.ToInt("SelectionType"),
                            Result = reader.ToString("Result")
                        });
                    }
                }
            }

            return result;
        }
    }
}
