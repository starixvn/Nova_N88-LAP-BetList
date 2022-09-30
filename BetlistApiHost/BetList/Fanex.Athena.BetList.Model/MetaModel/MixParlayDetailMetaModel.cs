using System;

namespace Fanex.Athena.BetList.Model.MetaModel
{
    public class MixParlayDetailMetaModel : IBalanceUpMetaModel
    {
        public long TransId { get; set; }

        public int CustId { get; set; }

        public bool IsShowIpvn { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }
    }
}