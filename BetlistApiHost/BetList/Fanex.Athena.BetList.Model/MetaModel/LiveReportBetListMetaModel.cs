// -----------------------------------------------------------------------
// <copyright file="LiveReportBetListMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.Model.MetaModel
{
    public class LiveReportBetListMetaModel : BetListExcelMetaModel
    {
        public int SportType { get; set; }

        public decimal SumWinlost { get; set; }

        public decimal SumLicenseeWL { get; set; }

        public decimal SumComm { get; set; }

        public decimal SumPlayerComm { get; set; }

        public int TotalRows { get; set; }
    }
}