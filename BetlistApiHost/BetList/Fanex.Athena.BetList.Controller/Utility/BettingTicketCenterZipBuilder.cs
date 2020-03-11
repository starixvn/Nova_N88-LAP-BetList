using System.Collections.Generic;
using System.IO;
using System.Web;
using Fanex.Athena.BetList.Excel;
using Fanex.Athena.BetList.Excel.Model;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Helpers;
using Fanex.Athena.Common.Utility.Services;
using Fanex.Athena.Common.Utility.Services.Interfaces;
using Ionic.Zip;
using Newtonsoft.Json;
using NPOI.SS.UserModel;

namespace Fanex.Athena.BetList.Controller.Utility
{
    public class BettingTicketCenterZipBuilder : IBettingTicketCenterZipBuilder
    {
        private readonly IAppSettings _appSettings;

        private readonly ITempFileService _tempFileService;
        private IBettingTicketCenterExcel _bettingTicketCenterExcel;

        public BettingTicketCenterZipBuilder(
            IAppSettings appSettings,
            ITempFileService tempFileService,
            IBettingTicketCenterExcel bettingTicketCenterExcel
            )
        {
            _appSettings = appSettings ?? new AppSettings();
            _tempFileService = tempFileService ?? new TempFileService(_appSettings, null);
            _bettingTicketCenterExcel = bettingTicketCenterExcel ?? new BettingTicketCenterExcel(null);
        }

        public IBettingTicketCenterExcel ExcelBuilder
        {
            get { return _bettingTicketCenterExcel ?? new BettingTicketCenterExcel(null); }
            set { _bettingTicketCenterExcel = value; }
        }

        public MemoryStream BuildZip(HttpResponseBase response, GroupTicketResult ticketResult, string title, bool hasPlayerComm, string totalText)
        {
            var excelFiles = new List<string>();

            for (int i = 0; i < ticketResult.DataPath.Count; i++)
            {
                var contentData = _tempFileService.ReadString(ticketResult.DataPath[i]);
                var excelModel = CreateExcelModel(ticketResult, contentData, title, hasPlayerComm, totalText);
                if (excelModel != null)
                {
                    var workbook = ExcelBuilder.Export(excelModel);
                    var dataFileName = ticketResult.DataPath[i] + ".dat";
                    var dataFilePath = Path.Combine(_appSettings.TempFilePath, dataFileName);

                    SaveFile(workbook, dataFilePath);

                    excelFiles.Add(dataFileName);
                    BetListHelper.ClearData(excelModel.Data);
                    excelModel.Data = null;
                    ExcelBuilder.Dispose();
                    ExcelBuilder = null;
                }

            }

            CleanDataFile(ticketResult.DataPath);
            return CreateZipFile(excelFiles);
        }

        private static BettingTicketCenterExcelModel CreateExcelModel(
            GroupTicketResult groupData,
            string contentData,
            string title,
            bool hasPlayerComm,
            string totalText)
        {
            if (string.IsNullOrEmpty(contentData))
            {
                return null;
            }

            var dataBySheet = JsonConvert.DeserializeObject<TicketResult>(contentData);

            dataBySheet.SumStake = groupData.SumStake;
            dataBySheet.SumWinLoss = groupData.SumWinLoss;
            dataBySheet.SumLicenseeWL = groupData.SumLicenseeWL;
            dataBySheet.SumBetEffective = groupData.SumBetEffective;
            dataBySheet.SumComm = groupData.SumComm;
            dataBySheet.SumPlayerComm = groupData.SumPlayerComm;
            dataBySheet.TotalRows = groupData.TotalRows;

            return new BettingTicketCenterExcelModel
            {
                Data = dataBySheet,
                Option = new BettingTicketCenterOption
                {
                    ShowTotal = true,
                    ShowSumStake = true,
                    ShowLicenseeWl = true,
                    ShowWinLoss = true,
                    ShowExternalId = true,
                    ShowSystemId = true,
                    IsAllowedCustomerComm = hasPlayerComm
                },
                Title = title,
                TotalText = totalText
            };
        }

        protected internal virtual void SaveFile(IWorkbook workbook, string dataFilePath)
        {
            using (var stream = new FileStream(dataFilePath, FileMode.CreateNew, FileAccess.Write))
            using (var bufferStream = new BufferedStream(stream))
            {
                workbook.Write(bufferStream);
                workbook.Clear();
            }
        }

        private void CleanDataFile(IList<string> dataPath)
        {
            foreach (var item in dataPath)
            {
                _tempFileService.Delete(item);
            }
        }

        private MemoryStream CreateZipFile(IList<string> excelFiles)
        {
            var mem = new MemoryStream();
            using (var zip = new ZipFile())
            {
                for (var i = 0; i < excelFiles.Count; i++)
                {
                    zip.AddEntry($"sheet{i}.xls", _tempFileService.ReadBytes(excelFiles[i]));
                }

                CleanDataFile(excelFiles);
                zip.Save(mem);
            }
            return mem;
        }
    }
}