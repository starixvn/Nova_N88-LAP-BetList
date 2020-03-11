namespace Fanex.Athena.BetList.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Fanex.Athena.BetList.DataAccess.Interfaces;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Athena.Common.DataAccess;
    using Fanex.Athena.Common.DataAccess.Interfaces;
    using Fanex.Common;
    using Fanex.Data;

    public class SystemParlayDetailDataAccess : BaseDataAccess, ISystemParlayDetailDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public SystemParlayDetailDataAccess(IBalanceUpDataAccess balanceUpDataAccess)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        [CLSCompliant(false)]
        public async Task<SystemParlayDetailData> GetData(SystemParlayDetailMetaModel metaModel)
        {
            const string UspName = "Lrf_BetList_SelCombinationParlayDetail";
            using (var db = this.GetObjectDb(UspName))
            {
                SetSystemParlayDetailParameter(db, metaModel);

                var dataSet = await db.ExecuteDataSetAsync().ConfigureAwait(false);
                var ticketList = new List<SystemParlayDetailTicket>();
                var serialList = new List<SystemParlayDetailSerial>();

                if (dataSet.Tables.Count > 0)
                {
                    using (var ticketData = dataSet.Tables[0])
                    {
                        foreach (DataRow row in ticketData.Rows)
                        {
                            ticketList.Add(ReadSystemParlayTicket(row));
                        }
                    }

                    if (dataSet.Tables.Count > 1)
                    {
                        using (var serialData = dataSet.Tables[1])
                        {
                            foreach (DataRow row in serialData.Rows)
                            {
                                serialList.Add(ReadSystemParlaySerial(row));
                            }
                        }
                    }
                }

                return new SystemParlayDetailData
                {
                    Outstanding = Converter.ToDecimal(db.GetParameterValue("outstanding")),
                    TicketList = ticketList,
                    SerialList = serialList
                };
            }
        }

        private void SetSystemParlayDetailParameter(IObjectDb db, SystemParlayDetailMetaModel metaModel)
        {
            var balanceUp = _balanceUpDataAccess.GetBalanceUp();

            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("refno", metaModel.RefNo);
            db.SetParameterValue("movebetsday", balanceUp.MoveBetsDay);
            db.SetParameterValue("balanceupday", balanceUp.BalanceUpDay);
            db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
            db.SetParameterValue("outstanding", null);
        }

        private static SystemParlayDetailTicket ReadSystemParlayTicket(DataRow row)
        {
            return new SystemParlayDetailTicket
            {
                Comm = Converter.ToDecimal(row["comm"]),
                MOdds = Converter.ToDecimal(row["modds"]),
                PlayerComm = Converter.ToDecimal(row["playercomm"]),
                Serial = Convert.ToInt64(row["serial"]),
                Stake = Converter.ToDecimal(row["stake"]),
                Status = Converter.ToString2(row["status"]),
                StatusId = Converter.ToInt(row["statusid"]),
                WinLost = Converter.ToDecimal(row["winlost"]),
                WinLostDate = Converter.ToDateTime(row["winlostdate"])
            };
        }

        private static SystemParlayDetailSerial ReadSystemParlaySerial(DataRow row)
        {
            return new SystemParlayDetailSerial
            {
                Serial = Convert.ToInt64(row["serial"]),
                HomeId = Converter.ToInt(row["homeid"]),
                AwayId = Converter.ToInt(row["awayid"]),
                IsNeutral = Converter.ToBool(row["IsNeutral"])
            };
        }
    }
}