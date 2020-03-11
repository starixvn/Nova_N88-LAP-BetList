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
        public async Task GetBetListForSingleCustomerAsync_WhenCall_ParametersPassedCorrectly(string productCode, string productList, bool showBA)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(BetListBySingleCustomerUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReaderAsync().Returns(Reader);
            Reader.Read().Returns(false);
            ObjectDb.GetOutputParams().Returns(new Hashtable());

            var metaModel = new SingleCustomersBetListMetaModel { ProductCodeList = productCode };

            // Action
            await DataAccess.GetBetListForSingleCustomerAsync(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("ShowBA", showBA);
                ObjectDb.Received(1).SetParameterValue("ProductList", productList);
                ObjectDb.Received(1).SetParameterValue("FromDate", metaModel.FromDate);
                ObjectDb.Received(1).SetParameterValue("ToDate", metaModel.ToDate);
                ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("UserName", metaModel.UserName);
                ObjectDb.Received(1).SetParameterValue("StatusIdList", metaModel.Status);
                ObjectDb.Received(1).SetParameterValue("LiveIndicator", metaModel.LiveStatus);
                ObjectDb.Received(1).SetParameterValue("Platform", metaModel.Platform);
                ObjectDb.Received(1).SetParameterValue("MoveBetsDay", BalanceUp.MoveBetsDay);
                ObjectDb.Received(1).SetParameterValue("BalanceUpDay", BalanceUp.BalanceUpDay);
                ObjectDb.Received(1).SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
                ObjectDb.Received(1).SetParameterValue("PageSize", metaModel.PageSize);
                ObjectDb.Received(1).SetParameterValue("PageIndex", metaModel.PageIndex);
                ObjectDb.Received(1).SetParameterValue("TotalRows", null);
                ObjectDb.Received(1).SetParameterValue("SumStake", null);
                ObjectDb.Received(1).SetParameterValue("SumWinLost", null);
                ObjectDb.Received(1).SetParameterValue("SumLicenseeWL", null);
                ObjectDb.Received(1).SetParameterValue("SumComm", null);
                ObjectDb.Received(1).SetParameterValue("SumPlayerComm", null);
            });
        }

        [Test]
        public async Task GetBetListForSingleCustomerAsync_Export_PagingParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(BetListBySingleCustomerUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReaderAsync().Returns(Reader);
            Reader.Read().Returns(false);
            ObjectDb.GetOutputParams().Returns(new Hashtable());

            var metaModel = new SingleCustomersBetListMetaModel { ProductCodeList = string.Empty, IsExport = true };

            // Action
            await DataAccess.GetBetListForSingleCustomerAsync(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("PageSize", int.MaxValue - 1);
                ObjectDb.Received(1).SetParameterValue("PageIndex", 1);
            });
        }

        [Test]
        public async Task GetBetListForSingleCustomerAsync_Sort_SortParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(BetListBySingleCustomerUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReaderAsync().Returns(Reader);
            Reader.Read().Returns(false);
            ObjectDb.GetOutputParams().Returns(new Hashtable());

            var metaModel = new SingleCustomersBetListMetaModel { ProductCodeList = string.Empty, SortBy = "Colum1", SortOrder = "asc" };

            // Action
            await DataAccess.GetBetListForSingleCustomerAsync(metaModel);

            // Assert
            ObjectDb.Received(1).SetParameterValue("Sort", metaModel.SortBy + " " + metaModel.SortOrder);
        }

        [Test]
        public async Task GetBetListForSingleCustomerAsync_WhenCall_ReturnTotalDataCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(BetListBySingleCustomerUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReaderAsync().Returns(Reader);
            Reader.Read().Returns(false);

            var outPut = new Hashtable
            {
                { "sumstake", FakeHelper.Create<decimal>() },
                { "totalrows", FakeHelper.Create<int>() },
                { "sumwinlost", FakeHelper.Create<decimal>() },
                { "sumlicenseewl", FakeHelper.Create<int>() },
                { "sumcomm", FakeHelper.Create<decimal>() },
                { "sumplayercomm", FakeHelper.Create<int>() }
            };
            ObjectDb.GetOutputParams().Returns(outPut);

            var metaModel = new SingleCustomersBetListMetaModel { ProductCodeList = string.Empty };

            // Action
            var result = await DataAccess.GetBetListForSingleCustomerAsync(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.SumStake, Is.EqualTo(outPut["sumstake"]));
                Assert.That(result.TotalRows, Is.EqualTo(outPut["totalrows"]));
                Assert.That(result.SumWinLoss, Is.EqualTo(outPut["sumwinlost"]));
                Assert.That(result.SumLicenseeWL, Is.EqualTo(outPut["sumlicenseewl"]));
                Assert.That(result.SumComm, Is.EqualTo(outPut["sumcomm"]));
                Assert.That(result.SumPlayerComm, Is.EqualTo(outPut["sumplayercomm"]));
            });
        }
    }
}