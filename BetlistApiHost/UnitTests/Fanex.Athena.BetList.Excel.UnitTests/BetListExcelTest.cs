using Fanex.Athena.BetList.Excel;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.Athena.Common.Utility.Services.Interfaces;
using Fanex.Athena.Resources;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.BetList.Excel.UnitTests
{
    [TestFixture]
    internal class BetListExcelTest
    {
        private const int LicenseeId = 9999;
        private IAppSettings _appSettings;
        private ITicketHelper _ticketHelper;
        private IBetListExcelBuilder _builder;
        private IIPService _ipService;

        [SetUp]
        public void Setup()
        {
            _appSettings = Substitute.For<IAppSettings>();
            _ticketHelper = Substitute.For<ITicketHelper>();
            _builder = Substitute.For<IBetListExcelBuilder>();
            _ipService = Substitute.For<IIPService>();
        }

        [Test]
        public void Export_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const string Title = "aaaa";
            var betListExcel = CreateBetListExcel();
            var workbook = Substitute.For<IWorkbook>();
            _builder.GetBetList().Returns(workbook);
            var ticketResult = new TicketResult();
            betListExcel.When(x => x.Build(_builder, ticketResult, Title)).DoNotCallBase();
            betListExcel.When(x => x.ProcessUi(workbook)).DoNotCallBase();

            // Action
            var result = betListExcel.Export(ticketResult, Title);

            // Assert
            Assert.Multiple(() =>
            {
                betListExcel.Received(1).Build(_builder, ticketResult, Title);
                betListExcel.Received(1).ProcessUi(workbook);
                Assert.That(result, Is.EqualTo(workbook));
            });
        }

        [Test]
        public void Build_NoData_CallRelevantMethods()
        {
            // Arrange
            const string Title = "aaaa";
            var betListExcel = CreateBetListExcel();
            var workbook = Substitute.For<IWorkbook>();
            _builder.GetBetList().Returns(workbook);
            var ticketResult = new TicketResult();

            // Action
            betListExcel.Build(_builder, ticketResult, Title);

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(1).BuildWorksheet(ViewResources.BetList, 11);
                _builder.Received(1).AddCell(Arg.Is<IRichTextString>(x => x.String == ViewResources.NoInfo), Arg.Any<ICellStyle>());
            });
        }

        [Test]
        public void Build_HasData_CallAssignSheetsData()
        {
            // Arrange
            const string Title = "aaaa";
            var betListExcel = CreateBetListExcel();
            var ticketResult = new TicketResult();
            var ticket = new LrfTicket();
            ticketResult.MainTicketList.Add(ticket);
            betListExcel.When(x => x.AssignSheetsData(_builder, ticketResult, Title, Arg.Any<int>())).DoNotCallBase();

            // Action
            betListExcel.Build(_builder, ticketResult, Title);

            // Assert
            betListExcel.Received(1).AssignSheetsData(_builder, ticketResult, Title, Arg.Any<int>());
        }

        [Test]
        public void ProcessUi_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int HiddenColumnIndex = 100;
            var betListExcel = CreateBetListExcel(hiddenColumns: new[] { HiddenColumnIndex });
            var sheet = Substitute.For<ISheet>();
            var workbook = Substitute.For<IWorkbook>();
            workbook.NumberOfSheets.Returns(1);
            workbook.GetSheetAt(0).Returns(sheet);
            _builder.GetBetList().Returns(workbook);

            // Action
            betListExcel.ProcessUi(workbook);

            // Assert
            Assert.Multiple(() =>
            {
                sheet.Received(1).SetColumnHidden(HiddenColumnIndex, true);
            });
        }

        [Test]
        public void ProcessUi_NotHaveHidenColumns_DoNothing()
        {
            // Arrange
            var betListExcel = CreateBetListExcel(hiddenColumns: null);
            var sheet = Substitute.For<ISheet>();
            var workbook = Substitute.For<IWorkbook>();
            workbook.GetSheetAt(0).Returns(sheet);

            // Action
            betListExcel.ProcessUi(workbook);

            // Assert
            Assert.Multiple(() =>
            {
                sheet.DidNotReceiveWithAnyArgs().SetColumnHidden(Arg.Any<int>(), Arg.Any<bool>());
            });
        }

        [Test]
        public void ProcessUi_FakeLicensee_HideSystemIdColumn()
        {
            // Arrange
            const int HiddenColumnIndex = 100;
            const int ColumnSystemIdIndex = 2;
            _appSettings.FakeLicenseeList.Returns(new[] { LicenseeId });
            var betListExcel = CreateBetListExcel(hiddenColumns: new[] { HiddenColumnIndex });
            var sheet = Substitute.For<ISheet>();
            var workbook = Substitute.For<IWorkbook>();
            workbook.NumberOfSheets.Returns(1);
            workbook.GetSheetAt(0).Returns(sheet);
            _builder.GetBetList().Returns(workbook);

            // Action
            betListExcel.ProcessUi(workbook);

            // Assert
            Assert.Multiple(() =>
            {
                sheet.Received(1).SetColumnHidden(HiddenColumnIndex, true);
                sheet.Received(1).SetColumnHidden(ColumnSystemIdIndex, true);
            });
        }

        private BetListExcel CreateBetListExcel(BetListShowingOption showingOption = null, int[] hiddenColumns = null)
        {
            var betListExcel = Substitute.ForPartsOf<BetListExcel>(
                _appSettings,
                _ticketHelper,
                _ipService,
                showingOption ?? new BetListShowingOption(),
                LicenseeId,
                hiddenColumns,
                false);
            betListExcel.CreateExcelBuilder().Returns(_builder);
            return betListExcel;
        }
    }
}