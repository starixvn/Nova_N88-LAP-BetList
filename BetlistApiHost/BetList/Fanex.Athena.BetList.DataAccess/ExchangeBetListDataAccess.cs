namespace Fanex.Athena.BetList.DataAccess
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Common.DataAccess;
    using Common.Utility.Helpers;
    using Fanex.Athena.Common.DataAccess.Interfaces;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Interfaces;
    using Model.DTO;
    using Model.MetaModel;

    public class ExchangeBetListDataAccess : BaseDataAccess, IExchangeBetListDataAccess
    {
        private const string ExchangeMatchTypeLay = "Lay";
        public async Task<ExchangeBetListResult> GetBetListDataAsync(ExchangeBetListMetaModel metaModel)
        {
            const string UspName = "Lrf_BetList_Exchange_Mem";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("CustId", metaModel.CustId);
                db.SetParameterValue("FromDate", metaModel.FromDate);
                db.SetParameterValue("ToDate", metaModel.ToDate);
                db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
                db.SetParameterValue("SumWinLost", null);

                var result = new ExchangeBetListResult();

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    result.SumWinLossList = ReadSumWinLossData(reader);
                    result.TicketList = ReadTicketData(reader);
                }

                result.GrandTotal = Converter.ToDecimal(db.GetParameterValue("SumWinLost"));
                return result;
            }
        }

        public async Task<IEnumerable<ExchangeTicketDetail>> GetTicketDetailListAsync(ExchangeBetListDetailMetaModel metaModel)
        {
            const string UspName = "Lrf_BetList_Exchange_Detail";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("CustId", metaModel.CustId);
                db.SetParameterValue("BetID", metaModel.BetId);
                db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    return ReadTicketDetailData(reader, metaModel.Type);
                }
            }
        }

        private IEnumerable<ExchangeTicketDetail> ReadTicketDetailData(IDataReader reader, string type)
        {
            var mainData = new List<ExchangeTicketDetail>();

            while (reader.Read())
            {
                var data = new ExchangeTicketDetail
                {
                    BetTaken = reader.ToDateTime("BetTaken"),
                    OddsReq = reader.ToDecimal("OddsRequest"),
                    Stake = reader.ToDecimal("Stake"),
                    OddsMatched = reader.ToDecimal("OddsMatched")
                };

                data.Liability = type == ExchangeMatchTypeLay ? NumberHelper.DecFormat((data.OddsMatched - 1) * data.Stake) : "-";
                mainData.Add(data);
            }

            return mainData;
        }

        private IEnumerable<ExchangeSumWinLoss> ReadSumWinLossData(IDataReader reader)
        {
            var mainData = new List<ExchangeSumWinLoss>();

            while (reader.Read())
            {
                mainData.Add(new ExchangeSumWinLoss
                {
                    UserId = reader.ToString("UserId"),
                    EventId = reader.ToInt("EventId"),
                    EventName = reader.ToString("EventName"),
                    EventTypeName = reader.ToString("EventTypeName"),
                    MarketId = reader.ToString("MarketId"),
                    MarketName = reader.ToString("MarketName"),
                    WinLost = reader.ToDecimal("WinLost"),
                    Commission = reader.ToDecimal("Commission")
                });
            }

            return mainData;
        }

        private IEnumerable<ExchangeTicket> ReadTicketData(IDataReader reader)
        {
            var detailData = new List<ExchangeTicket>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    detailData.Add(new ExchangeTicket
                    {
                        UserId = reader.ToString("UserId"),
                        EventId = reader.ToInt("EventId"),
                        MarketId = reader.ToString("MarketId"),
                        BetId = reader.ToLong("BetId"),
                        SelectionName = reader.ToString("SelectionName"),
                        OddsAvg = reader.ToDecimal("OddsAvg"),
                        Stake = reader.ToDecimal("Stake"),
                        Type = reader.ToString("Type"),
                        BetPlaced = reader.ToDateTime("BetPlaced"),
                        WinLost = reader.ToDecimal("WinLost")
                    });
                }
            }

            return detailData;
        }
    }
}