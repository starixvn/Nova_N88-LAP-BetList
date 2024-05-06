namespace Fanex.Athena.BetList.Model.MetaModel
{
    public class BookedBetBetListMetaModel : BaseBettingTicketMetaModel
    {
        public string Platform { get; set; }

        public string SearchText { get; set; }

        public int SearchOption { get; set; }

        public override bool IsValid => FromDate <= ToDate && !string.IsNullOrWhiteSpace(SearchText);
    }
}