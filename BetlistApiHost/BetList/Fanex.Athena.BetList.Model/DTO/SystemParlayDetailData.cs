namespace Fanex.Athena.BetList.Model.DTO
{
    using System.Collections.Generic;

    public class SystemParlayDetailData
    {
        public decimal Outstanding { get; set; }

        public bool IsParlayBoost { get; set; }

        public IEnumerable<SystemParlayDetailSerial> SerialList { get; set; }

        public IEnumerable<SystemParlayDetailTicket> TicketList { get; set; }
    }
}