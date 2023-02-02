namespace Fanex.Athena.Result.Controller.UnitTests
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Common.Utility.Services.Interfaces;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.Sportsbook.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Interfaces;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public partial class ResultReportControllerTest
    {
        private const string LanguageCode = "vi-VN";

        private const string HtmlResult = "<abc>hello</abc>";

        private IResultService _resultService;

        [SetUp]
        public void SetUp()
        {
            _resultService = Substitute.For<IResultService>();
        }

        [TearDown]
        public void TearDown()
        {
            _resultService.ClearReceivedCalls();
        }

        [Test]
        public async Task ResultDetail_DataIsNull_ReturnEmptyResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int MatchId = 23;
                const int SportType = 45445;
                _resultService.GetSportsbookGameDetailAsync(MatchId).Returns(x => Task.FromResult<Sportsbook>(null));

                // Action
                var result = await controller.ResultDetail(MatchId, SportType, LanguageCode).ConfigureAwait(false);

                // Assert
                Assert.That(result, Is.Empty);
            }
        }

        [Test]
        public async Task ResultDetail_WhenCall_ReturnCorrectResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string ExpectedHtmlGameDetail = "Abc";
                const int MatchId = 1;
                const int SportId = (int)SportType.Soccer;
                var detailedData = new Sportsbook
                {
                    MatchId = MatchId
                };
                _resultService.GetSportsbookGameDetailAsync(MatchId).Returns(detailedData);
                _resultService.HtmlGameDetailParser(detailedData, Arg.Any<ResultSetting>()).Returns(ExpectedHtmlGameDetail);

                // Action
                var result = await controller.ResultDetail(MatchId, SportId, LanguageCode).ConfigureAwait(false);

                // Assert
                Assert.Multiple(() =>
                {
                    _resultService.Received(1).HtmlGameDetailParser(
                        detailedData,
                        Arg.Is<ResultSetting>(x =>
                            x.SportType == (SportType)SportId &&
                            x.LanguageCode == LanguageCode));
                    Assert.That(result, Is.EqualTo(ExpectedHtmlGameDetail));
                });
            }
        }

        private ResultReportController CreateController()
        {
            var controller = new ResultReportController(
                _resultService,
                Substitute.For<ICacheRefAdapter>());
            controller.ControllerContext = MvcHelper.CreateControllerContext();
            controller.Url = MvcHelper.CreateUrlHelper(controller.ControllerContext.HttpContext);
            return controller;
        }
    }
}