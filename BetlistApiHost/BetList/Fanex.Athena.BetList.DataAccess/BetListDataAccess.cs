namespace Fanex.Athena.BetList.DataAccess
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Utility.Collections;
    using Fanex.Athena.BetList.DataAccess.Interfaces;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Athena.Common.DataAccess.Interfaces;
    using Fanex.Common;
    using Util;

    public class BetListDataAccess : BaseBetListDataAccess, IBetListDataAccess
    {
        public BetListDataAccess(ICurrencyDataAccess currencyDataAccess) : base(currencyDataAccess)
        {

        }

        public async Task<TicketResult> GetMixParlayDetailTicket(MixParlayDetailMetaModel metaModel)
        {
            const string UspName = "Lrf_BetList_SelParlayDetail";
            var currencyList = await this.GetCurrencyListAsync();
            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("TransId", metaModel.TransId);
                db.SetParameterValue("CustId", metaModel.CustId);
                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    var result = new TicketResult();
                    result.MainTicketList.AddRange(ReadBetListTicket(reader,
                        currencyList));
                    return result;
                }
            }
        }

        public async Task<TicketResult> GetBetListData(BetListMetaModel metaModel)
        {
            var ticketResult = new TicketResult();
            var currencyList = await this.GetCurrencyListAsync();

            using (var db = GetObjectDb(metaModel.UspKey))
            {
                BetListParameterBuilder.CreateBetListParameters(db, metaModel);

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    ticketResult.MainTicketList.AddRange(ReadBetListTicket(reader,
                       currencyList, metaModel.CustId));
                    if (reader.NextResult())
                    {
                        ticketResult.TicketDataList.AddRange(ReadReferneceTicket(reader).ToList());
                    }

                    SortData(metaModel, ticketResult.MainTicketList);
                }

                var outSqlParams = db.GetOutputParams() ?? new Hashtable();

                UpdateSummaryData(ticketResult, outSqlParams);

                return ticketResult;
            }
        }

        private static void UpdateSummaryData(TicketResult ticketResult, Hashtable outPutParams)
        {
            ticketResult.TotalRows = Converter.ToInt(outPutParams["totalrows"]);
            ticketResult.SumWinLoss = outPutParams.ContainsKey("sumwinlost")
                ? Converter.ToDecimal(outPutParams["sumwinlost"])
                : ticketResult.MainTicketList.Sum(x => x.Winlost);
            ticketResult.SumBetEffective = outPutParams.ContainsKey("sumbeteffective")
                ? Converter.ToDecimal(outPutParams["sumbeteffective"])
                : ticketResult.MainTicketList.Sum(x => x.BetEffective);
            ticketResult.SumLicenseeWL = outPutParams.ContainsKey("sumlicenseewl")
                ? Converter.ToDecimal(outPutParams["sumlicenseewl"])
                : ticketResult.MainTicketList.Sum(x => x.LicenseeWL);
            ticketResult.SumComm = outPutParams.ContainsKey("sumcomm")
                ? Converter.ToDecimal(outPutParams["sumcomm"])
                : ticketResult.MainTicketList.Sum(x => x.Comm);
            ticketResult.SumPlayerComm = outPutParams.ContainsKey("sumplayercomm")
                ? Converter.ToDecimal(outPutParams["sumplayercomm"])
                : ticketResult.MainTicketList.Sum(x => x.PlayerComm);
        }

        private static void SortData(BetListMetaModel metaModel, SortableList<LrfTicket> ticketList)
        {
            if (!metaModel.IsServerPaging && !string.IsNullOrWhiteSpace(metaModel.SortBy))
            {
                // Because Winloss and Commission are shared the same column (in grid)
                // so we need to sort by the total of two values which is stored in WinlossComm property.
                if (metaModel.SortBy.Equals("WinLoss", StringComparison.OrdinalIgnoreCase))
                {
                    metaModel.SortBy = "WinLossComm";
                }

                if (metaModel.SortBy.Equals("LicenseeWL", StringComparison.OrdinalIgnoreCase))
                {
                    metaModel.SortBy = "LicenseeWLComm";
                }

                if (metaModel.SortBy.Equals("nickname", StringComparison.OrdinalIgnoreCase))
                {
                    metaModel.SortBy = "ExternalId";
                }

                ticketList.Sort(metaModel.SortBy, metaModel.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}