// -----------------------------------------------------------------------
// <copyright file="DailyStatement.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.Model.DTO
{
    /// <summary>
    /// LRFTicket class.
    /// </summary>
    public class DailyStatement
    {
        public int Index { get; set; }

        public string Date { get; set; }

        public string Remark { get; set; }

        public string Turnover { get; set; }

        public string CreditDebit { get; set; }

        public string Commission { get; set; }

        public string Balance { get; set; }

        public string TransType { get; set; }

        public string FullStatementDate { get; set; }

        public string TransDesc { get; set; }

        public bool TransTypeHasBetList { get; set; }

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public int CustId { get; set; }
    }
}