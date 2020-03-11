namespace Fanex.Athena.Result.DataAccess.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.DataAccess.Interfaces;
    using Common.DataAccess.UnitTests;
    using Common.Model.DTO;
    using Common.Utility.Services.Interfaces;
    using Fanex.Athena.UnitTest.Common.NUnitExtensions;
    using Fanex.Result.Core.Racing.DTO;
    using Fanex.Result.Core.Sportsbook.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Result.DataAccess;
    using Result.Model.MetaModel;

    [TestFixture]
    public partial class ResultDataAccessTest : BaseDataAccessHelperTest<ResultDataAccess>
    {
        private const string GetOutRightSp = "Lrf_Result_Outright";
        private const string GetSportsSp = "Lrf_Result_SportSelByDate";
        private const string GetLeaguesSp = "Lrf_Result_LeagueSelByDate";
        private const string GetVirtualSportMatchesSp = "Lrf_Result_RaceSelByMeeting";
        private const string GetPopUpMatchesSp = "Lrf_BetList_SelMatchResult";
        private const string GetPopUpParalayMatchSp = "Lrf_BetList_SelMatchResultParlay";
        private const string GetMatchesSp = "Lrf_Result_MatchSelByLeague";

        private const int MatchId = 1;
        private const int RaceNo = 2;
        private const int BettypeId = 5;
        private const long BetId = 6;

        private readonly BalanceUp _balance = new BalanceUp
        {
            MoveBetsDay = new DateTime(2016, 8, 10),
            BalanceUpDay = new DateTime(2016, 8, 31)
        };

        private readonly ResultMetaModel _metaModel = new ResultMetaModel
        {
            FromDate = new DateTime(2017, 1, 1),
            ToDate = new DateTime(2017, 2, 2)
        };

        protected override void SetUpDataAccess()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            balanceUpDataAccess.GetBalanceUp().Returns(_balance);
            var cacheRefAdapter = Substitute.For<ICacheRefAdapter>();
            var appSettings = Substitute.For<IAppSettings>();
            DataAccess = Substitute.ForPartsOf<ResultDataAccess>(
                balanceUpDataAccess,
                cacheRefAdapter,
                appSettings);
        }

        [Test]
        public async Task GetOutRightAsync_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetOutRightSp)).Returns(ObjectDb);
            dynamic param = null;
            ObjectDb.ExecuteReaderAsync(Arg.Do<dynamic>(arg => param = arg));
            var expectedParam = new
            {
                fromdate = _metaModel.FromDate,
                todate = _metaModel.ToDate,
                isshowall = true
            };

            // Action
            await DataAccess.GetOutRightAsync(_metaModel).ConfigureAwait(false);

            // Assert
            Assert.That(param, Tobe.SameProperties(expectedParam));
        }

        [Test]
        public async Task GetOutRightAsync_WhenCall_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetOutRightSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);

            var readerDataMapping = new
            {
                eventdate = "03/16/2017",
                sporttype = 1,
                leagueid = 2,
                teamid = 3,
                status = "win"
            };
            MapReaderData(readerDataMapping);

            var outrights = new List<Outright>
            {
                new Outright
                {
                    EventDate = new DateTime(2017, 3, 16),
                    SportType = 1,
                    LeagueId = 2,
                    TeamId = 3,
                    Status = "win"
                }
            };

            // Action
            var result = await DataAccess.GetOutRightAsync(_metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(outrights.Count));
                Assert.That(result[0], Tobe.SameProperties(outrights[0]));
            });
        }

        [Test]
        public async Task GetSportsAsync_ParameterPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetSportsSp)).Returns(ObjectDb);

            var date = new DateTime(2017, 3, 16);
            var licenseeId = 1;

            // Action
            await DataAccess.GetSportsAsync(date, licenseeId).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("eventdate", date);
                ObjectDb.Received(1).SetParameterValue("licenseeid", licenseeId);
                ObjectDb.Received(1).SetParameterValue("movebetsday", _balance.MoveBetsDay);
            });
        }

        [Test]
        public async Task GetSportsAsync_ReturnCorrectData()
        {
            // Arrange
            const int SportId = 111;
            DataAccess.GetObjectDb(Arg.Is(GetSportsSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            var expectedResult = new List<Sport>
            {
                new Sport { SportId = SportId }
            };
            var readerDataMapping = new
            {
                sporttype = SportId
            };
            MapReaderData(readerDataMapping);
            var date = new DateTime(2017, 3, 16);
            var licenseeId = 1;

            // Action
            var result = await DataAccess.GetSportsAsync(date, licenseeId).ConfigureAwait(false);

            // Assert
            Assert.That(result[0].SportId, Is.EqualTo(expectedResult[0].SportId));
        }

        [Test]
        public async Task GetLeaguesAsync_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetLeaguesSp)).Returns(ObjectDb);
            dynamic param = null;
            ObjectDb.ExecuteReaderAsync(Arg.Do<dynamic>(arg => param = arg));

            var date = new DateTime(2017, 3, 16);
            var sportId = 1;
            var expectedResult = new
            {
                eventdate = date,
                sporttype = sportId,
                isshowall = true,
                movebetsday = _balance.MoveBetsDay
            };

            // Action
            await DataAccess.GetLeaguesAsync(date, sportId).ConfigureAwait(false);
            var result = param;

            // Assert
            Assert.That(result, Tobe.SameProperties(expectedResult));
        }

        [Test]
        public async Task GetLeaguesAsync_WhenCall_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetLeaguesSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);

            var expectedResult = new List<League>
            {
                new League { LeagueId = 1 }
            };
            var readerDataMapping = new
            {
                leagueid = 1
            };
            MapReaderData(readerDataMapping);

            var date = new DateTime(2017, 3, 16);
            var sportId = 1;

            // Action
            var result = await DataAccess.GetLeaguesAsync(date, sportId).ConfigureAwait(false);

            // Assert
            Assert.That(result[0].LeagueId, Is.EqualTo(expectedResult[0].LeagueId));
        }

        [Test]
        public async Task GetRacingMatchesAsync_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetVirtualSportMatchesSp)).Returns(ObjectDb);
            dynamic param = null;
            ObjectDb.ExecuteReaderAsync(Arg.Do<dynamic>(x => param = x));
            _metaModel.SportId = 1;
            _metaModel.LeagueId = 2;
            var expectedResult = new
            {
                fromdate = _metaModel.FromDate,
                leagueid = _metaModel.LeagueId,
                sporttype = _metaModel.SportId
            };

            // Action
            await DataAccess.GetVirtualSportMatchesAsync(_metaModel).ConfigureAwait(false);
            var result = param;

            // Assert
            Assert.That(result, Tobe.SameProperties(expectedResult));
        }

        [Test]
        public async Task GetRacingMatchesAsync_WhenCall_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetVirtualSportMatchesSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);

            var readerDataMapping = new Dictionary<string, object>
            {
                { "meetingid", 1 }, { "HorseId", "1" }, { "HorseNo", "2" }, { "IsFavor", "1" }, { "IsSCR", "1" },
                { "place", 2 }, { "placeodds", 10.5m }, { "odds", 0.5m }, { "racename", 4 }, { "kickofftime", "03/16/2017" },
                { "rowcount", 4 }, { "SCR", "SCR" }, { "TF", "TF" }
            };
            MapReaderData(readerDataMapping);

            var expectedResult = GetExpectedVirtualSportData();

            // Action
            var result = await DataAccess.GetVirtualSportMatchesAsync(_metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [Test]
        public async Task GetSportsbookGameDetailAsync_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_Result_MatchSelByLeague_Detail")).Returns(ObjectDb);

            // Action
            await DataAccess.GetSportsbookGameDetailAsync(MatchId).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("matchid", MatchId);
            });
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task GetSportsbookGameDetailAsync_WhenCall_ReturnCorrectData(bool betStatusIsNull)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_Result_MatchSelByLeague_Detail")).Returns(ObjectDb);

            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);

            Reader.FieldCount.Returns(3);
            Reader.GetName(0).Returns("lcolor");
            Reader.GetName(1).Returns("LiveAwayScore");
            Reader.GetName(2).Returns("LiveHomeScore");

            var expectedResult = GetExpectedSportsbookData();
            expectedResult.Bettype = 0;
            expectedResult.BetID = null;

            var dataMapping = GetReaderDataMappingForSportsbookType();
            var dataMappingForGameDetail = GetreaderDataMappingForGameDetail();
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            MapReaderData(dataMapping);
            int matchId = 11;

            if (betStatusIsNull)
            {
                dataMapping["betstatus"] = DBNull.Value;
                expectedResult.BetStatus = string.Empty;
            }

            // Action
            var result = await DataAccess.GetSportsbookGameDetailAsync(matchId).ConfigureAwait(false);

            // Assert
            Assert.That(result, Tobe.SameProperties(expectedResult));
        }

        private static Sportsbook GetExpectedSportsbookData(List<GameDetail> gameDetails = null)
        {
            return new Sportsbook
            {
                AwayId = 1,
                HomeId = 2,
                BetStatus = "win",
                LeagueId = 3,
                MatchId = 4,
                EventStatus = "lose",
                FinalAwayScore = 5,
                HtAwayScore = 6,
                Color = "red",
                FinalHomeScore = 7,
                HtHomeScore = 8,
                EventDate = new DateTime(2017, 3, 16),
                IsNeutral = true,
                KickOffTime = new DateTime(2017, 3, 16),
                MatchCode = "matchcode",
                GoalRecord = "goalrecord",
                IsHTCal = true,
                Calculate = true,
                RefNo = string.Empty,
                LiveAwayScore = 9,
                LiveHomeScore = 10,
                BetID = BetId.ToString(),
                Bettype = BettypeId,
                GameDetails = gameDetails ?? GetExpectedGameDetailData()
            };
        }

        private static List<GameDetail> GetExpectedGameDetailData()
        {
            return new List<GameDetail>
            {
                new GameDetail
                {
                    MatchId = 4,
                    ResourceId = "ResourceId",
                    HomeValue = "HomeValue",
                    AwayValue = "AwayValue",
                    ResultStatusID = 1,
                    SettlementStatusID = 2,
                    OtherInfo = "OtherInfo"
                }
            };
        }

        private static void MergeReaderDataMapping(Dictionary<string, object> source, Dictionary<string, object> destination)
        {
            foreach (var item in destination)
            {
                source[item.Key] = item.Value;
            }
        }

        private static Dictionary<string, object> GetReaderDataMappingForSportsbookType()
        {
            return new Dictionary<string, object>
            {
                { "awayID", 1 }, { "homeID", 2 }, { "betstatus", "win" }, { "leagueid", 3 }, { "matchid", 4 },
                { "eventstatus", "lose" }, { "finalawayscore", 5 }, { "htawayscore", 6 }, { "lcolor", "red" },
                { "finalhomescore", 7 }, { "hthomescore", 8 }, { "eventdate", "03/16/2017" }, { "IsNeutral", true },
                { "IsPending", true }, { "kickofftime", "03/16/2017" }, { "matchcode", "matchcode" }, { "goalrecord", "goalrecord" },
                { "IsHTCal", true }, { "Calculate", true }, { "LiveAwayScore", 9 }, { "LiveHomeScore", 10 }
            };
        }

        private static Dictionary<string, object> GetreaderDataMappingForGameDetail()
        {
            return new Dictionary<string, object>
            {
                { "ResultStatusID", 1 }, { "SettlementStatusID", 2 }, { "matchid", 4 }, { "ResourceId", "ResourceId" },
                { "HomeValue", "HomeValue" }, { "AwayValue", "AwayValue" }, { "OtherInfo", "OtherInfo" }
            };
        }

        private static List<Racing> GetExpectedVirtualSportData()
        {
            return new List<Racing>
            {
                new Racing
                {
                    LeagueId = 1,
                    EngineId = "1",
                    EngineNo = "2",
                    IsFavor = true,
                    IsSCR = true,
                    Place = 2,
                    PlaceOdds = 10.5m,
                    WinOdds = 0.5m,
                    RaceName = 4,
                    KickOffTime = new DateTime(2017, 3, 16),
                    Favourite = "1",
                    RowCount = 4,
                    SCROfMatch = "SCR",
                    TFOfMatch = "TF"
                }
            };
        }
    }
}