namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Utility.Helpers;
    using Model.DTO;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Interfaces;
    using UnitTest.Common.Extensions;
    using UnitTest.Common.Helpers;

    public class LiveReportBetListControllerTest : BaseControllerHelperTest
    {
        private const string AgentListXml = "<xml/>";

        private IBetListService _betListService;
        private ITicketBuilderService _ticketBuilderService;

        [SetUp]
        public void SetUp()
        {
            _betListService = Substitute.For<IBetListService>();
            _ticketBuilderService = Substitute.For<ITicketBuilderService>();
        }

        [TearDown]
        public void TearDown()
        {
            _betListService = Substitute.For<IBetListService>();
            _ticketBuilderService = null;
        }

        [Test]
        public async Task GetData_WhenCall_ReturnCorrectData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<LiveReportBetListMetaModel>();

                var ticketResult = new TicketResult
                {
                    SumWinLoss = FakeHelper.Create<decimal>(),
                    SumPlayerComm = FakeHelper.Create<decimal>(),
                    SumLicenseeWL = FakeHelper.Create<decimal>(),
                    SumComm = FakeHelper.Create<decimal>(),
                };
                _betListService.GetBetListData(metaModel).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2);
                _ticketBuilderService.BuildTicketText(ticketResult).Returns(ticketText.ToList());

                // Action
                var result = (await controller.GetData(metaModel)).Data;
                var main = result.GetValue("Main");
                var total = result.GetValue("Total");

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(main, Is.EqualTo(ticketText));
                    Assert.That(total.GetValue("SumWinLoss"), Is.EqualTo(ticketResult.SumWinLoss + ticketResult.SumPlayerComm));
                    Assert.That(total.GetValue("SumLicenseeWl"), Is.EqualTo(ticketResult.SumLicenseeWL + ticketResult.SumComm));
                    AssertClearData(ticketResult);
                });
            }
        }

        [TestCase(1, 20, 50, 1, 3)]
        [TestCase(2, 30, 60, 31, 2)]
        [TestCase(3, 10, 60, 21, 6)]
        public async Task GetPagingData_WhenCall_ReturnCorrectData(int pageIndex, int pageSize, int totalRows, int startIndex, int totalPage)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<LiveReportBetListMetaModel>();
                metaModel.PageIndex = pageIndex;
                metaModel.PageSize = pageSize;
                metaModel.TotalRows = totalRows;

                var ticketResult = new TicketResult();
                _betListService.GetBetListData(metaModel).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2);
                _ticketBuilderService.BuildTicketText(ticketResult, metaModel, startIndex).Returns(ticketText.ToList());

                // Action
                var result = (await controller.GetPagingData(metaModel)).Data;
                var page = result.GetValue("page");
                var total = result.GetValue("total");
                var records = result.GetValue("records");
                var rows = result.GetValue("rows");
                var isHasCustComm = result.GetValue("IsHasCustComm");

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(page, Is.EqualTo(pageIndex));
                    Assert.That(total, Is.EqualTo(totalPage));
                    Assert.That(records, Is.EqualTo(totalRows));
                    Assert.That(rows, Is.EqualTo(ticketText));
                    Assert.That(isHasCustComm, Is.EqualTo(metaModel.HasPlayerCom));
                    AssertClearData(ticketResult);
                });
            }
        }

        [TestCase(1.123, 2.345, "3.47")]
        [TestCase(1.123, 2.34, "3.46")]
        [TestCase(-1.123, -2.345, "<span class='negative-number'>-3.47</span>")]
        public async Task GetPagingData_WhenCall_ReturnCorrectLicenseeWLText(decimal sumLicenseeWL, decimal sumComm, string expectedLicenseeWLText)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<LiveReportBetListMetaModel>();
                metaModel.SumLicenseeWL = sumLicenseeWL;
                metaModel.SumComm = sumComm;

                var ticketResult = new TicketResult();
                _betListService.GetBetListData(metaModel).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2);
                _ticketBuilderService.BuildTicketText(ticketResult, metaModel, Arg.Any<int>()).Returns(ticketText.ToList());

                var expectedWinloss = NumberHelper.DecFormat(metaModel.SumWinlost + metaModel.SumPlayerComm, 2);

                // Action
                var result = (await controller.GetPagingData(metaModel)).Data;
                var userdata = result.GetValue("userdata");

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(userdata.GetValue("WinLoss"), Is.EqualTo(expectedWinloss));
                    Assert.That(userdata.GetValue("LicenseeWL"), Is.EqualTo(expectedLicenseeWLText));
                });
            }
        }

        private LiveReportBetListController CreateController()
        {
            return new LiveReportBetListController(_betListService, _ticketBuilderService);
        }
    }
}