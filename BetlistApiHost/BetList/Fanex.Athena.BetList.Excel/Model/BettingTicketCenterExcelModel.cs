using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;

namespace Fanex.Athena.BetList.Excel.Model
{
    public class BettingTicketCenterExcelModel
    {
        public TicketResult Data { get; set; }

        public int LicenseeId { get; set; }

        public string TotalText { get; set; }

        public string Title { get; set; }

        public BettingTicketCenterOption Option { get; set; }
    }
}
