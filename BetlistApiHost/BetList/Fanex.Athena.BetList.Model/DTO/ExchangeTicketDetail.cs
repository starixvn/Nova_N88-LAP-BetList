namespace Fanex.Athena.BetList.Model.DTO
{
    using System;
    using Common.Constant;

    public class ExchangeTicketDetail
    {
        public DateTime BetTaken { get; set; }

        public decimal OddsReq { get; set; }

        public decimal Stake { get; set; }

        public string Liability { get; set; }

        public decimal OddsMatched { get; set; }

        public string BetTakenDateString => BetTaken.ToString(FormatKey.DateTimeFormat);
    }
}