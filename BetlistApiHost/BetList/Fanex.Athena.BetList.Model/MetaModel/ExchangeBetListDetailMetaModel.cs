namespace Fanex.Athena.BetList.Model.MetaModel
{
    public class ExchangeBetListDetailMetaModel : BaseExchangeBetListMetaModel
    {
        public long BetId { get; set; }

        public string Type { get; set; }

        public override bool IsValid => base.IsValid && BetId > 0;
    }
}