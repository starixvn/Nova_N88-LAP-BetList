namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using Model.DTO;
    using Model.MetaModel;

    using NSubstitute;

    using NUnit.Framework;

    using Service.Interfaces;

    using UnitTest.Common.Extensions;
    using UnitTest.Common.Helpers;

    public class BetListByProductControllerTest : BaseControllerHelperTest
    {
        private IBetListByProductService _service;

        private ITicketBuilderService _ticketBuilderService;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            _service = Substitute.For<IBetListByProductService>();
            _ticketBuilderService = Substitute.For<ITicketBuilderService>();
        }

        [OneTimeTearDown]
        public void FixtureTearDown()
        {
            _ticketBuilderService = null;
            _service = null;
        }


        [Test]
        public async Task Index_WhenCall_QueryAndReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<BetListByProductMetaModel>();

                var ticketResult = new TicketResult();
                _service.QueryAsync(metaModel).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2).ToList();
                _ticketBuilderService.BuildTicketText(ticketResult, metaModel, metaModel.HasPlayerCom).Returns(ticketText);

                // Action
                var result = (await controller.Index(metaModel) as JsonResult).Data;
                var data = result.GetValue("Data");

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(data, Is.EqualTo(ticketText));
                });
            }
        }

        private BetListByProductController CreateController()
        {
            return new BetListByProductController(
                _service,
                _ticketBuilderService);
        }
    }
}