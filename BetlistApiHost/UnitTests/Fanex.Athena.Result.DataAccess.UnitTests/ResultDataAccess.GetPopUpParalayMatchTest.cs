namespace Fanex.Athena.Result.DataAccess.UnitTests
{
    using System;
    using System.Threading.Tasks;
    using Fanex.Athena.UnitTest.Common.NUnitExtensions;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public partial class ResultDataAccessTest
    {
        [Test]
        public async Task GetPopUpParalayMatchAsync_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpParalayMatchSp)).Returns(ObjectDb);
            dynamic result = null;
            ObjectDb.ExecuteReaderAsync(Arg.Do<dynamic>(x => result = x));
            var refNo = 1;
            var expected = new
            {
                refno = refNo
            };

            // Action
            await DataAccess.GetPopUpParalayMatchAsync(refNo).ConfigureAwait(false);

            // Assert
            Assert.That(result, Tobe.SameProperties(expected));
        }

        [Test]
        public async Task GetPopUpParalayMatchAsync_WhenCall_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpParalayMatchSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);
            Reader.FieldCount.Returns(3);
            Reader.GetName(0).Returns("lcolor");
            Reader.GetName(1).Returns("bettype");
            Reader.GetName(2).Returns("betcheck");

            var refNo = FakeHelper.Create<long>();
            var sportId = FakeHelper.Create<int>();

            var sportsbookData = GetExpectedSportsbookData();
            sportsbookData.RefNo = refNo.ToString();
            sportsbookData.LiveHomeScore = null;
            sportsbookData.LiveAwayScore = null;

            var dataMapping = GetReaderDataMappingForSportsbookType();
            dataMapping["sporttype"] = sportId;
            dataMapping["bettype"] = sportsbookData.Bettype;
            dataMapping["betcheck"] = sportsbookData.BetID;
            var dataMappingForGameDetail = GetreaderDataMappingForGameDetail();
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            MapReaderData(dataMapping);

            // Action
            var result = await DataAccess.GetPopUpParalayMatchAsync(refNo).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0].LeagueId, Is.EqualTo(sportsbookData.LeagueId));
                Assert.That(result[0].SportId, Is.EqualTo(sportId));
                Assert.That(result[0].Sportbook[0], Tobe.SameProperties(sportsbookData));
                Assert.That(result[0].Sportbook[0].GameDetails[0], Tobe.SameProperties(sportsbookData.GameDetails[0]));
            });
        }

        [Test]
        public async Task GetPopUpParalayMatchAsync_SomeColumnsAreNotExist_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpParalayMatchSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.NextResult().Returns(false);

            var refNo = FakeHelper.Create<long>();
            var sportId = FakeHelper.Create<int>();

            var dataMapping = GetReaderDataMappingForSportsbookType();
            dataMapping["sporttype"] = sportId;

            DataAccessTestHelper.MapReaderData(Reader, dataMapping);

            var sportsbookData = GetExpectedSportsbookData();
            sportsbookData.RefNo = refNo.ToString();
            sportsbookData.Bettype = 0;
            sportsbookData.BetID = string.Empty;
            sportsbookData.Color = string.Empty;
            sportsbookData.LiveHomeScore = null;
            sportsbookData.LiveAwayScore = null;

            // Action
            var result = await DataAccess.GetPopUpParalayMatchAsync(refNo).ConfigureAwait(false);

            // Assert
            Assert.That(result[0].Sportbook[0], Tobe.SameProperties(sportsbookData));
        }

        [Test]
        public async Task GetPopUpParalayMatchAsync_SomeColumnsReturnedNullValue_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpParalayMatchSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("lcolor");

            var refNo = FakeHelper.Create<long>();
            var sportId = FakeHelper.Create<int>();

            var dataMapping = GetReaderDataMappingForSportsbookType();
            dataMapping["sporttype"] = sportId;
            dataMapping["betstatus"] = DBNull.Value;
            dataMapping["lcolor"] = DBNull.Value;

            var dataMappingForGameDetail = GetreaderDataMappingForGameDetail();
            dataMappingForGameDetail["ResultStatusID"] = DBNull.Value;
            dataMappingForGameDetail["SettlementStatusID"] = DBNull.Value;
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            DataAccessTestHelper.MapReaderData(Reader, dataMapping);

            var sportsbookData = GetExpectedSportsbookData();
            sportsbookData.RefNo = refNo.ToString();
            sportsbookData.Bettype = 0;
            sportsbookData.BetID = string.Empty;
            sportsbookData.Color = string.Empty;
            sportsbookData.BetStatus = string.Empty;
            sportsbookData.LiveHomeScore = null;
            sportsbookData.LiveAwayScore = null;
            sportsbookData.GameDetails[0].ResultStatusID = 0;
            sportsbookData.GameDetails[0].SettlementStatusID = 0;

            // Action
            var result = await DataAccess.GetPopUpParalayMatchAsync(refNo).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0].Sportbook[0], Tobe.SameProperties(sportsbookData));
                Assert.That(result[0].Sportbook[0].GameDetails[0], Tobe.SameProperties(sportsbookData.GameDetails[0]));
            });
        }

        [Test]
        public async Task GetPopUpParalayMatchAsync_ParlayItemHasMultipleSportsbookItems_ReturnCorrectLengthOfSportsbookList()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpParalayMatchSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, true, false, true, false);
            Reader.NextResult().Returns(true);

            var dataMapping = GetReaderDataMappingForSportsbookType();
            dataMapping["LeagueID"] = 11;
            dataMapping["sporttype"] = 12;
            var dataMappingForGameDetail = GetreaderDataMappingForGameDetail();
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            DataAccessTestHelper.MapReaderData(Reader, dataMapping);

            // Action
            var result = await DataAccess.GetPopUpParalayMatchAsync(FakeHelper.Create<long>()).ConfigureAwait(false);

            // Assert
            Assert.That(result[0].Sportbook.Count, Is.EqualTo(2));
        }
    }
}