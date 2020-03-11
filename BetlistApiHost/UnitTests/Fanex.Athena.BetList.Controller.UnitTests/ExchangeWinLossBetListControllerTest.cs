namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Interfaces;
    using System.Threading.Tasks;
    using UnitTest.Common.Helpers;

    public class ExchangeWinLossBetListControllerTest
    {
        private IExchangeBetListService _service;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            _service = Substitute.For<IExchangeBetListService>();
        }

        [OneTimeTearDown]
        public void FixtureTearDown()
        {
            _service = Substitute.For<IExchangeBetListService>();
        }

        [Test]
        public async Task GetData_WhenCall_CallServiceToGetData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<ExchangeBetListMetaModel>();

                // Action
                await controller.GetData(metaModel).ConfigureAwait(false);

                // Assert
                await _service.Received(1).GetBetListDataAsync(metaModel);
            }
        }

        [Test]
        public async Task Detail_WhenCall_CallServiceToGetData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<ExchangeBetListDetailMetaModel>();

                // Action
                await controller.Detail(metaModel).ConfigureAwait(false);

                // Assert
                await _service.Received().GetTicketDetailListAsync(metaModel);
            }
        }

        private ExchangeWinLossBetListController CreateController()
        {
            return new ExchangeWinLossBetListController(_service);
        }
    }
}