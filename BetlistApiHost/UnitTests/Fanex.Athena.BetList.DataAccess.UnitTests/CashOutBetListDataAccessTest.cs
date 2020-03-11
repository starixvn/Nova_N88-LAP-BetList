namespace Fanex.Athena.BetList.DataAccess.UnitTests
{
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Athena.BetList.DataAccess;
    using Athena.BetList.DataAccess.Util;
    using Athena.BetList.Model.MetaModel;
    using Common.DataAccess.UnitTests;
    using Fanex.Athena.Common.DataAccess.Interfaces;
    using Fanex.Athena.Common.Model.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;
    using UnitTest.Common.NUnitExtensions;

    public class CashOutBetListDataAccessTest : BaseDataAccessHelperTest<CashOutBetListDataAccess>
    {
        private const string UspName = "Lrf_BetList_CashOut_Detail";

        private readonly BalanceUp _balanceUp = FakeHelper.Create<BalanceUp>();

        private IBalanceUpDataAccess _balanceUpDataAccess;

        protected override void SetUpDataAccess()
        {
            _balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _balanceUpDataAccess.GetBalanceUp().Returns(_balanceUp);
            DataAccess = Substitute.ForPartsOf<CashOutBetListDataAccess>(CurrencyDataAccess, _balanceUpDataAccess);
        }

        [Test]
        public async Task GetDetail_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var metaModel = FakeHelper.Create<CashOutBetListDetailMetaModel>();
            var expectedParam = new
            {
                metaModel.TransId,
                metaModel.RefNo,
                metaModel.BetType,
                OriginalTransId = metaModel.BetdaqId,
                AgentList = metaModel.AgentListXml,
                metaModel.BaseCurrency,
                _balanceUp.MoveBetsDay,
                _balanceUp.BalanceUpDay
            };

            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(ObjectDb);
            object param = null;
            await ObjectDb.ExecuteReaderAsync(Arg.Do<object>(x => param = x));

            // Action
            await DataAccess.GetDetail(metaModel).ConfigureAwait(false);

            // Assert
            Assert.That(param, Tobe.SameProperties(expectedParam));
        }

        [Test]
        public async Task GetDetail_WhenCall_OriginalTicketReturnCorrectly()
        {
            // Arrange
            const string UserName = "user1";
            const int PlayerComm = 999;

            var safeReader = Substitute.For<ISafeDataReader>();
            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(ObjectDb);
            DataAccess.GetSafeDataReader(Arg.Any<IDataReader>(), Arg.Any<string[]>()).ReturnsForAnyArgs(safeReader);

            safeReader.Read().Returns(true, false);

            safeReader.GetString("username").Returns(UserName);
            safeReader.GetDecimal("playercomm").Returns(PlayerComm);

            // Action
            var result = await DataAccess.GetDetail(new CashOutBetListDetailMetaModel()).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.OriginalTicket.UserName, Is.EqualTo(UserName));
                Assert.That(result.OriginalTicket.PlayerComm, Is.EqualTo(PlayerComm));
            });
        }

        [Test]
        public async Task GetDetail_WhenCall_CashOutTicketListReturnCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(ObjectDb);

            Reader.Read().Returns(false, true, false);
            Reader.NextResult().Returns(true);

            var dataReader = new
            {
                BetType = 1,
                Stake = 2
            };
            MapReaderData(dataReader);

            // Action
            var result = await DataAccess.GetDetail(new CashOutBetListDetailMetaModel()).ConfigureAwait(false);

            var firstCashoutTicket = result.CashOutTicketList.First();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstCashoutTicket.BetType, Is.EqualTo(dataReader.BetType));
                Assert.That(firstCashoutTicket.Stake, Is.EqualTo(dataReader.Stake));
            });
        }
    }
}