using NPOI.SS.UserModel;

namespace Fanex.Athena.BetList.Excel.Helpers
{
    public static class ExcelHelper
    {
        public static void HideColumn(ISheet sheet, params int[] columnIndex)
        {
            // Clear content
            var firstRowNum = sheet.FirstRowNum;
            var lastRowNum = sheet.LastRowNum;

            for (int i = firstRowNum; i <= lastRowNum; i++)
            {
                foreach (int j in columnIndex)
                {
                    var cell = GetCell(sheet, i, j);
                    if (cell != null)
                    {
                        cell.SetCellValue(string.Empty);
                    }
                }
            }

            // Hide column
            foreach (int i in columnIndex)
            {
                sheet.SetColumnWidth(i, 0);
                sheet.SetColumnHidden(i, true);
            }
        }

        private static ICell GetCell(ISheet sheet, int rowIndex, int colIndex)
        {
            return sheet.GetRow(rowIndex).GetCell(colIndex);
        }
    }
}