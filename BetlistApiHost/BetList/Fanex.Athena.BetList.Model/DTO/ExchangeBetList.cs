namespace Fanex.Athena.BetList.Model.DTO
{
    using System.Collections.Generic;

    public class ExchangeBetList
    {
        public ExchangeSumWinLoss Ticket { get; set; }

        public IEnumerable<ExchangeTicket> Details { get; set; }
    }
}