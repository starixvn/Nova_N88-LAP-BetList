namespace Fanex.Athena.BetList.DataAccess
{
    using Fanex.Athena.BetList.DataAccess.Interfaces;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Athena.Common.DataAccess.Interfaces;
    using Fanex.Common;
    using Fanex.Data;
    using System.Threading.Tasks;

    public class PlayerAdminBetListDataAccess : BaseBetListDataAccess, IPlayerAdminBetListDataAccess
    {
        private const string UspKey = "Lrf_BetList_Running_Mem";
        public PlayerAdminBetListDataAccess(ICurrencyDataAccess currencyService) : base(currencyService)
        {

        }
        public async Task<TicketResult> GetRunningBetListAsync(PlayerAdminBetListMetaModel metaModel)
        {
            var ticketResult = new TicketResult();
            var currencyList = await this.GetCurrencyListAsync();

            using (var db = GetObjectDb(UspKey))
            {
                SetParameter(db, metaModel);

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    ticketResult.MainTicketList = ReadBetListTicket(reader, currencyList);

                    if (reader.NextResult())
                    {
                        ticketResult.TicketDataList.AddRange(ReadReferneceTicket(reader));
                    }
                }

                var outSqlParams = db.GetOutputParams();
                ticketResult.SumStake = Converter.ToDecimal(outSqlParams["SumStake"]);
                ticketResult.TotalRows = Converter.ToInt(outSqlParams["TotalRows"]);
            }

            return ticketResult;
        }

        private static void SetParameter(IObjectDb db, PlayerAdminBetListMetaModel metaModel)
        {
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("CustId", metaModel.CustId);
            db.SetParameterValue("PageIndex", metaModel.PageIndex);
            db.SetParameterValue("PageSize", metaModel.PageSize);
            db.SetParameterValue("SumStake", 0);
            db.SetParameterValue("TotalRows", 0);

            if (!string.IsNullOrEmpty(metaModel.SortBy) && !string.IsNullOrEmpty(metaModel.SortOrder))
            {
                db.SetParameterValue("Sort", $"{metaModel.SortBy} {metaModel.SortOrder}");
            }
        }
    }
}