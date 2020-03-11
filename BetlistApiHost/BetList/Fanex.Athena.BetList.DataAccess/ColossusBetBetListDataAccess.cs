// -----------------------------------------------------------------------
// <copyright file="ColossusBetBetListDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Common.Utility.Helpers;
    using Fanex.Athena.Common.DataAccess.Interfaces;
    using Fanex.BetList.ColossusBets.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.Common.Extensions;
    using Interfaces;
    using Model.DTO;
    using Model.MetaModel;
    using Model.ViewModel;

    public class ColossusBetBetListDataAccess : BaseBetListDataAccess, IColossusBetBetListDataAccess
    {
        private const int SoldBetType = 18002;

        private readonly ITicketHelper _ticketHelper;

        [CLSCompliant(false)]
        public ColossusBetBetListDataAccess(ITicketHelper ticketHelper) : base(null)
        {
            _ticketHelper = ticketHelper;
        }

        public int GetSiteId(int custId)
        {
            const string UspKey = "Acc_CustomerSelSiteByCustId";

            using (var db = this.GetObjectDb(UspKey))
            {
                db.SetParameterValue("custid", custId);
                db.SetParameterValue("siteid", null);
                db.ExecuteNonQuery();
                return (int)db.GetParameterValue("siteid");
            }
        }

        public async Task<ColossusBetDetailViewModel> GetData(ColossusBetDetailMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_CB_Detail";

            using (var db = this.GetObjectDb(UspKey))
            {
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("custid", metaModel.CustId);
                db.SetParameterValue("refno", metaModel.RefNo);
                db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    return ReadData(reader, _ticketHelper);
                }
            }
        }

        private static ColossusBetDetailViewModel ReadData(IDataReader reader, ITicketHelper ticketHelper)
        {
            var dataList = new List<ColossusBetDetail>();
            decimal totalSoldPercent = 0;
            decimal totalWinLoss = 0;
            decimal totalComm = 0;
            decimal totalSoldWinLoss = 0;
            while (reader.Read())
            {
                var bettypeId = reader.ToInt("BetType");
                var sold = reader.ToDecimal("Sold");
                var winloss = reader.ToDecimal("winlost");
                var comm = reader.ToDecimal("comm");
                totalSoldPercent += sold;
                totalWinLoss += winloss;
                totalComm += comm;
                totalSoldWinLoss += bettypeId == SoldBetType ? winloss : 0;
                dataList.Add(new ColossusBetDetail
                {
                    TransDate = reader.ToDateTime("TransDate"),
                    TransactionType = BuilderExtension.BuildColossusBetsTransactionType(bettypeId, sold, ticketHelper),
                    WinLoss = BuilderExtension.BuildColossusBetsWinLoss(winloss),
                    Commission = comm
                });
            }

            return new ColossusBetDetailViewModel
            {
                DataList = dataList,
                TotalSoldPercent = totalSoldPercent.ToPercentage(),
                TotalWinLoss = totalWinLoss,
                TotalComm = totalComm,
                TotalSoldWinLoss = totalSoldWinLoss
            };
        }
    }
}