namespace Fanex.Athena.BetList.Model.DTO
{
    using System;
    using System.Collections.Generic;

    public class SystemParlayDetailItem
    {
        public DateTime Date { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal WinLoss { get; set; }

        public decimal PlayerComm { get; set; }

        public string Status { get; set; }

        public IEnumerable<SystemParlayDetailMatch> MatchList { get; set; }
    }
}