namespace Fanex.Athena.BetList.Model.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.BetList.Model.DTO;

    public class ExchangeBetListViewModel
    {
        public IEnumerable<ExchangeBetList> MainData { get; set; }

        public decimal GrandTotal { get; set; }
    }
}
