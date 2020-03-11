using Fanex.Athena.BetList.Excel.Builders;
using Fanex.Athena.BetList.Excel.Model;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.Resources;
using Fanex.Athena.UnitTest.Common.Helpers;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.BetList.Excel.UnitTests
{
    [TestFixture]
    internal class BettingTicketCenterExcelTest
    {
        private IBettingTicketCenterExcelBuilder _builder;
        private BettingTicketCenterExcel _excel;

        [SetUp]
        public void Setup()
        {
            _builder = Substitute.For<IBettingTicketCenterExcelBuilder>();
            _excel = Substitute.ForPartsOf<BettingTicketCenterExcel>(_builder);
        }

        [Test]
        public void Export_HasData_CallRelevantMethods()
        {
            // Arrange
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.Add(new LrfTicket());
            var model = new BettingTicketCenterExcelModel
            {
                Data = ticketResult
            };
            var workbook = Substitute.For<IWorkbook>();
            _builder.GetWorkbook().Returns(workbook);
            _excel.When(x => x.BuildSheets(Arg.Any<BettingTicketCenterExcelModel>())).DoNotCallBase();

            // Action
            var result = _excel.Export(model);

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(1).InitializeFonts();
                _builder.Received(1).InitializeCellStyles();
                _excel.Received(1).BuildSheets(model);
                Assert.That(result, Is.SameAs(workbook));
            });
        }

        [Test]
        public void Export_NoData_CallRelevantMethods()
        {
            // Arrange
            var model = new BettingTicketCenterExcelModel
            {
                Title = FakeHelper.Create<string>(),
                Data = new TicketResult(),
                Option = FakeHelper.Create<BettingTicketCenterOption>()
            };
            var workbook = Substitute.For<IWorkbook>();
            _builder.GetWorkbook().Returns(workbook);
            _excel.When(x => x.BuildSheets(Arg.Any<BettingTicketCenterExcelModel>())).DoNotCallBase();

            // Action
            var result = _excel.Export(model);

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(1).CreateSheet(ViewResources.BettingTicketCenter, model.Option);
                _builder.Received(1).AddTitle(model.Title, model.Option);
                _builder.Received(1).AddHeader(model.Option);
                _builder.Received(1).AddNoInfoRow(model.Option);
                Assert.That(result, Is.SameAs(workbook));
            });
        }

        [Test]
        public void BuildSheets_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.Add(new LrfTicket());
            var model = new BettingTicketCenterExcelModel
            {
                Title = FakeHelper.Create<string>(),
                Data = ticketResult,
                Option = FakeHelper.Create<BettingTicketCenterOption>()
            };

            _excel.When(x => x.AssignSheetData(Arg.Any<BettingTicketCenterExcelModel>(), Arg.Any<int>(), Arg.Any<int>())).DoNotCallBase();
            _excel.When(x => x.AddFooter(Arg.Any<BettingTicketCenterExcelModel>())).DoNotCallBase();

            // Action
            _excel.BuildSheets(model);

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(1).CreateSheet(ViewResources.Page + "1", model.Option);
                _builder.Received(1).AddTitle(model.Title, model.Option);
                _builder.Received(1).AddHeader(model.Option);
                _excel.Received(1).AssignSheetData(model, 0, 1);
                _builder.Received(1).GetWorkbook();
            });
        }

        [TestCase(false)]
        [TestCase(true)]
        public void BuildSheets_ShowTotal_CallAddFooter(bool isShowTotal)
        {
            // Arrange
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.Add(new LrfTicket());
            var model = new BettingTicketCenterExcelModel
            {
                Option = new BettingTicketCenterOption
                {
                    ShowTotal = isShowTotal
                },
                Title = FakeHelper.Create<string>(),
                Data = ticketResult
            };

            _excel.When(x => x.AssignSheetData(Arg.Any<BettingTicketCenterExcelModel>(), Arg.Any<int>(), Arg.Any<int>())).DoNotCallBase();
            _excel.When(x => x.AddFooter(Arg.Any<BettingTicketCenterExcelModel>())).DoNotCallBase();

            // Action
            _excel.BuildSheets(model);

            // Assert
            if (isShowTotal)
            {
                _excel.Received(1).AddFooter(model);
            }
            else
            {
                _excel.DidNotReceive().AddFooter(model);
            }
        }

        [Test]
        public void AssignSheetData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.Add(new LrfTicket());
            var model = new BettingTicketCenterExcelModel
            {
                Data = ticketResult,
                LicenseeId = FakeHelper.Create<int>(),
                Option = FakeHelper.Create<BettingTicketCenterOption>()
            };

            // Action
            _excel.AssignSheetData(model, 0, 1);

            // Assert
            _builder.Received(1).AssignRowData(model.Data, model.Data.MainTicketList[0], model.LicenseeId, model.Option);
        }

        [Test]
        public void AddFooter_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const decimal SumPlayerComm = 0.12m;
            const decimal SumComm = 0.34m;
            var sumStake = FakeHelper.Create<decimal>();
            var sumWL = FakeHelper.Create<decimal>();
            var sumLicenseeWL = FakeHelper.Create<decimal>();
            var model = new BettingTicketCenterExcelModel
            {
                Option = FakeHelper.Create<BettingTicketCenterOption>(),
                Data = new TicketResult
                {
                    SumStake = sumStake,
                    SumWinLoss = sumWL,
                    SumPlayerComm = SumPlayerComm,
                    SumLicenseeWL = sumLicenseeWL,
                    SumComm = SumComm,
                },
                TotalText = FakeHelper.Create<string>()
            };

            // Action
            _excel.AddFooter(model);

            // Assert
            _builder.Received(1).AddFooter(sumStake, sumWL, SumPlayerComm, sumLicenseeWL + SumComm, model.TotalText, model.Option);
        }
    }
}