namespace Fanex.Athena.BetList.Model.MetaModel
{
    using System;

    public class BetListByProductMetaModel : ICommonValueMetaModel
    {
        public string BaseCurrency { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int CustId { get; set; }

        public DateTime WinlostDate { get; set; }

        public string ProductCode { get; set; }

        public bool HasPlayerCom { get; set; }

        public bool IsShowIpvn { get; set; }

        public int TransType { get; set; }

        public string AgentListXml { get; set; }

        public string Sort { get; set; } = "Transid ASC";
    }
}
