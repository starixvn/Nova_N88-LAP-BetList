namespace Fanex.Athena.BetList.DataAccess.UnitTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Athena.BetList.DataAccess;
    using Athena.BetList.Model.DTO;
    using Athena.BetList.Model.MetaModel;
    using Common.Constant;
    using Common.DataAccess.UnitTests;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class ExchangeBetListDataAccessTest : BaseDataAccessHelperTest<ExchangeBetListDataAccess>
    {
        private const string BetListUsp = "Lrf_BetList_Exchange_Mem";

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            DataAccess.GetObjectDb(Arg.Is(BetListUsp)).Returns(ObjectDb);
        }

        [Test]
        public async Task GetBetListData_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var metaModel = FakeHelper.Create<ExchangeBetListMetaModel>();

            // Action
            await DataAccess.GetBetListDataAsync(metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("CustId", metaModel.CustId);
                ObjectDb.Received(1).SetParameterValue("FromDate", metaModel.FromDate);
                ObjectDb.Received(1).SetParameterValue("ToDate", metaModel.ToDate);
                ObjectDb.Received(1).SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
                ObjectDb.Received(1).SetParameterValue("SumWinLost", null);
            });
        }

        [Test]
        public async Task GetBetListData_NoData_ReturnEmptyResult()
        {
            // Arrange
            Reader.Read().Returns(false);
            ObjectDb.GetParameterValue("SumWinLost").Returns(0);

            // Action
            var result = await DataAccess.GetBetListDataAsync(new ExchangeBetListMetaModel()).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.SumWinLossList, Is.Empty);
                Assert.That(result.TicketList, Is.Empty);

                Assert.That(result.GrandTotal, Is.Zero);
            });
        }

        [Test]
        public async Task GetBetListData_HasSumWinLossListData_ReturnCorrectResult()
        {
            // Arrange
            var sumWL = FakeHelper.Create<decimal>();
            ObjectDb.GetParameterValue("SumWinLost").Returns(sumWL);

            var ticket = FakeHelper.Create<ExchangeSumWinLoss>();

            MapReaderData(ticket);

            Reader.Read().Returns(true, false, false);
            Reader.NextResult().Returns(true);

            var metaModel = new ExchangeBetListMetaModel();

            // Action
            var result = await DataAccess.GetBetListDataAsync(metaModel).ConfigureAwait(false);
            var firstSumWL = result.SumWinLossList.ElementAt(0);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstSumWL.UserId, Is.EqualTo(ticket.UserId));
                Assert.That(firstSumWL.EventId, Is.EqualTo(ticket.EventId));
                Assert.That(firstSumWL.EventName, Is.EqualTo(ticket.EventName));
                Assert.That(firstSumWL.EventTypeName, Is.EqualTo(ticket.EventTypeName));
                Assert.That(firstSumWL.MarketId, Is.EqualTo(ticket.MarketId));
                Assert.That(firstSumWL.MarketName, Is.EqualTo(ticket.MarketName));
                Assert.That(firstSumWL.WinLost, Is.EqualTo(ticket.WinLost));
                Assert.That(firstSumWL.Commission, Is.EqualTo(ticket.Commission));

                Assert.That(result.GrandTotal, Is.EqualTo(sumWL));
            });
        }

        [Test]
        public async Task GetBetListData_HasTicketDataData_ReturnCorrectResult()
        {
            // Arrange
            var ticket = FakeHelper.Create<ExchangeTicket>();
            ticket.BetPlaced = FakeHelper.TruncateMiliseconds(ticket.BetPlaced);
            MapReaderData(ticket);

            Reader.Read().Returns(false, true, false);
            Reader.NextResult().Returns(true);

            var metaModel = new ExchangeBetListMetaModel();

            // Action
            var result = (await DataAccess.GetBetListDataAsync(metaModel).ConfigureAwait(false)).TicketList.ElementAt(0);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.UserId, Is.EqualTo(ticket.UserId));
                Assert.That(result.EventId, Is.EqualTo(ticket.EventId));
                Assert.That(result.MarketId, Is.EqualTo(ticket.MarketId));
                Assert.That(result.BetId, Is.EqualTo(ticket.BetId));
                Assert.That(result.SelectionName, Is.EqualTo(ticket.SelectionName));
                Assert.That(result.OddsAvg, Is.EqualTo(ticket.OddsAvg));
                Assert.That(result.WinLost, Is.EqualTo(ticket.WinLost));
                Assert.That(result.Stake, Is.EqualTo(ticket.Stake));
                Assert.That(result.BetPlaced, Is.EqualTo(ticket.BetPlaced));
                Assert.That(result.BetPlacedDateString, Is.EqualTo(ticket.BetPlaced.ToString(FormatKey.DateTimeFormat)));
            });
        }
    }
}