namespace Fanex.Athena.BetList.DataAccess
{
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using BetList.Model.DTO;
    using BetList.Model.MetaModel;
    using Common.Constant;
    using Common.DataAccess.Interfaces;
    using Data;
    using Fanex.Athena.Common.Utility.Helpers;
    using Fanex.Common;
    using Interfaces;

    public class BettingTicketCenterDataAccess : BaseBetListDataAccess, IBettingTicketCenterDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public BettingTicketCenterDataAccess(IBalanceUpDataAccess balanceUpDataAccess,
            ICurrencyDataAccess currencyService) : base(currencyService)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public async Task<TicketResult> GetBetListForSingleCustomerAsync(
            SingleCustomersBetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_Mem";

            var ticketResult = new TicketResult();
            var currencyList = await this.GetCurrencyListAsync();
            using (var db = GetObjectDb(UspKey))
            {
                AddSingleCustomerParameters(db, metaModel);

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    ticketResult.MainTicketList.AddRange(ReadBetListTicket(reader, currencyList));
                }

                var outSqlParams = db.GetOutputParams();
                ticketResult.SumStake = Converter.ToDecimal(outSqlParams["sumstake"]);
                ticketResult.SumWinLoss = Converter.ToDecimal(outSqlParams["sumwinlost"]);
                ticketResult.SumLicenseeWL = Converter.ToDecimal(outSqlParams["sumlicenseewl"]);
                ticketResult.SumComm = Converter.ToDecimal(outSqlParams["sumcomm"]);
                ticketResult.SumPlayerComm = Converter.ToDecimal(outSqlParams["sumplayercomm"]);
                ticketResult.TotalRows = Converter.ToInt(outSqlParams["totalrows"]);
            }

            return ticketResult;
        }

        public async Task<TicketResult> GetBetListByRefnoAsync(RefNoBetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_RefNo_Mem";

            var ticketResult = new TicketResult();

            if (long.TryParse(metaModel.RefNo, out long refno))
            {
                var currencyList = await this.GetCurrencyListAsync();

                using (var db = GetObjectDb(UspKey))
                {
                    var balanceUp = _balanceUpDataAccess.GetBalanceUp();
                    db.SetParameterValue("AgentList", metaModel.AgentListXml);
                    db.SetParameterValue("UserName", metaModel.UserName);
                    db.SetParameterValue("RefNo", refno);
                    db.SetParameterValue("MoveBetsDay", balanceUp.MoveBetsDay);
                    db.SetParameterValue("BalanceUpDay", balanceUp.BalanceUpDay);
                    db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
                    db.SetParameterValue("ViewSabaCoins", metaModel.ViewSabaCoins);

                    using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        ticketResult.MainTicketList.AddRange(ReadBetListTicket(reader, currencyList, 0, metaModel.LastXDays));
                        ticketResult.TotalRows = ticketResult.MainTicketList.Count;
                    }
                }
            }

            return ticketResult;
        }

        public async Task<TicketResult> GetRunningBetListAsync(BaseBettingTicketMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_Running";

            var ticketResult = new TicketResult();
            var currencyList = await this.GetCurrencyListAsync();

            using (var db = GetObjectDb(UspKey))
            {
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("UserName", metaModel.UserName);
                db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
                db.SetParameterValue("ProductList", metaModel.ProductCodeList);
                db.SetParameterValue("PageSize", metaModel.IsExport ? int.MaxValue - 1 : metaModel.PageSize);
                db.SetParameterValue("PageIndex", metaModel.IsExport ? 1 : metaModel.PageIndex);
                db.SetParameterValue("StatusFilter", metaModel.Status);
                db.SetParameterValue("ConvertToBaseCur", metaModel.IsConvertAll);
                db.SetParameterValue("TotalRows", null);
                db.SetParameterValue("SumStake", null);
                db.SetParameterValue("SortType", metaModel.SortType);
                db.SetParameterValue("SortOrder", metaModel.SortOrder);

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    ticketResult.MainTicketList.AddRange(ReadBetListTicket(reader, currencyList));
                }

                var outSqlParams = db.GetOutputParams();
                ticketResult.SumStake = Converter.ToDecimal(outSqlParams["sumstake"]);
                ticketResult.TotalRows = Converter.ToInt(outSqlParams["totalrows"]);
            }

            return ticketResult;
        }

        private void AddSingleCustomerParameters(IObjectDb db, SingleCustomersBetListMetaModel metaModel)
        {
            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            db.SetParameterValue("ProductList", metaModel.ProductCodeList);
            db.SetParameterValue("FromDate", metaModel.FromDate);
            db.SetParameterValue("ToDate", metaModel.ToDate);
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("UserName", metaModel.UserName);
            db.SetParameterValue("StatusIdList", metaModel.Status);
            db.SetParameterValue("LiveIndicator", metaModel.LiveStatus);
            db.SetParameterValue("Platform", metaModel.Platform);
            db.SetParameterValue("MoveBetsDay", balanceUp.MoveBetsDay);
            db.SetParameterValue("BalanceUpDay", balanceUp.BalanceUpDay);
            db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
            db.SetParameterValue("PageSize", metaModel.IsExport ? int.MaxValue - 1 : metaModel.PageSize);
            db.SetParameterValue("PageIndex", metaModel.IsExport ? 1 : metaModel.PageIndex);

            db.SetParameterValue("TotalRows", null);
            db.SetParameterValue("SumStake", null);
            db.SetParameterValue("SumWinLost", null);
            db.SetParameterValue("SumLicenseeWL", null);
            db.SetParameterValue("SumComm", null);
            db.SetParameterValue("SumPlayerComm", null);
            db.SetParameterValue("SortType", metaModel.SortType);
            db.SetParameterValue("SortOrder", metaModel.SortOrder);
        }

        public async Task<TicketResult> GetBetListForBookedBetAsync(BookedBetBetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_BookedBets";
            const int UserNameOption = 3;

            var ticketResult = new TicketResult();
            var currencyList = await this.GetCurrencyListAsync();

            if (metaModel.SearchOption == UserNameOption || long.TryParse(metaModel.SearchText, out long _))
            {
                using (var db = GetObjectDb(UspKey))
                {
                    db.SetParameterValue("FromDate", metaModel.FromDate);
                    db.SetParameterValue("ToDate", metaModel.ToDate);
                    db.SetParameterValue("AgentList", metaModel.AgentListXml);
                    db.SetParameterValue("SearchText", SqlHelper.ToSqlTextSearch(metaModel.SearchText));
                    db.SetParameterValue("SearchOption", metaModel.SearchOption);
                    db.SetParameterValue("StatusIdList", metaModel.Status);
                    db.SetParameterValue("Platform", metaModel.Platform);
                    db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
                    db.SetParameterValue("ConvertToBaseCur", metaModel.IsConvertAll);
                    db.SetParameterValue("PageSize", metaModel.IsExport ? int.MaxValue - 1 : metaModel.PageSize);
                    db.SetParameterValue("PageIndex", metaModel.IsExport ? 1 : metaModel.PageIndex);

                    db.SetParameterValue("TotalRows", null);
                    db.SetParameterValue("SumStake", null);
                    db.SetParameterValue("SortType", metaModel.SortType);
                    db.SetParameterValue("SortOrder", metaModel.SortOrder);

                    using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        ticketResult.MainTicketList.AddRange(ReadBetListTicket(reader, currencyList));
                    }

                    var outSqlParams = db.GetOutputParams();
                    ticketResult.SumStake = Converter.ToDecimal(outSqlParams["SumStake"]);
                    ticketResult.TotalRows = Converter.ToInt(outSqlParams["TotalRows"]);
                }
            }

            return ticketResult;
        }
    }
}