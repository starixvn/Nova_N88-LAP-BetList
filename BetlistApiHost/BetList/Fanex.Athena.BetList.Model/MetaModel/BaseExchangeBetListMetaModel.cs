namespace Fanex.Athena.BetList.Model.MetaModel
{
    public class BaseExchangeBetListMetaModel
    {
        private string _baseCurrency = string.Empty;

        public int CustId { get; set; }

        public int CurrencyId { get; set; }

        public string AgentListXml { get; set; }

        public bool ExcludeTestData { get; set; }

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
                    : value.ToUpperInvariant();
            }
        }

        public virtual bool IsValid => CustId > 0;
    }
}