namespace Fanex.Athena.BetList.Model.MetaModel
{
    public class NewParlayDetailMetaModel : MixParlayDetailMetaModel
    {
        public string BetTeam { get; set; }

        public string BetCheck { get; set; }

        public decimal Handicap1 { get; set; }

        public decimal Handicap2 { get; set; }
    }
}