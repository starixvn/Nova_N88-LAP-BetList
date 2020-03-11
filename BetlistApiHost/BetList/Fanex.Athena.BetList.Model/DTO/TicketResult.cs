// -----------------------------------------------------------------------
// <copyright file="TicketResult.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.Model.DTO
{
    using System.Collections.Generic;
    using Common.Utility.Collections;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// TicketResult class.
    /// </summary>
    public class TicketResult
    {
        [System.CLSCompliant(false)]
        public SortableList<LrfTicket> MainTicketList { get; set; } = new SortableList<LrfTicket>();

        [System.CLSCompliant(false)]
        public List<ITicketData> TicketDataList { get; set; } = new List<ITicketData>();

        // Sum value
        public int TotalRows { get; set; }

        public decimal SumWinLoss { get; set; }

        public decimal SumBetEffective { get; set; }

        public decimal SumLicenseeWL { get; set; }

        public decimal SumComm { get; set; }

        public decimal SumPlayerComm { get; set; }

        public decimal SumStake { get; set; }
    }
}