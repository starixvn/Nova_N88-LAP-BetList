// -----------------------------------------------------------------------
// <copyright file="BetListMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.Model.MetaModel
{
    using System;
    using System.Collections.Generic;
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

        [ScriptIgnore]
        public string UspKey { get; set; }

        public string ProductCodeList { get; set; } = string.Empty;

        public bool IsShowSumStake { get; set; }

        public bool IsServerPaging { get; set; }

        public string SortBy { get; set; }

        public string SortOrder { get; set; }

        public bool IsExportExcel { get; set; }

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

        public int MatchId { get; set; }

        public int LeagueId { get; set; }

        public int BetType { get; set; }

        public string MultipleBettype { get; set; }

        public string BetTeam { get; set; }

        public int? LiveIndicator { get; set; }

        public string RaceNo { get; set; }

        public int DrawFilter { get; set; }

        public long TransId { get; set; }

        public long TransType { get; set; }

        public int PlatformType { get; set; }

        public string BetChoice { get; set; }

        public string Handicap { get; set; }

        public long RefCode { get; set; }

        public virtual bool IsValid
        {
            get
            {
                return FromDate <= ToDate
                    && !string.IsNullOrWhiteSpace(ProductCodeList);
            }
        }
    }
}