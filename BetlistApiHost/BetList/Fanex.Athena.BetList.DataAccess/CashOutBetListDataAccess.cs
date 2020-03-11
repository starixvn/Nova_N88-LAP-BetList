// -----------------------------------------------------------------------
// <copyright file="ColossusBetBetListDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.DataAccess
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using BetList.Model.DTO;
    using BetList.Model.ViewModel;
    using Common.Utility.Collections;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Athena.Common.DataAccess.Interfaces;
    using Fanex.Common.Extensions;
    using Interfaces;

    public class CashOutBetListDataAccess : BaseBetListDataAccess, ICashOutBetListDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public CashOutBetListDataAccess(ICurrencyDataAccess currencyService, IBalanceUpDataAccess balanceUpDataAccess) : base(currencyService)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public async Task<CashOutBetListDetailViewModel> GetDetail(
            CashOutBetListDetailMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_CashOut_Detail";

            var balanceUpData = _balanceUpDataAccess.GetBalanceUp();
            var currencyList = await this.GetCurrencyListAsync().ConfigureAwait(false);

            using (var db = GetObjectDb(UspKey))
            {
                var param = new
                {
                    metaModel.TransId,
                    metaModel.RefNo,
                    metaModel.BetType,
                    OriginalTransId = metaModel.BetdaqId,
                    AgentList = metaModel.AgentListXml,
                    metaModel.BaseCurrency,
                    balanceUpData.MoveBetsDay,
                    balanceUpData.BalanceUpDay
                };
                using (var reader = await db.ExecuteReaderAsync(param).ConfigureAwait(false))
                {
                    var viewModel = new CashOutBetListDetailViewModel
                    {
                        OriginalTicket = ReadBetListTicket(reader, currencyList).GetFirstOrDefault()
                    };

                    if (reader.NextResult())
                    {
                        viewModel.CashOutTicketList = ReadCashOutTickets(reader);
                    }

                    return viewModel;
                }
            }
        }

        private static IEnumerable<CashOutTicket> ReadCashOutTickets(IDataReader reader)
        {
            var result = new SortableList<CashOutTicket>();

            while (reader.Read())
            {
                var item = new CashOutTicket
                {
                    BetType = reader.ToInt("bettype"),
                    TransId = reader.ToLong("transid"),
                    RefNo = reader.ToString("refno"),
                    Status = reader.ToString("status"),
                    Stake = reader.ToDecimal("stake"),
                    WinLoss = reader.ToDecimal("winlost"),
                    Price = reader.ToDecimal("price"),
                    WinLostDate = reader.ToDateTime("winlostdate"),
                    TransDate = reader.ToDateTime("transdate"),
                    Currency = reader.ToInt("currency"),
                    CustId = reader.ToInt("custid")
                };

                result.Add(item);
            }

            return result;
        }
    }
}