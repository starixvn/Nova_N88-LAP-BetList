namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System.Threading.Tasks;
    using DataAccess.Interfaces;
    using Model.DTO;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Extensions;
    using UnitTest.Common.Helpers;

    public class OutstandingDailyStatementBetListControllerTest
    {
        private IDailyStatementBetListDataAccess _dailyStatementBetListDataAccess;

        [SetUp]
        public void SetUp()
        {
            _dailyStatementBetListDataAccess = Substitute.For<IDailyStatementBetListDataAccess>();
        }

        [TearDown]
        public void TearDown()
        {
            _dailyStatementBetListDataAccess = null;
        }

        [Test]
        public async Task GetData_WhenCall_ReturnCorrectData()
        {
            using (var controller = new OutstandingDailyStatementBetListController(_dailyStatementBetListDataAccess))
            {
                // Arrange
                var metaModel = FakeHelper.Create<BetListMetaModel>();

                var dataList = FakeHelper.CreateMany<DailyStatement>(2);
                _dailyStatementBetListDataAccess.GetDailyStatementData(metaModel).Returns(dataList);

                // Action
                var result = (await controller.GetData(metaModel)).Data;
                var main = result.GetValue("Main");
                var total = result.GetValue("Total");

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(main, Is.EqualTo(dataList));
                    Assert.That(total.GetValue("SumWinLoss"), Is.Zero);
                    Assert.That(total.GetValue("SumLicenseeWl"), Is.Zero);
                });
            }
        }
    }
}