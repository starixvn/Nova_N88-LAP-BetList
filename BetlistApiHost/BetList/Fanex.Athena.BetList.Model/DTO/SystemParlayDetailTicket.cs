namespace Fanex.Athena.BetList.Model.DTO
{
    using System;

    public class SystemParlayDetailTicket
    {
        public decimal Comm { get; set; }

        public decimal MOdds { get; set; }

        public decimal PlayerComm { get; set; }

        public long Serial { get; set; }

        public decimal Stake { get; set; }

        public string Status { get; set; }

        public int StatusId { get; set; }

        public long TransId { get; set; }

        public decimal WinLost { get; set; }

        public DateTime WinLostDate { get; set; }
    }
}