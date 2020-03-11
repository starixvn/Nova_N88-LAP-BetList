namespace Fanex.Athena.BetList.Excel
{
    using System;
    using System.Linq;
    using Builders;
    using Common.Constant;
    using Model;
    using NPOI.SS.UserModel;
    using Resources;

    public class BettingTicketCenterExcel : IBettingTicketCenterExcel
    {
        private readonly IBettingTicketCenterExcelBuilder _builder;

        private bool disposed;

        public BettingTicketCenterExcel(IBettingTicketCenterExcelBuilder builder)
        {
            _builder = builder ?? new BettingTicketCenterExcelBuilder(null, null);
        }

        public virtual IWorkbook Export(BettingTicketCenterExcelModel excelModel)
        {
            if (excelModel.Data.MainTicketList.Any())
            {
                _builder.InitializeFonts();
                _builder.InitializeCellStyles();
                BuildSheets(excelModel);
            }
            else
            {
                _builder.CreateSheet(ViewResources.BettingTicketCenter, excelModel.Option);
                _builder.AddTitle(excelModel.Title, excelModel.Option);
                _builder.AddHeader(excelModel.Option);
                _builder.AddNoInfoRow(excelModel.Option);
            }

            return _builder.GetWorkbook();
        }

        protected internal virtual void BuildSheets(BettingTicketCenterExcelModel excelModel)
        {
            var numOfSheets = (int)Math.Ceiling((double)excelModel.Data.MainTicketList.Count / SystemConstants.SheetSizeExcel);
            for (int sheetIndex = 0; sheetIndex < numOfSheets; sheetIndex++)
            {
                _builder.CreateSheet(ViewResources.Page + (sheetIndex + 1), excelModel.Option);
                _builder.AddTitle(excelModel.Title, excelModel.Option);
                _builder.AddHeader(excelModel.Option);

                var endIndex = Math.Min((sheetIndex + 1) * SystemConstants.SheetSizeExcel, excelModel.Data.MainTicketList.Count);

                AssignSheetData(excelModel, sheetIndex, endIndex);

                if (excelModel.Option.ShowTotal)
                {
                    AddFooter(excelModel);
                }

                _builder.GetWorkbook(); // Adjust row height for current sheet
            }
        }

        protected internal virtual void AssignSheetData(BettingTicketCenterExcelModel excelModel, int sheetIndex, int endIndex)
        {
            for (var i = sheetIndex * SystemConstants.SheetSizeExcel; i < endIndex; i++)
            {
                _builder.AssignRowData(
                    excelModel.Data,
                    excelModel.Data.MainTicketList[i],
                    excelModel.LicenseeId,
                    excelModel.Option);
            }
        }

        protected internal virtual void AddFooter(BettingTicketCenterExcelModel excelModel)
        {
            _builder.AddFooter(
                excelModel.Data.SumStake,
                excelModel.Data.SumWinLoss,
                excelModel.Data.SumPlayerComm,
                excelModel.Data.SumLicenseeWL + excelModel.Data.SumComm,
                excelModel.TotalText,
                excelModel.Option);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing && _builder != null)
            {
                ((IDisposable)_builder)?.Dispose();
            }

            disposed = true;
        }
    }
}