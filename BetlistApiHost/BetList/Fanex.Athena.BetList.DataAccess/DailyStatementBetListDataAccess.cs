namespace Fanex.Athena.BetList.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.DataAccess.Interfaces;
    using Common.Utility.Extensions;
    using Fanex.Common.Extensions;
    using Interfaces;
    using Model.DTO;
    using Model.MetaModel;
    using Util;

    public class DailyStatementBetListDataAccess : BaseBetListDataAccess, IDailyStatementBetListDataAccess
    {
        private static readonly string[] TransTypeHasBetList = { "get", "com", "post", "bet", "gp" };

        private static readonly Dictionary<string, int> TransTypeIdMap = new Dictionary<string, int>
            {
                { "betting statement", 1 }, { "void/cancelled bet", 2 },
                { "outstanding bet", 3 }, { "grand prize", 4 }
            };

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public DailyStatementBetListDataAccess(IBalanceUpDataAccess balanceUpDataAccess
               ) : base(null)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public async Task<IEnumerable<DailyStatement>> GetDailyStatementData(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_Statement_Mem";

            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            metaModel.BalanceUpDay = balanceUp.BalanceUpDay;
            metaModel.MoveBetsDay = balanceUp.MoveBetsDay;

            using (var db = this.GetObjectDb(UspKey))
            {
                db.SetParameterValue("CustID", metaModel.CustId);
                db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
                BetListParameterBuilder.CreateBetListDateRangeParameters(db, metaModel);
                BetListParameterBuilder.CreateBetlistBUParameters(db, metaModel);
                BetListParameterBuilder.CreateBetListPagingParameters(db, metaModel);
                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    return ReadData(metaModel, reader);
                }
            }
        }

        private static IEnumerable<DailyStatement> ReadData(BetListMetaModel metaModel, IDataReader reader)
        {
            const string FormatRemark = "<span class='bl-underdog'>{0} <br/>- {1}</span>";
            var index = 1;
            decimal balance = 0;
            var currentCulture = CultureInfo.CurrentCulture;

            var result = new List<DailyStatement>();
            while (reader.Read())
            {
                var winlostDate = reader.ToDateTime("winlostdate");
                var transType = reader.ToString("transtype").ToLower(currentCulture);
                var transDesc = reader.ToString("transdesc");
                var isOutstandingBet = transDesc.ToLower(currentCulture) == "outstanding bet";
                var transTypeId = TransTypeIdMap.GetValueOrDefault(transDesc.ToLower(currentCulture), 0);
                var credit = isOutstandingBet || transType == "bal" ? 0 : reader.ToDecimal("totalamt");
                var comm = reader.ToDecimal("commission");
                var turnover = reader.ToDecimal("turnover");

                // Add commission.
                if (!transType.Equals("gp"))
                {
                    var outstanding = isOutstandingBet ? turnover : 0;
                    var playerComm = metaModel.HasPlayerCom ? comm : 0;
                    balance = transType == "bal" ? reader.ToDecimal("totalamt") : balance + credit + playerComm - outstanding;
                }

                result.Add(new DailyStatement
                {
                    TransType = transTypeId.ToString(),
                    CustId = metaModel.CustId,
                    ExternalId = metaModel.ExternalId ?? "0",
                    SystemId = metaModel.SystemId ?? "0",
                    FullStatementDate = $"{winlostDate:dddd} <br/> {winlostDate:MM/dd/yyyy hh:mm:ss tt}",
                    TransTypeHasBetList = TransTypeHasBetList.Contains(transType),
                    Index = index++,
                    Date = winlostDate.ToString("MM/dd/yyyy"),
                    Remark = string.Format(FormatRemark, transType.ToUpper(currentCulture), transDesc),
                    Turnover = NumberReport(turnover),
                    CreditDebit = NumberReport(credit),
                    Commission = NumberReport(comm),
                    Balance = NumberReport(balance)
                });
            }

            return result;
        }

        protected static string NumberReport(object value)
        {
            var number = Convert.ToDecimal(value);
            return number >= 0 ? "<span class='bl-underdog'>" + $"{number:#,##0.00}" + "</span>"
                       : "<span class='bl-favorite'>" + $"{number:#,##0.00}" + "</span>";
        }
    }
}