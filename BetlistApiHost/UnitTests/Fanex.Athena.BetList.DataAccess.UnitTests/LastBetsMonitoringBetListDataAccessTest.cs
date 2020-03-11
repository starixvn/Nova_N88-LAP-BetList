namespace Fanex.Athena.BetList.DataAccess.UnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Athena.BetList.DataAccess;
    using Athena.BetList.Model.DTO;
    using Athena.BetList.Model.MetaModel;
    using Common.DataAccess.UnitTests;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class LastBetsMonitoringBetListDataAccessTest : BaseDataAccessHelperTest<LastBetsMonitoringBetListDataAccess>
    {
        private const string UspKey = "Lrf_Monitor_BetlistSel_New";
        protected override void SetUpDataAccess()
        {
          
            DataAccess = Substitute.ForPartsOf<LastBetsMonitoringBetListDataAccess>(
                CurrencyDataAccess);
        }
        [Test]
        public async Task GetLastBetsMonitoringData_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(UspKey)).Returns(ObjectDb);
            var metaModel = FakeHelper.Create<LastBetsMonitoringMetaModel>();

            // Action
            await DataAccess.GetLastBetsMonitoringData(metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("BetType", metaModel.BetType);
                ObjectDb.Received(1).SetParameterValue("Currency", metaModel.Currency);
                ObjectDb.Received(1).SetParameterValue("SpecialLevel", metaModel.Level);
                ObjectDb.Received(1).SetParameterValue("TransId", metaModel.TransId);
                ObjectDb.Received(1).SetParameterValue("LimitRowCount", metaModel.PageSize);
                ObjectDb.Received(1).SetParameterValue("SportId", metaModel.SportId);
                ObjectDb.Received(1).SetParameterValue("LeagueId", metaModel.LeagueId);
                ObjectDb.Received(1).SetParameterValue("MatchId", metaModel.MatchId);
                ObjectDb.Received(1).SetParameterValue("MinStake", metaModel.StakeFrom);
                ObjectDb.Received(1).SetParameterValue("MaxStake", metaModel.StakeTo);
                ObjectDb.Received(1).SetParameterValue("LiveIndicator", metaModel.LiveStatus);
                ObjectDb.Received(1).SetParameterValue("BaseCurrency", metaModel.BasedCurrency);
                ObjectDb.Received(1).SetParameterValue("BetFrom", metaModel.BetFrom);
            });
        }

        [Test]
        public async Task GetMatchList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Any<string>()).Returns(ObjectDb);
            var expected = new BetListMatch { MatchId = 1, LeagueId = 2 };
            ObjectDb.QueryAsync<BetListMatch>().Returns(new List<BetListMatch> { expected });

            // Action
            var result = (await DataAccess.GetMatchList("SPNameTest").ConfigureAwait(false))[0];

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}