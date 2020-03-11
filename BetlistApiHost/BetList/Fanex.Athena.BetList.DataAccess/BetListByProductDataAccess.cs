namespace Fanex.Athena.BetList.DataAccess
{
    using System.Threading.Tasks;
    using Fanex.Athena.BetList.DataAccess.Interfaces;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Athena.Common.DataAccess.Interfaces;
    using Fanex.Common;

    public class BetListByProductDataAccess : BaseBetListDataAccess, IBetListByProductDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public BetListByProductDataAccess(IBalanceUpDataAccess balanceUpDataAccess,
            ICurrencyDataAccess currenyDataAccess) : base(currenyDataAccess)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public async Task<TicketResult> QueryAsync(BetListByProductMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_Statement_Mem";

            var ticketResult = new TicketResult();
            var currencyList = await this.GetCurrencyListAsync();
            using (var db = GetObjectDb(UspKey))
            {
                SetParam(metaModel, db);

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    ticketResult.MainTicketList.AddRange(ReadBetListTicket(reader,
                        currencyList));
                }

                var outSqlParams = db.GetOutputParams();
                GetOutPutResult(ticketResult, outSqlParams);
            }

            return ticketResult;
        }

        private static void GetOutPutResult(TicketResult ticketResult, System.Collections.Hashtable outSqlParams)
        {
            ticketResult.TotalRows = Converter.ToInt(outSqlParams["totalrows"]);
            ticketResult.SumStake = Converter.ToDecimal(outSqlParams["SumStake"]);
            ticketResult.SumWinLoss = Converter.ToDecimal(outSqlParams["SumWinLost"]);
            ticketResult.SumLicenseeWL = Converter.ToDecimal(outSqlParams["SumLicenseeWL"]);
            ticketResult.SumComm = Converter.ToDecimal(outSqlParams["SumComm"]);
            ticketResult.SumPlayerComm = Converter.ToDecimal(outSqlParams["SumPlayerComm"]);
        }

        private void SetParam(BetListByProductMetaModel metaModel, Data.IObjectDb db)
        {
            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            db.SetParameterValue("@AgentList", metaModel.AgentListXml);
            db.SetParameterValue("CustId", metaModel.CustId);
            db.SetParameterValue("@Date", metaModel.WinlostDate.Date);
            db.SetParameterValue("@MoveBetsDay", balanceUp.MoveBetsDay);
            db.SetParameterValue("@BalanceUpDay", balanceUp.BalanceUpDay);
            db.SetParameterValue("@ProductCode", metaModel.ProductCode);
            db.SetParameterValue("@TransType", metaModel.TransType);
            db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
            db.SetParameterValue("PageSize", metaModel.PageSize);
            db.SetParameterValue("PageIndex", metaModel.PageIndex);
            db.SetParameterValue("@Sort", metaModel.Sort);
            db.SetParameterValue("@TotalRows", null);
            db.SetParameterValue("@SumStake", null);
            db.SetParameterValue("@SumWinLost", null);
            db.SetParameterValue("@SumLicenseeWL", null);
            db.SetParameterValue("@SumComm", null);
            db.SetParameterValue("@SumPlayerComm", null);
        }
    }
}