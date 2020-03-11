namespace Fanex.Athena.BetList.Service.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Athena.BetList.DataAccess.Interfaces;
    using Athena.BetList.Model.DTO;
    using Athena.BetList.Model.MetaModel;
    using Athena.BetList.Service;
    using Athena.BetList.Service.Interfaces;
    using Common.Constant;
    using Common.Utility.Services.Interfaces;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class LastBetsMonitoringBetListServiceTest
    {
        private readonly IDictionary<int, string> _currencyList = new Dictionary<int, string> { { 20, "UUS" } };
        private ILastBetsMonitoringBetListDataAccess _lastBetsMonitoringBetListDataAccess;
        private ILastBetsMonitoringBetListService _betListService;
        private IMemoryCacheService _cache;

        [SetUp]
        public void SetUp()
        {
            _cache = Substitute.For<IMemoryCacheService>();
            _lastBetsMonitoringBetListDataAccess = Substitute.For<ILastBetsMonitoringBetListDataAccess>();
            _betListService = new LastBetsMonitoringBetListService(_lastBetsMonitoringBetListDataAccess, _cache);
        }

        [TearDown]
        public void TearDown()
        {
            _betListService = null;
            _lastBetsMonitoringBetListDataAccess = null;
        }

        [TestCase(BetType.Outright)]
        [TestCase(BetType.Handicap)]
        [TestCase(BetType.Win)]
        public async Task GetLastBetsMonitoringData_MatchDataIsNotCached_TicketIsAssignedDataCorrectlyFromDBMatchList(BetType betType)
        {
            // Arrange
            var match = FakeHelper.Create<BetListMatch>();
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.AddRange(new List<LrfTicket>
            {
                new LrfTicket { MatchId = match.MatchId, BetTypeId = (int)betType }
            });

            _lastBetsMonitoringBetListDataAccess.GetLastBetsMonitoringData(
                Arg.Any<LastBetsMonitoringMetaModel>())
                .Returns(ticketResult);

            _cache.Get<IList<BetListMatch>>(Arg.Any<string>()).Returns(x => { return null; });

            _lastBetsMonitoringBetListDataAccess.GetMatchList(Arg.Any<string>())
                .Returns(new List<BetListMatch> { match });

            // Action
            var result = await _betListService.GetLastBetsMonitoringData(
                new LastBetsMonitoringMetaModel
                {
                    OddsType = string.Empty
                }
                );

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.MainTicketList[0].SportTypeId, Is.EqualTo(match.SportType));
                Assert.That(result.MainTicketList[0].LeagueId, Is.EqualTo(match.LeagueId));
                Assert.That(result.MainTicketList[0].MatchCode, Is.EqualTo(match.MatchCode));
                Assert.That(result.MainTicketList[0].HomeId, Is.EqualTo(match.HomeId));
                Assert.That(result.MainTicketList[0].AwayId, Is.EqualTo(match.AwayId));
                Assert.That(result.MainTicketList[0].Race, Is.EqualTo(match.Race));
            });
        }

        [TestCase(BetType.WinPlace)]
        [TestCase(BetType.Place)]
        [TestCase(BetType.OddEven)]
        public async Task GetLastBetsMonitoringData_MatchDataIsCached_TicketIsAssignedDataCorrectlyFromCachedMatchList(BetType betType)
        {
            // Arrange
            var match = FakeHelper.Create<BetListMatch>();
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.AddRange(new List<LrfTicket>
            {
                new LrfTicket { MatchId = match.MatchId, BetTypeId = (int)betType }
            });

            _lastBetsMonitoringBetListDataAccess.GetLastBetsMonitoringData(Arg.Any<LastBetsMonitoringMetaModel>())
                .Returns(ticketResult);

            _cache.Get<IList<BetListMatch>>(Arg.Any<string>()).Returns(new List<BetListMatch> { match });

            _lastBetsMonitoringBetListDataAccess.GetMatchList(Arg.Any<string>()).Returns(new List<BetListMatch>());

            // Action
            var result = await _betListService.GetLastBetsMonitoringData(new LastBetsMonitoringMetaModel { OddsType = string.Empty }).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.MainTicketList[0].SportTypeId, Is.EqualTo(match.SportType));
                Assert.That(result.MainTicketList[0].LeagueId, Is.EqualTo(match.LeagueId));
                Assert.That(result.MainTicketList[0].MatchCode, Is.EqualTo(match.MatchCode));
                Assert.That(result.MainTicketList[0].HomeId, Is.EqualTo(match.HomeId));
                Assert.That(result.MainTicketList[0].AwayId, Is.EqualTo(match.AwayId));
                Assert.That(result.MainTicketList[0].Race, Is.EqualTo(match.Race));
            });
        }

        [TestCase(BetType.Outright)]
        [TestCase(BetType.Win)]
        [TestCase(BetType.Place)]
        [TestCase(BetType.WinPlace)]
        public void GetLastBetsMonitoringData_OutRightBetType_CallCorrectCacheKeyAndSpName(BetType betType)
        {
            // Arrange
            const string ExpectedCacheKey = "LastBetsOutrightMatchList";
            const string ExpectedSpName = "Lrf_Monitor_OutrightSel";

            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.AddRange(new List<LrfTicket>
            {
                new LrfTicket { MatchId = 999, BetTypeId = (int)betType }
            });

            _lastBetsMonitoringBetListDataAccess.GetLastBetsMonitoringData(Arg.Any<LastBetsMonitoringMetaModel>())
                .Returns(ticketResult);

            _cache.Get<IList<BetListMatch>>(Arg.Any<string>()).Returns(x => { return null; });

            _lastBetsMonitoringBetListDataAccess.GetMatchList(Arg.Any<string>())
                .Returns(new List<BetListMatch>());

            // Action
            var result = _betListService.GetLastBetsMonitoringData(
                new LastBetsMonitoringMetaModel { OddsType = string.Empty });

            // Assert
            Assert.Multiple(() =>
            {
                _cache.Received().Get<IList<BetListMatch>>(ExpectedCacheKey);
                _lastBetsMonitoringBetListDataAccess.Received().GetMatchList(ExpectedSpName);
            });
        }

        [TestCase(BetType.OverUnder)]
        [TestCase(BetType.OddEven)]
        [TestCase(BetType.NextGoal)]
        [TestCase(BetType.MoneyLine)]
        public void GetLastBetsMonitoringData_IsNotOutRightBetType_CallCorrectCacheKeyAndSpName(BetType betType)
        {
            // Arrange
            const string ExpectedCacheKey = "LastBetsMatchList";
            const string ExpectedSpName = "Lrf_Monitor_MatchSel";

            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.AddRange(new List<LrfTicket>
            {
                new LrfTicket { MatchId = 999, BetTypeId = (int)betType }
            });

            _lastBetsMonitoringBetListDataAccess.GetLastBetsMonitoringData(Arg.Any<LastBetsMonitoringMetaModel>())
                .Returns(ticketResult);

            _cache.Get<IList<BetListMatch>>(Arg.Any<string>()).Returns(x => { return null; });

            _lastBetsMonitoringBetListDataAccess.GetMatchList(Arg.Any<string>())
                .Returns(new List<BetListMatch>());

            // Action
            var result = _betListService.GetLastBetsMonitoringData(
                new LastBetsMonitoringMetaModel { OddsType = string.Empty });

            // Assert
            Assert.Multiple(() =>
            {
                _cache.Received().Get<IList<BetListMatch>>(ExpectedCacheKey);
                _lastBetsMonitoringBetListDataAccess.Received().GetMatchList(ExpectedSpName);
            });
        }

        [TestCase(OddsType.Decimal, -2, -1)]
        [TestCase(OddsType.Malay, -2, 1.5)]
        [TestCase(OddsType.American, -2, 1.5)]
        [TestCase(OddsType.American, 4, 2)]
        [TestCase(OddsType.American, 1.31, 2.31)]
        [TestCase("0", 5, 5)]
        public async Task GetLastBetsMonitoringData_WhenCall_OddsIsConvertedToDecimal(string oddsType, decimal oddsValue, decimal expectedValue)
        {
            // Arrange
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.AddRange(new List<LrfTicket>
            {
                new LrfTicket
                {
                    BetTypeId = 1,
                    Odds = oddsValue,
                    OddsType = oddsType
                }
            });
            _lastBetsMonitoringBetListDataAccess.GetLastBetsMonitoringData(Arg.Any<LastBetsMonitoringMetaModel>())
                .Returns(ticketResult);

            _cache.Get<IList<BetListMatch>>(Arg.Any<string>()).Returns(new List<BetListMatch>());

            // Action
            var result = await _betListService.GetLastBetsMonitoringData(
                new LastBetsMonitoringMetaModel
                {
                    OddsType = "dec"
                }
                );

            // Assert
            Assert.That(result.MainTicketList[0].Odds, Is.EqualTo(expectedValue));
        }

        [TestCase(OddsType.Decimal, -2, -2)]
        [TestCase(OddsType.Malay, -2, 0.5)]
        [TestCase(OddsType.American, -2, 0.5)]
        [TestCase(OddsType.American, 0, 0)]
        [TestCase(OddsType.Hongkong, 2, -0.5)]
        [TestCase(OddsType.Indo, 2, 2)]
        public async Task GetLastBetsMonitoringData_WhenCall_OddsIsConvertedToMy(string oddsType, decimal oddsValue, decimal expectedValue)
        {
            // Arrange
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.AddRange(new List<LrfTicket>
            {
                new LrfTicket
                {
                    BetTypeId = 1,
                    Odds = oddsValue,
                    OddsType = oddsType
                }
            });
            _lastBetsMonitoringBetListDataAccess.GetLastBetsMonitoringData(Arg.Any<LastBetsMonitoringMetaModel>())
                .Returns(ticketResult);

            _cache.Get<IList<BetListMatch>>(Arg.Any<string>()).Returns(new List<BetListMatch>());

            // Action
            var result = await _betListService.GetLastBetsMonitoringData(
                new LastBetsMonitoringMetaModel
                {
                    OddsType = "my"
                }
                );

            // Assert
            Assert.That(result.MainTicketList[0].Odds, Is.EqualTo(expectedValue));
        }

        [Test]
        public async Task GetLastBetsMonitoringData_WhenCallWithTimeZoneGreaterThanNegativeNumber4_TransDateOfTicketUpdateValue()
        {
            // Arrange
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.AddRange(new List<LrfTicket>
            {
                new LrfTicket
                {
                    BetTypeId = 1,
                    Odds = -2,
                    OddsType = OddsType.American,
                    TransDate = new DateTime(2017, 3, 17, 0, 0, 0)
                }
            });
            _lastBetsMonitoringBetListDataAccess.GetLastBetsMonitoringData(
                Arg.Any<LastBetsMonitoringMetaModel>()).Returns(ticketResult);

            _cache.Get<IList<BetListMatch>>(Arg.Any<string>()).Returns(new List<BetListMatch>());

            var expectedResult = new DateTime(2017, 3, 17, 4, 0, 0);

            // Action
            var result = await _betListService.GetLastBetsMonitoringData(
                new LastBetsMonitoringMetaModel
                {
                    OddsType = "my",
                    TimeZone = 0
                }
                );

            // Assert
            Assert.That(result.MainTicketList[0].TransDate, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetLastBetsMonitoringData_WhenCallWithTimeZoneLessThanNegativeNumber4_TransDateOfTicketUpdateValue()
        {
            // Arrange
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.AddRange(new List<LrfTicket>
            {
                new LrfTicket
                {
                    BetTypeId = 1,
                    Odds = -2,
                    OddsType = OddsType.American,
                    TransDate = new DateTime(2017, 3, 17, 0, 0, 0)
                }
            });
            _lastBetsMonitoringBetListDataAccess.GetLastBetsMonitoringData(
                Arg.Any<LastBetsMonitoringMetaModel>()).Returns(ticketResult);

            _cache.Get<IList<BetListMatch>>(Arg.Any<string>()).Returns(new List<BetListMatch>());

            var expectedResult = new DateTime(2017, 3, 17, 0, 0, 0);

            // Action
            var result = await _betListService.GetLastBetsMonitoringData(
                new LastBetsMonitoringMetaModel
                {
                    OddsType = "my",
                    TimeZone = -5
                }
                );

            // Assert
            Assert.That(result.MainTicketList[0].TransDate, Is.EqualTo(expectedResult));
        }
    }
}