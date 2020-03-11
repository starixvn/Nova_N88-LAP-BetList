using System;
using Fanex.Athena.BetList.Excel.Model;
using NPOI.SS.UserModel;

namespace Fanex.Athena.BetList.Excel
{
    public interface IBettingTicketCenterExcel : IDisposable
    {
        IWorkbook Export(BettingTicketCenterExcelModel excelModel);
    }
}