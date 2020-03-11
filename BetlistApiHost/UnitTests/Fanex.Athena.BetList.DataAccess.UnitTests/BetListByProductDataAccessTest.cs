namespace Fanex.Athena.BetList.DataAccess.UnitTests
{
    using Athena.BetList.DataAccess;
    using Athena.BetList.Model.MetaModel;
    using Common.DataAccess.UnitTests;
    using Fanex.Athena.Common.DataAccess.Interfaces;
    using Fanex.Athena.Common.Model.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections;
    using System.Threading.Tasks;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class BetListByProductDataAccessTest : BaseDataAccessHelperTest<BetListByProductDataAccess>
    {
        private IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly BalanceUp _balanceUpInfo = new BalanceUp
        {
            BalanceUpDay = DateTime.Now,
            MoveBetsDay = DateTime.Now
        };

        protected override void SetUpDataAccess()
        {
            _balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _balanceUpDataAccess.GetBalanceUp().Returns(_balanceUpInfo);
            DataAccess = Substitute.ForPartsOf<BetListByProductDataAccess>(_balanceUpDataAccess,
                CurrencyDataAccess);
        }

        [Test]
        public async Task QueryAsync_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_Statement_Mem";

            DataAccess.GetObjectDb(Arg.Is(UspKey)).Returns(ObjectDb);
            var metaModel = FakeHelper.Create<BetListByProductMetaModel>();

            // Action
            await DataAccess.QueryAsync(metaModel).ConfigureAwait(false);

            // Assert
            ObjectDb.Received(1).SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
            ObjectDb.Received(1).SetParameterValue("PageSize", metaModel.PageSize);
            ObjectDb.Received(1).SetParameterValue("PageIndex", metaModel.PageIndex);
            ObjectDb.Received(1).SetParameterValue("CustId", metaModel.CustId);
            ObjectDb.Received(1).SetParameterValue("@Date", metaModel.WinlostDate.Date);
            ObjectDb.Received(1).SetParameterValue("@TransType", metaModel.TransType);
            ObjectDb.Received(1).SetParameterValue("@ProductCode", metaModel.ProductCode);
            ObjectDb.Received(1).SetParameterValue("@AgentList", metaModel.AgentListXml);
            ObjectDb.Received(1).SetParameterValue("@Sort", metaModel.Sort);
            ObjectDb.Received(1).SetParameterValue("@BalanceUpDay", _balanceUpInfo.BalanceUpDay);
            ObjectDb.Received(1).SetParameterValue("@MoveBetsDay", _balanceUpInfo.MoveBetsDay);

            ObjectDb.Received(1).SetParameterValue("@Sort", metaModel.Sort);

            ObjectDb.Received(1).SetParameterValue("@TotalRows", null);
            ObjectDb.Received(1).SetParameterValue("@SumStake", null);
            ObjectDb.Received(1).SetParameterValue("@SumWinLost", null);
            ObjectDb.Received(1).SetParameterValue("@SumLicenseeWL", null);
            ObjectDb.Received(1).SetParameterValue("@SumComm", null);
            ObjectDb.Received(1).SetParameterValue("@SumPlayerComm", null);
        }

        [Test]
        public async Task QueryAsync_WhenCall_CallReadBetListTicket()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_Statement_Mem";

            DataAccess.GetObjectDb(Arg.Is(UspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReaderAsync().Returns(Reader);
            Reader.Read().Returns(false);
            var metaModel = FakeHelper.Create<BetListByProductMetaModel>();

            // Action
            await DataAccess.QueryAsync(metaModel).ConfigureAwait(false);

            // Assert
            DataAccess.Received(1).ReadBetListTicket(Reader, UserCurrencyList, 0);
        }

        [Test]
        public async Task QueryAsync_WhenCall_ReadCorrectOutPutResult()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_Statement_Mem";

            DataAccess.GetObjectDb(Arg.Is(UspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReaderAsync().Returns(Reader);
            Reader.Read().Returns(false);
            var outPut = new Hashtable
            {
                { "totalrows", FakeHelper.Create<int>() },
                { "SumStake", FakeHelper.Create<int>() },
                { "SumWinLost", FakeHelper.Create<int>() },
                { "SumLicenseeWL", FakeHelper.Create<int>() },
                { "SumComm", FakeHelper.Create<int>() },
                { "SumPlayerComm", FakeHelper.Create<int>() }
            };

            ObjectDb.GetOutputParams().Returns(outPut);

            var metaModel = FakeHelper.Create<BetListByProductMetaModel>();

            // Action
            var result = await DataAccess.QueryAsync(metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.TotalRows, Is.EqualTo(outPut["totalrows"]));
                Assert.That(result.SumStake, Is.EqualTo(outPut["SumStake"]));
                Assert.That(result.SumWinLoss, Is.EqualTo(outPut["SumWinLost"]));
                Assert.That(result.SumLicenseeWL, Is.EqualTo(outPut["SumLicenseeWL"]));
                Assert.That(result.SumComm, Is.EqualTo(outPut["SumComm"]));
                Assert.That(result.SumPlayerComm, Is.EqualTo(outPut["SumPlayerComm"]));
            });
        }
    }
}