namespace Fanex.Athena.BetList.Model.MetaModel
{
    public class AdvancedSearchBetListMetaModel : BaseBettingTicketMetaModel
    {
        public string LiveStatus { get; set; }

        public string SportIds { get; set; }

        public string SportListXML { get; set; }

        public string BetTypeIds { get; set; }

        public int LeagueId { get; set; }

        public int MatchId { get; set; }

        public string Ip { get; set; }

        public string OddsGroup { get; set; }

        public string Platform { get; set; }

        public string CustomerClass { get; set; }

        public string FullCustomerClass { get; set; }

        public string LeagueGroupIds { get; set; }

        public string LicenseeName { get; set; }

        public int CashOutMode { get; set; }

        public override bool IsValid
        {
            get
            {
                if (FromDate.Date != ToDate.Date && (LeagueId != 0 || MatchId != 0))
                {
                    return false;
                }

                return base.IsValid;
            }
        }
    }
}