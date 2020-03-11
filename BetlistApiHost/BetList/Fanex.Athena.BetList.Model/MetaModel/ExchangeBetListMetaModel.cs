namespace Fanex.Athena.BetList.Model.MetaModel
{
    using System;

    public class ExchangeBetListMetaModel : BaseExchangeBetListMetaModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override bool IsValid => base.IsValid && FromDate <= ToDate;
    }
}