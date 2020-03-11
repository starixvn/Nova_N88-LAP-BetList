namespace Fanex.Athena.Result.DataAccess.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.DataAccess.UnitTests;
    using Fanex.Athena.UnitTest.Common.NUnitExtensions;
    using Fanex.Result.Core.Happy5.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Result.DataAccess;
    using Result.Model.MetaModel;
    using UnitTest.Common.Helpers;

    public class ResultNumberGameDataAccessTest_Happy5 : BaseDataAccessHelperTest<ResultNumberGameDataAccess>
    {
        private const int MatchId = 122;

        private const string GetHappy5MatchesSp = "Lrf_Result_MatchSelNumberGameByLeague";

        private const string GetPopUpHappy5MatchSp = "Lrf_BetList_SelMatchResultNumberGame";

        private readonly Dictionary<string, object> _readerDataMapping = new Dictionary<string, object>
            {
                { "leagueid", 111 }, { "matchid", 222 }, { "eventstatus", "win" }, { "eventdate", "03/16/2017" }, { "isrefund", "1" },
                { "kickofftime", "1/1/2017" }, { "matchcode", "test" }, { "seq", 3 }, { "number", 4 }
            };

        private readonly ResultMetaModel _metaModel = new ResultMetaModel
        {
            FromDate = new DateTime(2017, 1, 1),
            ToDate = new DateTime(2017, 2, 2),
            SportId = 123
        };

        [Test]
        public async Task GetHappy5MatchesAsync_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetHappy5MatchesSp)).Returns(ObjectDb);
            _metaModel.LeagueId = 1;

            dynamic result = null;
            ObjectDb.ExecuteReaderAsync(Arg.Do<dynamic>(x => result = x));
            var expectedResult = new
            {
                fromdate = _metaModel.FromDate,
                leagueid = _metaModel.LeagueId,
                sporttype = _metaModel.SportId,
                isshowall = true
            };

            // Action
            await DataAccess.GetHappy5MatchesAsync(_metaModel).ConfigureAwait(false);

            // Assert
            Assert.That(result, Tobe.SameProperties(expectedResult));
        }

        [Test]
        public async Task GetHappy5MatchesAsync_WhenCall_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetHappy5MatchesSp)).Returns(ObjectDb);
            _metaModel.LeagueId = 1;

            Reader.Read().Returns(true, true, false);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("isrefund");

            MapReaderData(_readerDataMapping);

            var expectedResult = new List<Happy5>
            {
                GetHappy5SampleDataItem()
            };

            // Action
            var result = await DataAccess.GetHappy5MatchesAsync(_metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [Test]
        public async Task GetPopUpHappy5MatchAsync_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpHappy5MatchSp)).Returns(ObjectDb);
            dynamic result = null;
            ObjectDb.ExecuteReaderAsync(Arg.Do<dynamic>(x => result = x));
            var expected = new
            {
                matchId = MatchId
            };

            // Action
            await DataAccess.GetPopUpHappy5MatchAsync(MatchId).ConfigureAwait(false);

            // Assert
            Assert.That(result, Tobe.SameProperties(expected));
        }

        [Test]
        public async Task GetPopUpHappy5MatchAsync_WhenCall_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpHappy5MatchSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, true, false);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("isrefund");

            MapReaderData(_readerDataMapping);
            var dataItem = GetHappy5SampleDataItem();
            dataItem.IsPending = true;
            var expectedResult = new List<Happy5> { dataItem };

            // Action
            var result = await DataAccess.GetPopUpHappy5MatchAsync(222).ConfigureAwait(false);

            // Assert
            Assert.That(ObjectHelper.ToJson(result), Is.EqualTo(ObjectHelper.ToJson(expectedResult)));
        }

        private static Happy5 GetHappy5SampleDataItem()
        {
            return new Happy5
            {
                LeagueId = 111,
                MatchId = 222,
                EventStatus = "win",
                EventDate = new DateTime(2017, 3, 16),
                IsPending = true,
                KickOffTime = new DateTime(2017, 1, 1),
                MatchCode = "test",
                Seq = new List<int> { 3, 3 },
                Ball = new List<int> { 4, 4 }
            };
        }
    }
}