using System.Collections.Generic;
using NPOI.SS.UserModel;
using NSubstitute;

namespace Fanex.Athena.UnitTest.Common.Helpers
{
    public static class ExcelTestHelper
    {
        public static void MockGetRow(ISheet sheet, int columnCount)
        {
            var row = Substitute.For<IRow>();
            var cell = Substitute.For<ICell>();
            cell.Row.Returns(row);
            var cellList = new List<ICell>();
            for (int i = 0; i < columnCount; i++)
            {
                cellList.Add(cell);
            }

            row.Cells.ReturnsForAnyArgs(cellList);
            sheet.GetRow(0).ReturnsForAnyArgs(row);
        }

        public static IRow MockGetRow(ISheet sheet, int columnCount, int destRowIndex)
        {
            var row = Substitute.For<IRow>();
            var cellList = new List<ICell>(columnCount);

            for (int i = 0; i < columnCount; i++)
            {
                var cell = Substitute.For<ICell>();
                cellList.Add(cell);
            }

            row.Cells.ReturnsForAnyArgs(cellList);
            sheet.GetRow(destRowIndex).Returns(row);
            return row;
        }

        public static ICell MockGetCell(ISheet sheet, int rowIndex, int columnIndex)
        {
            return sheet.GetRow(rowIndex).GetCell(columnIndex);
        }
    }
}