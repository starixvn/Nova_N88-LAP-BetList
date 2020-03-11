namespace Fanex.Athena.BetList.Model.DTO
{
    using System.Collections.Generic;

    public class GroupTicketResult
    {
        public IList<string> DataPath { get; set; } = new List<string>();

        public int RowPerGroup { get; set; }

        public int TotalRow { get; set; }

        public int TotalRows { get; set; }

        public decimal SumWinLoss { get; set; }

        public decimal SumBetEffective { get; set; }

        public decimal SumLicenseeWL { get; set; }

        public decimal SumComm { get; set; }

        public decimal SumPlayerComm { get; set; }

        public decimal SumStake { get; set; }
    }
}