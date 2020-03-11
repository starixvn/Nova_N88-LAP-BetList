namespace Fanex.Athena.Result.DataAccess.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.UnitTest.Common.NUnitExtensions;
    using Fanex.Result.Core.Sportsbook.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public partial class ResultDataAccessTest
    {
        [Test]
        public async Task GetMatchesAsync_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetMatchesSp)).Returns(ObjectDb);
            dynamic param = null;
            ObjectDb.ExecuteReaderAsync(Arg.Do<dynamic>(x => param = x));
            _metaModel.SportId = 1;
            _metaModel.LeagueId = 2;

            var expected = new
            {
                fromdate = _metaModel.FromDate,
                sporttype = _metaModel.SportId,
                leagueid = _metaModel.LeagueId,
                movebetsday = _balance.MoveBetsDay
            };

            // Action
            await DataAccess.GetMatchesAsync<Sportsbook>(_metaModel).ConfigureAwait(false);

            // Assert
            Assert.That(param, Tobe.SameProperties(expected));
        }

        [Test]
        public async Task GetMatchesAsync_TypeIsSportsbook_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetMatchesSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);

            Reader.FieldCount.Returns(3);
            Reader.GetName(0).Returns("lcolor");
            Reader.GetName(1).Returns("LiveAwayScore");
            Reader.GetName(2).Returns("LiveHomeScore");

            var dataMapping = GetReaderDataMappingForSportsbookType();
            var dataMappingForGameDetail = GetreaderDataMappingForGameDetail();
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            DataAccessTestHelper.MapReaderData(Reader, dataMapping);

            var sportsbookData = GetExpectedSportsbookData();
            sportsbookData.BetID = "0";
            sportsbookData.Bettype = 0;
            var expectedResult = new List<Sportsbook> { sportsbookData };

            // Action
            var result = await DataAccess.GetMatchesAsync<Sportsbook>(_metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [Test]
        public void GetMatchesAsync_TypeIsNotSupported_ThrowException()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetMatchesSp)).Returns(ObjectDb);

            // Action
            AsyncTestDelegate result = () => DataAccess.GetMatchesAsync<object>(_metaModel);

            // Assert
            Assert.ThrowsAsync<NotSupportedException>(result, "The type 'object' is not supported.");
        }
    }
}