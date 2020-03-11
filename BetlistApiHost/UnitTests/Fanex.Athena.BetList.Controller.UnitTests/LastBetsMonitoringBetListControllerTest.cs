namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System.Threading.Tasks;
    using Common.Utility.Collections;
    using Fanex.BetList.Core.Entities;
    using Model.DTO;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Interfaces;
    using UnitTest.Common.Extensions;
    using UnitTest.Common.Helpers;

    public class LastBetsMonitoringBetListControllerTest : BaseControllerHelperTest
    {
        private ILastBetsMonitoringBetListService _lastBetsMonitoringBetListService;

        private ITicketHelper _ticketHelper;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            _lastBetsMonitoringBetListService = Substitute.For<ILastBetsMonitoringBetListService>();

            _ticketHelper = Substitute.For<ITicketHelper>();
        }

        [Test]
        public async Task GetData_NoData_ReturnNoNewData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<LastBetsMonitoringMetaModel>();

                _lastBetsMonitoringBetListService.GetLastBetsMonitoringData(metaModel).Returns(Task.FromResult(new TicketResult()));

                // Action
                var result = (await controller.GetData(metaModel)).Data;

                // Assert
                Assert.That(result.GetValue("NewRow"), Is.False);
            }
        }

        [Test]
        public async Task GetData_HasData_ReturnCorrectData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<LastBetsMonitoringMetaModel>();

                var ticket = FakeHelper.Create<LrfTicket>();
                ticket.BetTypeId = 1;
                var ticketResult = new TicketResult { MainTicketList = new SortableList<LrfTicket> { ticket } };
                _lastBetsMonitoringBetListService.GetLastBetsMonitoringData(metaModel).Returns(Task.FromResult(ticketResult));

                // Action
                var result = (await controller.GetData(metaModel)).Data;

                // Assert
                Assert.Multiple(() => 
                {
                    AssertClearData(ticketResult);
                    Assert.That(result.GetValue("NewRow"), Is.True);
                    Assert.That(result.GetValue("Rows"), Is.Not.Null);
                    Assert.That(result.GetValue("LastTransId"), Is.EqualTo(ticket.TransId));
                });
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(12)]
        [TestCase(20)]
        public async Task GetData_BuildOdds_DecimalOddsBetType_BuildCorrectOddString(int betType)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<LastBetsMonitoringMetaModel>();

                var ticket = FakeHelper.Create<LrfTicket>();
                ticket.BetTypeId = betType;
                var ticketResult = new TicketResult { MainTicketList = new SortableList<LrfTicket> { ticket } };
                _lastBetsMonitoringBetListService.GetLastBetsMonitoringData(metaModel).Returns(Task.FromResult(ticketResult));

                // Action
                var result = (await controller.GetData(metaModel)).Data;
                var data = (string)result.GetValue("Rows");

                // Assert
                Assert.That(data.Contains("underdog"), Is.True);
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(12)]
        [TestCase(20)]
        public async Task GetData_BuildOdds_NegativeDecimalOddsBetType_BuildCorrectOddString(int betType)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<LastBetsMonitoringMetaModel>();

                var ticket = FakeHelper.Create<LrfTicket>();
                ticket.BetTypeId = betType;
                ticket.Odds = -FakeHelper.Create<uint>();
                var ticketResult = new TicketResult { MainTicketList = new SortableList<LrfTicket> { ticket } };
                _lastBetsMonitoringBetListService.GetLastBetsMonitoringData(metaModel).Returns(Task.FromResult(ticketResult));

                // Action
                var result = (await controller.GetData(metaModel)).Data;
                var data = (string)result.GetValue("Rows");

                // Assert
                Assert.That(data.Contains("underdog negative"), Is.True);
            }
        }

        private LastBetMonitoringBetListController CreateController()
        {
            return new LastBetMonitoringBetListController(
                 _lastBetsMonitoringBetListService,
                 _ticketHelper);
        }
    }
}