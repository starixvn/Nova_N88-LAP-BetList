// -----------------------------------------------------------------------
// <copyright file="BetListMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.Model.MetaModel
{
    using System;
    using System.Web.Script.Serialization;

    public class BetListMetaModel : BaseMetaModel, ICommonValueMetaModel
    {
        public BetListMetaModel()
        {
            FromDate = DateTime.Today;
            Date = DateTime.Today;
            ToDate = DateTime.Today;
            PageIndex = 1;
            PageSize = int.MaxValue - 1;
        }

        public bool HasPlayerCom { get; set; }

        public bool IsShowIpvn { get; set; }

        [ScriptIgnore]
        public string UspKey { get; set; }

        public string ProductCodeList { get; set; } = string.Empty;

        public bool IsShowSumStake { get; set; }

        public bool IsServerPaging { get; set; }

        public string SortBy { get; set; }

        public string SortOrder { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime Date { get; set; }

        public int CustId { get; set; }

        public string SystemId { get; set; }

        public string ExternalId { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public int DrawFilter { get; set; }

        public long TransId { get; set; }

        public long TransType { get; set; }

        public long RefCode { get; set; }

        public byte FilterBy { get; set; }
        public int TimeZone { get; set; }
        public int ProductId { get; set; }
        public bool ExcludeDrawTicket { get; set; }
    }
}