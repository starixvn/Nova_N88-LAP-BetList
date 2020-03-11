namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System;
    using System.Threading.Tasks;
    using Common.Utility.Services.Interfaces;
    using DataAccess.Interfaces;
    using Logging;
    using Model.MetaModel;
    using Model.ViewModel;
    using Newtonsoft.Json.Linq;
    using NSubstitute;
    using NUnit.Framework;

    public class ColossusBetBetListControllerTest
    {
        private IAppSettings _appSettings;

        private ILogger _logger;

        private IColossusBetBetListDataAccess _colossusBetBetListDataAccess;

        private IWebClientService _webClientService;

        [SetUp]
        public void SetUp()
        {
            _appSettings = Substitute.For<IAppSettings>();
            _logger = Substitute.For<ILogger>();
            _colossusBetBetListDataAccess = Substitute.For<IColossusBetBetListDataAccess>();
            _webClientService = Substitute.For<IWebClientService>();
        }

        [TearDown]
        public void TearDown()
        {
            _appSettings = null;
            _logger = null;
            _colossusBetBetListDataAccess = null;
            _webClientService = null;
        }

        [Test]
        public async Task Detail_WhenCall_ReturnCorrectData()
        {
            using (var controller = CreateController())
            {
                // Arrang
                var metaModel = new ColossusBetDetailMetaModel();

                var viewModel = new ColossusBetDetailViewModel();
                _colossusBetBetListDataAccess.GetData(metaModel).Returns(viewModel);

                var expectedIframeUrl = SetUpForGetIframeUrl(metaModel);

                // Action
                var result = (ColossusBetDetailViewModel)(await controller.Detail(metaModel)).Data;

                // Assert
                Assert.That(viewModel.IframeUrl, Is.EqualTo(expectedIframeUrl));
            }
        }

        [Test]
        public async Task Detail_GetTokenThrowException_LogError()
        {
            using (var controller = CreateController())
            {
                // Arrang
                var exception = new Exception();
                var metaModel = new ColossusBetDetailMetaModel();

                var viewModel = new ColossusBetDetailViewModel();
                _colossusBetBetListDataAccess.GetData(metaModel).Returns(viewModel);

                var tokenApiUrl = "{0} {1}";
                _appSettings.GetValue("ColossusBetTokenAPI").Returns(tokenApiUrl);

                _webClientService.When(x => x.DownloadString(Arg.Any<string>())).Throw(exception);

                // Action
                await controller.Detail(metaModel);

                // Assert
                await _logger.Received(1).ErrorAsync("ColossusBet Get Token Error - ApiUrl: " + string.Format(tokenApiUrl, metaModel.UserId, metaModel.CustId), exception);
            }
        }

        [Test]
        public async Task Detail_TokenIsNull_LogError()
        {
            using (var controller = CreateController())
            {
                // Arrang
                var exception = new Exception();
                var metaModel = new ColossusBetDetailMetaModel();

                var viewModel = new ColossusBetDetailViewModel();
                _colossusBetBetListDataAccess.GetData(metaModel).Returns(viewModel);

                var tokenApiUrl = "{0} {1}";
                _appSettings.GetValue("ColossusBetTokenAPI").Returns(tokenApiUrl);

                var tokenData = "{'data':{}}";
                _webClientService.DownloadString(Arg.Any<string>()).Returns(tokenData);

                // Action
                await controller.Detail(metaModel);

                // Assert
                await _logger.Received(1).ErrorAsync($"Errors from Colossus Bets API (memberid: { metaModel.CustId}).\r\n" + JObject.Parse(tokenData));
            }
        }

        private string SetUpForGetIframeUrl(ColossusBetDetailMetaModel metaModel)
        {
            const int SiteId = 1;
            const string Url = "{0} {1} {2}";
            const string BetSiteId = "Colossus";
            const string Api = "API{0}{1}";
            const string Token = "token";

            _colossusBetBetListDataAccess.GetSiteId(metaModel.CustId).Returns(SiteId);

            _appSettings.GetValue("CBIframeUrlSiteId1").Returns(Url);
            _appSettings.GetValue("ColossusBetSiteId").Returns(BetSiteId);
            _appSettings.GetValue("ColossusBetTokenAPI").Returns(Api);

            _webClientService.DownloadString(Arg.Any<string>()).Returns("{'data':{'authorization':'token'}}");

            return string.Format(Url, metaModel.BetCheck, BetSiteId, Token);
        }

        private ColossusBetBetListController CreateController()
        {
            return new ColossusBetBetListController(
                _appSettings,
                _logger,
                _webClientService,
                _colossusBetBetListDataAccess);
        }
    }
}