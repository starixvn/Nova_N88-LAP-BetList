namespace Fanex.Athena.BetList.Model.ViewModel
{
    using System.Collections.Generic;
    using DTO;

    public class CashOutBetListDetailViewModel
    {
        public TicketText OriginalTicketText { get; set; }

        [System.CLSCompliant(false)]
        public LrfTicket OriginalTicket { get; set; }

        public IEnumerable<CashOutTicket> CashOutTicketList { get; set; }

        public IEnumerable<CashOutTicketText> CashOutTicketTextList { get; set; }

        public bool IsCashOut { get; set; }

        public bool IsHighlightOriginalTicket { get; set; }
    }
}