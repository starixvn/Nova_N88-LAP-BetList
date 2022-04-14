using System;

namespace Fanex.Athena.BetList.Model.MetaModel
{
    public class CashOutHistoryDetailMetaModel : CashOutBetListDetailMetaModel
    {
        public int CustId { get; set; }

        public int CurrencyId { get; set; }

        public string Currency { get; set; }

        public DateTime Date { get; set; }
    }
}