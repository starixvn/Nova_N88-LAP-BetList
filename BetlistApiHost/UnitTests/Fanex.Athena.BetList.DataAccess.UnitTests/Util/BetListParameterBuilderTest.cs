namespace Fanex.Athena.BetList.DataAccess.UnitTests.Util
{
    using System.Collections.Generic;
    using System.Linq;
    using Athena.BetList.DataAccess.Util;
    using Athena.BetList.Model.MetaModel;
    using Common.Constant;
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
        public void CreateBetListParameters_CalledByWinlossByLeague_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_WinlostByLeague_Mem";
            var metaModel = FakeHelper.Create<WinLossByLeagueBetListMetaModel>(x => x.ProductCodeList, "SB,NG");
            metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
                _objectDb.Received(1).SetParameterValue("CustId", metaModel.CustId);
                _objectDb.Received(1).SetParameterValue("LeagueID", metaModel.LeagueId);
                _objectDb.Received(1).SetParameterValue("MatchId", metaModel.MatchId);
                _objectDb.Received(1).SetParameterValue("Odds", metaModel.Odds);
                AssertDateRangeParameters(_objectDb, metaModel);
                AssertMoveBetParams(_objectDb, metaModel);
                _objectDb.Received(1).SetParameterValue("ShowWin", metaModel.ShowWin);
                _objectDb.Received(1).SetParameterValue("ShowLoss", metaModel.ShowLoss);
                _objectDb.Received(1).SetParameterValue("ShowDraw", metaModel.ShowDraw);
                _objectDb.Received(1).SetParameterValue("ShowSB", true);
                _objectDb.Received(1).SetParameterValue("ShowBA", false);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByCorectScoreBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_TotalBets_Sel430";
            _metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                AssertPagingParameters(_objectDb, _metaModel);
            });
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
        public void CreateBetListParameters_CalledByRunningBetListByBettype_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelRunningByBettype";
            _metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                _objectDb.Received(1).SetParameterValue("BetType", _metaModel.BetType);
                AssertPagingParameters(_objectDb, _metaModel);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByRunningBetListByMultipleBettype_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelRunningByBettypeList";
            _metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                _objectDb.Received(1).SetParameterValue("BetType", _metaModel.MultipleBettype);
                AssertPagingParameters(_objectDb, _metaModel);
            });
        }

        [TestCase(2, 2)]
        [TestCase(null, -1)]
        public void CreateBetListParameters_CalledByMatchWLBettypeBetlist_ParametersPassedCorrectly(int? liveIndicator, int expectedLiveIndicator)
        {
            // Arrange
            const string UspKey = "Lrf_BetList_MatchWL_Bettype";

            _metaModel.LiveIndicator = liveIndicator;
            _metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                _objectDb.Received(1).SetParameterValue("BetType", _metaModel.BetType);
                _objectDb.Received(1).SetParameterValue("MatchId", _metaModel.MatchId);
                _objectDb.Received(1).SetParameterValue("LiveIndicator", expectedLiveIndicator);
                _objectDb.Received(1).SetParameterValue("Sort", _metaModel.SortBy + " " + _metaModel.SortOrder);
                AssertPagingParameters(_objectDb, _metaModel);
                AssertSummaryParameters(_objectDb);
                _objectDb.Received(1).SetParameterValue("Date", _metaModel.FromDate);
                _objectDb.Received(1).SetParameterValue("showSB", true);
                _objectDb.Received(1).SetParameterValue("showNG", true);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByMatchWLOutRightBetList_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_MatchWL_Outright";
            _metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                AssertDateRangeParameters(_objectDb, _metaModel);
                _objectDb.Received(1).SetParameterValue("MoveBetsDay", _metaModel.MoveBetsDay);
                _objectDb.Received(1).SetParameterValue("Sort", _metaModel.SortBy + " " + _metaModel.SortOrder);
                AssertPagingParameters(_objectDb, _metaModel);
                AssertSummaryParameters(_objectDb);
                _objectDb.Received(1).SetParameterValue("showSB", true);
                _objectDb.DidNotReceive().SetParameterValue("showNG", true);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByMatchWLParlayBetList_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_MatchWL_Parlay";
            _metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                _objectDb.Received(1).SetParameterValue("Sort", _metaModel.SortBy + " " + _metaModel.SortOrder);
                _objectDb.Received(1).SetParameterValue("BetType", _metaModel.BetType);
                _objectDb.Received(1).SetParameterValue("MoveBetsDay", _metaModel.MoveBetsDay);
                AssertDateRangeParameters(_objectDb, _metaModel);
                AssertPagingParameters(_objectDb, _metaModel);
                AssertSummaryParameters(_objectDb);
                _objectDb.Received(1).SetParameterValue("showSB", true);
                _objectDb.Received(1).SetParameterValue("showNG", true);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByPlatformWinLossBetList_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_PlatformWinLoss_Mem";
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
                _objectDb.Received(1).SetParameterValue("PlatformType", _metaModel.PlatformType);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByOutStandingCompletedBetList_ParametersPassedCorrectlyWithoutSort()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_CustomerList_Mem";
            _metaModel.SortBy = string.Empty;
            _metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                _objectDb.Received(1).SetParameterValue("CustId", _metaModel.CustId);
                AssertDateRangeParameters(_objectDb, _metaModel);
                AssertPagingParameters(_objectDb, _metaModel);
                AssertSummaryParameters(_objectDb);
                _objectDb.Received(1).SetParameterValue("showSB", true);
                _objectDb.Received(1).SetParameterValue("showNG", true);
                _objectDb.DidNotReceive().SetParameterValue("Sort", Arg.Any<string>());
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByOutStandingCompletedBetList_ParametersPassedCorrectlyWithSort()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_CustomerList_Mem";
            _metaModel.SortBy = "Column";
            _metaModel.SortOrder = "asc";
            _metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                _objectDb.Received(1).SetParameterValue("CustId", _metaModel.CustId);
                AssertDateRangeParameters(_objectDb, _metaModel);
                AssertPagingParameters(_objectDb, _metaModel);
                AssertSummaryParameters(_objectDb);
                _objectDb.Received(1).SetParameterValue("showSB", true);
                _objectDb.Received(1).SetParameterValue("showNG", true);
                _objectDb.Received(1).SetParameterValue("Sort", _metaModel.SortBy + " " + _metaModel.SortOrder);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByDailyStatementBetList_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelByDayAll_Mem";
            _metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                AssertDailyStatementBetList(_objectDb, _metaModel);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByDailyStatementBetListOld_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelByDayAll_Mem_Old";
            _metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, _metaModel);

            // Assert
            AssertDailyStatementBetList(_objectDb, _metaModel);
        }

        [Test]
        public void CreateBetListParameters_CalledByRunningBetList_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelRunningByDate_Mem";
            _metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                _objectDb.Received(1).SetParameterValue("CustId", _metaModel.CustId);
                AssertDateRangeParameters(_objectDb, _metaModel);
                AssertPagingParameters(_objectDb, _metaModel);
                _objectDb.Received(1).SetParameterValue("showSB", true);
                _objectDb.Received(1).SetParameterValue("showNG", true);
            });
        }

        [TestCase(2, 2)]
        [TestCase(null, -1)]
        public void CreateBetListParameters_CalledByMatchRunningBetLitByBettype_ParametersPassedCorrectly(int? liveIndicator, int expectedLiveIndicator)
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelRunningByBettypeMatch";

            _metaModel.LiveIndicator = liveIndicator;
            _metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                _objectDb.Received(1).SetParameterValue("BetType", _metaModel.BetType);
                _objectDb.Received(1).SetParameterValue("MatchId", _metaModel.MatchId);
                AssertPagingParameters(_objectDb, _metaModel);
                _objectDb.Received(1).SetParameterValue("LiveIndicator", expectedLiveIndicator);
                _objectDb.Received(1).SetParameterValue("betteam", _metaModel.BetTeam);
            });
        }

        [TestCase("Test", "Test", 1, 1)]
        [TestCase(null, "", null, -1)]
        public void CreateBetListParameters_CalledByMatchRunningBetLitByMultipleBettypeOfMatch_ParametersPassedCorrectly(
            string multipleBettype,
            string expectedMultipleBettype,
            int? liveIndicator,
            int expectedLiveIndicator)
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelRunningByMultipleBettypeMatch";

            _metaModel.MultipleBettype = multipleBettype;
            _metaModel.LiveIndicator = liveIndicator;
            _metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                _objectDb.Received(1).SetParameterValue("BetType", expectedMultipleBettype);
                _objectDb.Received(1).SetParameterValue("MatchId", _metaModel.MatchId);
                AssertPagingParameters(_objectDb, _metaModel);
                _objectDb.Received(1).SetParameterValue("LiveIndicator", expectedLiveIndicator);
                _objectDb.Received(1).SetParameterValue("betteam", _metaModel.BetTeam);
            });
        }

        [TestCase(2, 2)]
        [TestCase(null, -1)]
        public void CreateBetListParameters_CalledByRunningBetListByForeCastMatch_ParametersPassedCorrectly(int? liveIndicator, int expectedLiveIndicator)
        {
            // Arrange
            const string UspKey = "Lrf_BetList_Forecast_SelByMatch";

            _metaModel.LiveIndicator = liveIndicator;
            _metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                _objectDb.Received(1).SetParameterValue("BetType", _metaModel.BetType);
                _objectDb.Received(1).SetParameterValue("MatchId", _metaModel.MatchId);
                AssertPagingParameters(_objectDb, _metaModel);
                _objectDb.Received(1).SetParameterValue("LiveIndicator", expectedLiveIndicator);
                _objectDb.Received(1).SetParameterValue("betteam", _metaModel.BetTeam);
                _objectDb.Received(1).SetParameterValue("BetChoice", _metaModel.BetChoice);
                _objectDb.Received(1).SetParameterValue("Handicap", _metaModel.Handicap);
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
        public void CreateBetListParameters_CalledByWinLossByBetType_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelWinLossByBetType";
            _metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                _objectDb.Received(1).SetParameterValue("Sort", _metaModel.SortBy + " " + _metaModel.SortOrder);
                _objectDb.Received(1).SetParameterValue("BetType", _metaModel.BetType);
                AssertDateRangeParameters(_objectDb, _metaModel);
                AssertMoveBetParams(_objectDb, _metaModel);
                AssertPagingParameters(_objectDb, _metaModel);
                AssertSummaryParameters(_objectDb);
            });
        }

        [TestCase("nickname", "usd", "nickname ")]
        [TestCase("test", "usd", "test ")]
        public void CreateBetListParameters_CreateBetListSortingParameters_ParametersPassedCorrectly(string sortBy, string baseCurrency, string expectedSortBy)
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelWinLossByBetType";

            var metaModel = new BetListMetaModel { SortBy = sortBy, BaseCurrency = baseCurrency, UspKey = UspKey };
            _metaModel.UspKey = UspKey;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, metaModel);

            // Assert
            _objectDb.Received(1).SetParameterValue("Sort", expectedSortBy);
        }

        [Test]
        public void CreateBetListParameters_CreateBetListSortingParameters_SortByIsEmpty_SortByParameterIsNotPassed()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelWinLossByBetType";

            var metaModel = new BetListMetaModel
            {
                UspKey = UspKey
            };

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, metaModel);

            // Assert
            _objectDb.DidNotReceive().SetParameterValue("Sort", Arg.Any<string>());
        }

        [TestCase(true, 1, 100, 1, int.MaxValue - 1)]
        [TestCase(false, 2, 50, 2, 50)]
        public void CreateBetListParameters_CalledByLiveReportNumberGameBetlist_ParametersPassedPagingInfoCorrectly(
            bool isExportExcel,
            int pageIndex,
            int pageSize,
            int expectedIndex,
            int expectedSize)
        {
            // Arrange
            const string UspKey = "Lrf_BetList_LiveReport_SelWinlossNumberGame";
            var metaModel = new LiveReportBetListMetaModel
            {
                IsExportExcel = isExportExcel,
                PageIndex = pageIndex,
                PageSize = pageSize,
                UspKey = UspKey
            };

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("PageSize", expectedSize);
                _objectDb.Received(1).SetParameterValue("PageIndex", expectedIndex);
            });
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

        private static void AssertDailyStatementBetList(IObjectDb objectDb, BetListMetaModel metaModel)
        {
            objectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
            objectDb.Received(1).SetParameterValue("CustId", metaModel.CustId);
            objectDb.Received(1).SetParameterValue("fromdate", metaModel.FromDate);
            objectDb.Received(1).SetParameterValue("transtype", metaModel.TransType);
            AssertPagingParameters(objectDb, metaModel);
            AssertSummaryParameters(objectDb);
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