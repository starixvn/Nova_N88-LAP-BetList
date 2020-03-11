namespace Fanex.Athena.BetList.DataAccess.UnitTests
{
    using System;
    using System.Threading.Tasks;
    using Athena.BetList.DataAccess;
    using Athena.BetList.Model.MetaModel;
    using Common.DataAccess.Interfaces;
    using Common.DataAccess.UnitTests;
    using Common.Model.DTO;
    using NSubstitute;
    using NUnit.Framework;

    public partial class BettingTicketsCenterDataAccessTest : BaseDataAccessHelperTest<BettingTicketCenterDataAccess>
    {
        private const string BetListByRefnoUspKey = "Lrf_BetList_RefNo_Mem";
        private const string RunningBetListUspKey = "Lrf_BetList_Running";
        private const string BetListBySingleCustomerUspKey = "Lrf_BetList_Mem";

        private static readonly BalanceUp BalanceUp = new BalanceUp
        {
            MoveBetsDay = new DateTime(2016, 1, 1),
            BalanceUpDay = new DateTime(2016, 2, 1)
        };

        protected override void SetUpDataAccess()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            balanceUpDataAccess.GetBalanceUp().Returns(BalanceUp);
            DataAccess = Substitute.ForPartsOf<BettingTicketCenterDataAccess>(
                balanceUpDataAccess, CurrencyDataAccess);
        }

        [Test]
        public async Task GetBetListByRefnoAsync_WhenCall_ParameterIsPassesCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(BetListByRefnoUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReaderAsync().Returns(Reader);
            Reader.Read().Returns(false);

            var metaModel = new RefNoBetListMetaModel();

            // Action
            await DataAccess.GetBetListByRefnoAsync(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("UserName", metaModel.UserName);
                ObjectDb.Received(1).SetParameterValue("RefNo", metaModel.RefNo);
                ObjectDb.Received(1).SetParameterValue("MoveBetsDay", BalanceUp.MoveBetsDay);
                ObjectDb.Received(1).SetParameterValue("BalanceUpDay", BalanceUp.BalanceUpDay);
                ObjectDb.Received(1).SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
            });
        }
    }
}