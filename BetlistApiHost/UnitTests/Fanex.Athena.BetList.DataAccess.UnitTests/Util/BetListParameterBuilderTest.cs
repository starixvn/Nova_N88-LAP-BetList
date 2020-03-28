namespace Fanex.Athena.BetList.DataAccess.UnitTests.Util
{
    using Athena.BetList.DataAccess.Util;
    using Athena.BetList.Model.MetaModel;
    using Data;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class BetListParameterBuilderTest
    {
        private BetListMetaModel _metaModel;
        private IObjectDb _objectDb;

        [SetUp]
        public void SetUp()
        {
            _metaModel = FakeHelper.Create<BetListMetaModel>();
            _metaModel.ProductCodeList = "SB,NG";

            _objectDb = Substitute.For<IObjectDb>();
        }

        [TearDown]
        public void TearDown()
        {
            _metaModel = null;
            _objectDb = null;
        }

        [Test]
        public void CreateBetListParameters_CalledByCustomerWlBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelWinLossByDate_Mem";
            _metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                _objectDb.Received(1).SetParameterValue("CustId", _metaModel.CustId);
                AssertMoveBetParams(_objectDb, _metaModel);
                AssertDateRangeParameters(_objectDb, _metaModel);
                AssertPagingParameters(_objectDb, _metaModel);
                AssertSummaryParameters(_objectDb);
                _objectDb.Received(1).SetParameterValue("showSB", true);
                _objectDb.Received(1).SetParameterValue("showNG", true);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByLicenseeWLBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelWinloss_Mem";
            _metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                _objectDb.Received(1).SetParameterValue("CustId", _metaModel.CustId);
                AssertDateRangeParameters(_objectDb, _metaModel);
                AssertMoveBetParams(_objectDb, _metaModel);
                _objectDb.Received(1).SetParameterValue("showSB", true);
                _objectDb.Received(1).SetParameterValue("showNG", true);
                AssertPagingParameters(_objectDb, _metaModel);
                AssertSummaryParameters(_objectDb);
                _objectDb.Received(1).SetParameterValue("drawfilter", _metaModel.DrawFilter);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByMicroGamingRngDetail_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_RNG_Detail";
            _metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, _metaModel);

            // Assert
            AssertCasinoDetailParameters(_objectDb, _metaModel);
        }

        [Test]
        public void CreateBetListParameters_CalledByAGCasinoDetail_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_LC_Detail";
            _metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, _metaModel);

            // Assert
            AssertCasinoDetailParameters(_objectDb, _metaModel);
        }

        [Test]
        public void CreateBetListParameters_CalledByBonusDetail_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_LicenseeBonus_Detail";
            _metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                _objectDb.Received(1).SetParameterValue("TransId", _metaModel.TransId);
                _objectDb.Received(1).SetParameterValue("RefCode", _metaModel.RefCode);
                _objectDb.Received(1).SetParameterValue("BaseCurrency", _metaModel.BaseCurrency);
            });
        }

        private static void AssertSummaryParameters(IObjectDb objectDb)
        {
            var summaryParams = new[] { "SumWinlost", "SumLicenseeWL", "SumComm", "SumPlayerComm" };

            foreach (var param in summaryParams)
            {
                objectDb.Received(1).SetParameterValue(param, null);
            }
        }

        private static void AssertPagingParameters(IObjectDb objectDb, BetListMetaModel metaModel)
        {
            objectDb.Received(1).SetParameterValue("PageSize", metaModel.PageSize);
            objectDb.Received(1).SetParameterValue("PageIndex", metaModel.PageIndex);
            objectDb.Received(1).SetParameterValue("totalrows", null);
        }

        private static void AssertDateRangeParameters(IObjectDb objectDb, BetListMetaModel metaModel)
        {
            objectDb.Received(1).SetParameterValue("FromDate", metaModel.FromDate);
            objectDb.Received(1).SetParameterValue("ToDate", metaModel.ToDate);
        }

        private static void AssertMoveBetParams(IObjectDb objectDb, BetListMetaModel metaModel)
        {
            objectDb.Received(1).SetParameterValue("MoveBetsDay", metaModel.MoveBetsDay);
            objectDb.Received(1).SetParameterValue("BalanceUpDay", metaModel.BalanceUpDay);
        }

        private static void AssertCasinoDetailParameters(IObjectDb objectDb, BetListMetaModel metaModel)
        {
            objectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
            objectDb.Received(1).SetParameterValue("refno", metaModel.TransId);
            objectDb.Received(1).SetParameterValue("CustId", metaModel.CustId);
            objectDb.Received(1).SetParameterValue("transtype", metaModel.TransType);
        }
    }
}