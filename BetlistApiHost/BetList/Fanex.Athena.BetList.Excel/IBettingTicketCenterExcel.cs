using System;
using BetListNPOI.SS.UserModel;
using Fanex.Athena.BetList.Excel.Model;

namespace Fanex.Athena.BetList.Excel
{
    public interface IBettingTicketCenterExcel : IDisposable
    {
        IWorkbook Export(BettingTicketCenterExcelModel excelModel);
    }
}