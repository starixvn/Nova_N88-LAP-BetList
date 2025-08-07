namespace Fanex.Athena.BetList.Model.MetaModel
{
    using System;

    public class BaseBettingTicketMetaModel : ICommonValueMetaModel
    {
        private const int DefaultPageSize = 50;

        private string _baseCurrency = string.Empty;

        private string _userName;

        public BaseBettingTicketMetaModel()
        {
            PageIndex = 1;
            PageSize = DefaultPageSize;
        }

        public string BaseCurrency
        {
            get
            {
                return _baseCurrency;
            }

            set
            {
                _baseCurrency = value == null
                    ? string.Empty
                    : value.ToLowerInvariant();
            }
        }

        public string AgentListXml { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string ProductCodeList { get; set; }

        public string UserName
        {
            get
            {
                return _userName;
            }

            set
            {
                _userName = value?.Trim();
            }
        }

        public string Status { get; set; }

        public bool IsExport { get; set; }

        public byte SortType { get; set; }

        public byte SortOrder { get; set; }

        public virtual bool IsValid => FromDate <= ToDate && !string.IsNullOrWhiteSpace(ProductCodeList);

        public bool HasPlayerCom { get; set; }

        public bool IsConvertAll { get; set; }

        public string BaseCurrencyName { get; set; }

        public int CustId { get; set; }

        public bool IsShowIpvn { get; set; }

        public bool ViewSabaCoins { get; set; }
    }
}