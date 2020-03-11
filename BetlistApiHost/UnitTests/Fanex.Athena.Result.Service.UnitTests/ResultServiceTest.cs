namespace Fanex.Athena.Result.Service.UnitTests
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Fanex.Result.Core.Racing.DTO;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class ResultServiceTest
    {
        private IResultDataAccess _dataAccess;
        private IResultNumberGameDataAccess _numberGameDataAccess;
        private ResultService _service;

        [SetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<IResultDataAccess>();
            _numberGameDataAccess = Substitute.For<IResultNumberGameDataAccess>();
            _service = new ResultService(_dataAccess, _numberGameDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public async Task GetOutRightAsync_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new ResultMetaModel();

            // Action
            await _service.GetOutRightAsync(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received(1).GetOutRightAsync(metaModel);
            });
        }

        [Test]
        public async Task GetSportsAsync_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            const int LicenseeId = 1;
            var date = new DateTime(2018, 1, 1);

            // Action
            await _service.GetSportsAsync(date, LicenseeId);

            // Assert
            await _dataAccess.Received(1).GetSportsAsync(date, LicenseeId);
        }

        [Test]
        public async Task GetLeaguesAsync_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            const int LicenseeId = 1;
            var date = new DateTime(2018, 1, 1);

            // Action
            await _service.GetLeaguesAsync(date, LicenseeId);

            // Assert
            await _dataAccess.Received(1).GetLeaguesAsync(date, LicenseeId);
        }

        [Test]
        public async Task GetMatchesAsync_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new ResultMetaModel();

            // Action
            await _service.GetMatchesAsync(metaModel);

            // Assert
            await _dataAccess.Received(1).GetMatchesAsync<Sportsbook>(metaModel);
        }

        [Test]
        public async Task GetNumberGameMatchesAsync_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new ResultMetaModel();

            // Action
            await _service.GetNumberGameMatchesAsync(metaModel);

            // Assert
            await _numberGameDataAccess.Received(1).GetNumberGameMatchesAsync(metaModel);
        }

        [Test]
        public async Task  GetHappy5MatcheAsyncs_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new ResultMetaModel();

            // Action
            await _service.GetHappy5MatchesAsync(metaModel);

            // Assert
            await _numberGameDataAccess.Received(1).GetHappy5MatchesAsync(metaModel);
        }

        [Test]
        public async Task GetVirtualSportMatchesAsync_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new ResultMetaModel();

            // Action
            await _service.GetVirtualSportMatchesAsync(metaModel);

            // Assert
            await _dataAccess.Received(1).GetVirtualSportMatchesAsync(metaModel);
        }

        [Test]
        public async Task GetSportsbookGameDetailAsync_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            const int MatchId = 1;

            // Action
            await _service.GetSportsbookGameDetailAsync(MatchId);

            // Assert
            await _dataAccess.Received(1).GetSportsbookGameDetailAsync(MatchId);
        }

        [Test]
        public async Task GetPopUpMatchAsync_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new ResultPopupMetaModel
            {
                MatchId = 1,
                Bettype = 3,
                BetId = 4,
                Handicap1 = 3,
                Handicap2 = 5,
                RaceNo = 1
            };

            // Action
            await _service.GetPopUpMatchAsync(metaModel);

            // Assert
            await _dataAccess.Received(1).GetPopUpMatchesAsync<Sportsbook>(Arg.Is<ResultPopupMetaModel>(x => x == metaModel && !x.RaceNo.HasValue));
        }

        [Test]
        public async Task GetPopUpRacingMatchAsync_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new ResultPopupMetaModel
            {
                MatchId = 1,
                RaceNo = 2,
                Bettype = 3,
                BetId = 4
            };

            // Action
            await _service.GetPopUpVirtualSportMatchAsync(metaModel);

            // Assert
            await _dataAccess.Received(1).GetPopUpMatchesAsync<Racing>(metaModel);
        }

        [Test]
        public async Task GetPopUpParlayMatchAsync_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            const long RefNo = 4;

            // Action
            await _service.GetPopUpParlayMatchAsync(RefNo);

            // Assert
            await _dataAccess.Received(1).GetPopUpParalayMatchAsync(RefNo);
        }

        [Test]
        public async Task GetPopUpNumberGameMatchsAsync_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            const int MatchId = 1;

            // Action
            await _service.GetPopUpNumberGameMatchsAsync(MatchId);

            // Assert
            await _numberGameDataAccess.Received(1).GetPopUpNumberGameMatchAsync(MatchId);
        }

        [Test]
        public async Task GetPopUpHappy5Matches_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            const int MatchId = 1;

            // Action
            await _service.GetPopUpHappy5MatchesAsync(MatchId);

            // Assert
            await _numberGameDataAccess.Received(1).GetPopUpHappy5MatchAsync(MatchId);
        }

        [Test]
        public async Task GetPopUpSingleMatchParlayAsync_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var matchId = FakeHelper.Create<int>();
            var marketId = FakeHelper.Create<string>();

            // Action
            await _service.GetPopUpSingleMatchParlayAsync(matchId, marketId);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received(1).GetPopUpSingleMatchParlayAsync(matchId, marketId);
            });
        }
    }
}