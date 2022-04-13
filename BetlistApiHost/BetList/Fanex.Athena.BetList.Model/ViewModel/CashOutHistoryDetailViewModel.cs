namespace Fanex.Athena.BetList.Model.ViewModel
{
    public class CashOutHistoryDetailViewModel
    {
        public long TransId { get; set; }

        public decimal Rate { get; set; }

        public string CashoutExtraInfo { get; set; }

        public string Currency { get; set; }

        public string BaseCurrency { get; set; }
    }
}