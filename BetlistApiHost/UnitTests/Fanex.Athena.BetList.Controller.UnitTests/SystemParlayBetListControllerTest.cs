namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.BetList.Core.Entities;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Interfaces;
    using UnitTest.Common.Helpers;

    public class SystemParlayBetListControllerTest
    {
        [Test]
        public async Task Detail_WhenCall_CallCorrectMethod()
        {
            var ticketHelper = Substitute.For<ITicketHelper>();
            var systemParlayDetailService = Substitute.For<ISystemParlayDetailService>();

            using (var controller = new SystemParlayBetListController(ticketHelper, systemParlayDetailService))
            {
                // Arrange
                const string ExpectedResultContent = "System parlay detail result content";
                const string ExpectedViewPath = "ViewPath";
                var server = Substitute.For<HttpServerUtilityBase>();
                server.MapPath("~/Views/SystemParlayDetail/Index.cshtml").Returns(ExpectedViewPath);
                var controllerContext = Substitute.For<ControllerContext>();
                controllerContext.HttpContext.Server.Returns(server);
                controller.ControllerContext = controllerContext;

                var metaModel = FakeHelper.Create<SystemParlayDetailMetaModel>();

                var systemParlayData = new SystemParlayData();
                systemParlayDetailService.BuildHtmlContent(metaModel, ticketHelper, ExpectedViewPath).Returns(ExpectedResultContent);

                // Action
                var result = await controller.Detail(metaModel);

                // Assert
                Assert.That(result, Is.EqualTo(ExpectedResultContent));
            }
        }
    }
}