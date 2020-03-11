namespace Fanex.Athena.BetList.Model.DTO
{
    using System.Collections.Generic;

    public class ExchangeBetListResult
    {
        public IEnumerable<ExchangeSumWinLoss> SumWinLossList { get; set; }

        public IEnumerable<ExchangeTicket> TicketList { get; set; }

        public decimal GrandTotal { get; set; }
    }
}