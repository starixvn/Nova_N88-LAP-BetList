namespace Fanex.Athena.BetList.DataAccess.UnitTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Athena.BetList.DataAccess;
    using Athena.BetList.Model.MetaModel;
    using Common.DataAccess.UnitTests;
    using Fanex.BetList.Core.Entities;
    using NSubstitute;
    using NUnit.Framework;

    public class ColossusBetBetListDataAccessTest : BaseDataAccessHelperTest<ColossusBetBetListDataAccess>
    {
        private const string GetSiteIdSp = "Acc_CustomerSelSiteByCustId";
        private const string GetDataSp = "Lrf_BetList_CB_Detail";
        private const int SoldBetType = 18002;

        protected override void SetUpDataAccess()
        {
            var ticketHelper = Substitute.For<ITicketHelper>();
            DataAccess = Substitute.ForPartsOf<ColossusBetBetListDataAccess>(
                ticketHelper);
        }

        [Test]
        public void GetSiteId_WhenCall_DataReturnCorrectly()
        {
            // Arrange
            const int CustId = 999;
            const int ExpectedSiteId = 100;
            DataAccess.GetObjectDb(Arg.Is(GetSiteIdSp)).Returns(ObjectDb);
            ObjectDb.GetParameterValue("siteid").Returns(ExpectedSiteId);

            // Action
            var result = DataAccess.GetSiteId(CustId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("custid", CustId);
                ObjectDb.Received(1).SetParameterValue("siteid", null);

                Assert.That(result, Is.EqualTo(ExpectedSiteId));
            });
        }

        [TestCase(true, 1)]
        [TestCase(false, 0)]
        public async Task GetData_WhenCall_DataReturnCorrectly(bool isSoldBetType, decimal expectedTotalSoldWinLoss)
        {
            // Arrange
            var metaModel = new ColossusBetDetailMetaModel
            {
                BaseCurrency = "USD",
                AgentListXml = "<xml>",
                CustId = 999,
                RefNo = 1000
            };
            DataAccess.GetObjectDb(Arg.Is(GetDataSp)).Returns(ObjectDb);
            Reader.Read().Returns(true, false);

            var readerDataMapping = new
            {
                TransDate = "1/1/2017",
                Comm = 123,
                BetType = isSoldBetType ? SoldBetType : 0,
                Winlost = isSoldBetType ? expectedTotalSoldWinLoss : 1234
            };
            MapReaderData(readerDataMapping);

            // Action
            var result = await DataAccess.GetData(metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count(), Is.EqualTo(1));
                Assert.That(result.DataList.ElementAt(0).TransDate, Is.EqualTo(new DateTime(2017, 1, 1)));
                Assert.That(result.DataList.ElementAt(0).Commission, Is.EqualTo(123));
                Assert.That(result.TotalComm, Is.EqualTo(123));
                Assert.That(result.TotalSoldWinLoss, Is.EqualTo(expectedTotalSoldWinLoss));
            });
        }
    }
}