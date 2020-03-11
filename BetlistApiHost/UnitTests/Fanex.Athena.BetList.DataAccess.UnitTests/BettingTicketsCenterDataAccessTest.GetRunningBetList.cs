namespace Fanex.Athena.BetList.DataAccess.UnitTests
{
    using System.Collections;
    using System.Threading.Tasks;
    using Athena.BetList.Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public partial class BettingTicketsCenterDataAccessTest
    {
        [TestCase("", "", false)]
        [TestCase("P1,SB,P2", "P1,SB,P2", false)]
        [TestCase("BA,P1,P2", "P1,P2", true)]
        [TestCase("P1,BA,P2,SB", "P1,P2,SB", true)]
        public async Task GetRunningBetListAsync_WhenCall_ParametersPassedCorrectly(string productCode, string productList, bool showBA)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(RunningBetListUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReaderAsync().Returns(Reader);
            Reader.Read().Returns(false);
            ObjectDb.GetOutputParams().Returns(new Hashtable());

            var metaModel = new BaseBettingTicketMetaModel { ProductCodeList = productCode };

            // Action
            await DataAccess.GetRunningBetListAsync(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("UserName", metaModel.UserName);
                ObjectDb.Received(1).SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
                ObjectDb.Received(1).SetParameterValue("ProductList", productList);
                ObjectDb.Received(1).SetParameterValue("showBA", showBA);
                ObjectDb.Received(1).SetParameterValue("PageSize", metaModel.PageSize);
                ObjectDb.Received(1).SetParameterValue("PageIndex", metaModel.PageIndex);
                ObjectDb.Received(1).SetParameterValue("TotalRows", null);
                ObjectDb.Received(1).SetParameterValue("SumStake", null);
            });
        }

        [Test]
        public async Task GetRunningBetListAsync_Export_PagingParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(RunningBetListUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReaderAsync().Returns(Reader);
            Reader.Read().Returns(false);
            ObjectDb.GetOutputParams().Returns(new Hashtable());

            var metaModel = new BaseBettingTicketMetaModel { ProductCodeList = string.Empty, IsExport = true };

            // Action
            await DataAccess.GetRunningBetListAsync(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("PageSize", int.MaxValue - 1);
                ObjectDb.Received(1).SetParameterValue("PageIndex", 1);
            });
        }

        [Test]
        public async Task GetRunningBetListAsync_Sort_SortParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(RunningBetListUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReaderAsync().Returns(Reader);
            Reader.Read().Returns(false);
            ObjectDb.GetOutputParams().Returns(new Hashtable());

            var metaModel = new BaseBettingTicketMetaModel { ProductCodeList = string.Empty, SortBy = "Colum1", SortOrder = "asc" };

            // Action
            await DataAccess.GetRunningBetListAsync(metaModel);

            // Assert
            ObjectDb.Received(1).SetParameterValue("Sort", metaModel.SortBy + " " + metaModel.SortOrder);
        }

        [Test]
        public async Task GetRunningBetListAsync_WhenCall_ReturnTotalDataCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(RunningBetListUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReaderAsync().Returns(Reader);
            Reader.Read().Returns(false);

            var outPut = new Hashtable
            {
                { "sumstake", FakeHelper.Create<decimal>() },
                { "totalrows", FakeHelper.Create<int>() }
            };
            ObjectDb.GetOutputParams().Returns(outPut);

            var metaModel = new BaseBettingTicketMetaModel { ProductCodeList = string.Empty };

            // Action
            var result = await DataAccess.GetRunningBetListAsync(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.SumStake, Is.EqualTo(outPut["sumstake"]));
                Assert.That(result.TotalRows, Is.EqualTo(outPut["totalrows"]));
            });
        }
    }
}