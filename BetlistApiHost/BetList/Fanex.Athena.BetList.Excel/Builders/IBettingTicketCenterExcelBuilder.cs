using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Model.DTO;
using NPOI.SS.UserModel;

namespace Fanex.Athena.BetList.Excel.Builders
{
    public interface IBettingTicketCenterExcelBuilder
    {
        void CreateSheet(string sheetName, BettingTicketCenterOption option);

        IWorkbook GetWorkbook();

        void AddFooter(decimal sumStake, decimal sumWinLoss, decimal sumPlayerComm, decimal sumLicenseeWL, string totalText, BettingTicketCenterOption option);

        void AddHeader(BettingTicketCenterOption option);

        void AddNoInfoRow(BettingTicketCenterOption option);

        void AddTitle(string title, BettingTicketCenterOption option);

        void AssignRowData(TicketResult ticketResult, LrfTicket ticket, int licenseeId, BettingTicketCenterOption option);

        void InitializeCellStyles();

        void InitializeFonts();
    }
}