namespace Fanex.Athena.BetList.Model.DTO
{
    using System;
    using Common.Constant;

    public class ExchangeTicket
    {
        public string UserId { get; set; }

        public int EventId { get; set; }

        public long BetId { get; set; }

        public string MarketId { get; set; }

        public string SelectionName { get; set; }

        public decimal OddsAvg { get; set; }

        public decimal Stake { get; set; }

        public string Type { get; set; }

        public DateTime BetPlaced { get; set; }

        public string BetPlacedDateString => BetPlaced.ToString(FormatKey.DateTimeFormat);

        public decimal WinLost { get; set; }
    }
}