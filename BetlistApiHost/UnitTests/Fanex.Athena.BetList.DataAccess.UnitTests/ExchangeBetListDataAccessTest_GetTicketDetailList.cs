namespace Fanex.Athena.BetList.DataAccess.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Athena.BetList.DataAccess;
    using Athena.BetList.Model.MetaModel;
    using Common.DataAccess.UnitTests;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class ExchangeBetListDataAccessTest_GetTicketDetailList : BaseDataAccessHelperTest<ExchangeBetListDataAccess>
    {
        private const string TicketDetailUsp = "Lrf_BetList_Exchange_Detail";

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            DataAccess.GetObjectDb(Arg.Is(TicketDetailUsp)).Returns(ObjectDb);
        }

        [Test]
        public async Task GetTicketDetailList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var metaModel = FakeHelper.Create<ExchangeBetListDetailMetaModel>();

            // Action
            await DataAccess.GetTicketDetailListAsync(metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("CustId", metaModel.CustId);
                ObjectDb.Received(1).SetParameterValue("BetID", metaModel.BetId);
                ObjectDb.Received(1).SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
            });
        }

        [Test]
        public async Task GetTicketDetailList_NoData_ReturnEmptyResult()
        {
            // Arrange
            Reader.Read().Returns(false);

            // Action
            var result = await DataAccess.GetTicketDetailListAsync(new ExchangeBetListDetailMetaModel()).ConfigureAwait(false);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetTicketDetailList_HasData_ReturnCorrectResult()
        {
            // Arrange
            const string BaseCurrency = "usd";

            MapReaderData(new Dictionary<string, object>
            {
                { "BetTaken", "2018/02/02" },
                { "OddsRequest", 2 },
                { "Stake", 3 },
                { "OddsMatched", 4 },
            });

            Reader.Read().Returns(true, false);

            var metaModel = new ExchangeBetListDetailMetaModel
            {
                BaseCurrency = BaseCurrency
            };

            // Action
            var result = await DataAccess.GetTicketDetailListAsync(metaModel).ConfigureAwait(false);
            var firstItem = result.ElementAt(0);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstItem.BetTaken, Is.EqualTo(new DateTime(2018, 2, 2)));
                Assert.That(firstItem.OddsReq, Is.EqualTo(2));
                Assert.That(firstItem.Stake, Is.EqualTo(3));
                Assert.That(firstItem.OddsMatched, Is.EqualTo(4));
            });
        }

        [TestCase("Lay", "9.00")]
        [TestCase("WTH", "-")]
        public async Task GetTicketDetailList_HasData_ReturnCorrectLiability(
            string exchangeMatchType,
            string expectedLiability)
        {
            // Arrange
            MapReaderData(new Dictionary<string, object>
            {
                { "Stake", 3 },
                { "OddsMatched", 4 },
            });

            Reader.Read().Returns(true, false);

            var metaModel = new ExchangeBetListDetailMetaModel
            {
                Type = exchangeMatchType
            };

            // Action
            var result = await DataAccess.GetTicketDetailListAsync(metaModel).ConfigureAwait(false);

            // Assert
            Assert.That(result.ElementAt(0).Liability, Is.EqualTo(expectedLiability));
        }
    }
}