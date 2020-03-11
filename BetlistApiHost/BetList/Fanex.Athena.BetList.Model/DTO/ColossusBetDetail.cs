namespace Fanex.Athena.BetList.Model.DTO
{
    using System;

    public class ColossusBetDetail
    {
        public DateTime TransDate { get; set; }

        public string TransactionType { get; set; }

        public string WinLoss { get; set; }

        public decimal Commission { get; set; }
    }
}