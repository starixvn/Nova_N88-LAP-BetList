namespace Fanex.Athena.Result.Service.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Common.Utility.Services.Interfaces;
    using Logging;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using UnitTest.Common.Extensions;
    using UnitTest.Common.Helpers;

    public class RNGCasinoSingleWalletServiceTest
    {
        private IAppSettings _appSettings;

        private IWebClientService _webClientService;

        private ILogger _logger;

        private RngCasinoSingleWalletService _service;

        [SetUp]
        public void SetUp()
        {
            _appSettings = Substitute.For<IAppSettings>();
            _webClientService = Substitute.For<IWebClientService>();
            _logger = Substitute.For<ILogger>();
            _service = new RngCasinoSingleWalletService(
                _logger,
                _appSettings,
                _webClientService);
        }

        [TearDown]
        public void TearDown()
        {
            _service = null;
        }

        [Test]
        public void GetResultInfo_GetTokenFail_ReturnErrorResult()
        {
            // Arrange
            var refNo = FakeHelper.Create<string>();
            var token = new
            {
                Result = new
                {
                    ErrorCode = FakeHelper.Create<int>(),
                    ErrorDescription = FakeHelper.Create<string>()
                }
            };
            SetUpForGetRngCasinoServerToken(token);

            // Action
            object result = _service.GetResultInfo(refNo);
            var resultData = result.GetValue("Result");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.GetValue("Success"), Is.False);
                Assert.That(resultData.GetValue("ErrorCode"), Is.EqualTo(token.Result.ErrorCode.ToString()));
                Assert.That(resultData.GetValue("ErrorDescription"), Is.EqualTo(token.Result.ErrorDescription.ToString()));
            });
        }

        [Test]
        public void GetResultInfo_GetErrorWhileGettingToken_ReturnErrorResultAndWriteErrorLog()
        {
            // Arrange
            var refNo = FakeHelper.Create<string>();
            var token = new
            {
                Result = new
                {
                    ErrorCode = FakeHelper.Create<int>(),
                    ErrorDescription = FakeHelper.Create<string>()
                }
            };
            SetUpForGetRngCasinoServerToken(token);

            _webClientService.UploadString(Arg.Any<Uri>(), Arg.Any<string>(), Arg.Any<IDictionary<HttpRequestHeader, string>>())
                .Returns(x => { throw new Exception(); });

            // Action
            object result = _service.GetResultInfo(refNo);
            var resultData = result.GetValue("Result");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.GetValue("Success"), Is.False);
                Assert.That(resultData.GetValue("ErrorCode"), Is.EqualTo(1));
                Assert.That(resultData.GetValue("ErrorDescription"), Is.EqualTo(ViewResources.ErrorProcessingMsg));
            });
        }

        [Test]
        public void GetResultInfo_GetTokenSuccessfully_ReturnCorrectResult()
        {
            // Arrange
            var refNo = FakeHelper.Create<string>();
            var token = new
            {
                Result = new
                {
                    ErrorCode = 0,
                    Token = FakeHelper.Create<string>()
                }
            };
            SetUpForGetRngCasinoServerToken(token);
            SetUpForGetRngCasinoPopupUrl();

            // Action
            var result = _service.GetResultInfo(refNo);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.GetType().GetProperty("Success").GetValue(result, null), Is.True);
                StringAssert.Contains("RNGCasinoResultPopupUrl", result.GetType().GetProperty("Result").GetValue(result, null));
            });
        }

        private void SetUpForGetRngCasinoServerToken(object token)
        {
            var tokenString = ObjectHelper.ToJson(token);

            _appSettings.GetValue("RNGCasinoGetServerTokenAPIUrl").Returns("http://RngCasinoServerToken");
            _webClientService.UploadString(Arg.Any<Uri>(), Arg.Any<string>(), Arg.Any<IDictionary<HttpRequestHeader, string>>())
                .Returns(tokenString);

            // CreateServerTokenParameters
            _appSettings.GetValue("RNGCasinoLogin").Returns(FakeHelper.Create<string>());
            _appSettings.GetValue("RNGCasinoPassword").Returns(FakeHelper.Create<string>());
            _appSettings.GetValue("RNGCasinoSiteId").Returns(FakeHelper.Create<int>().ToString());
            _appSettings.GetValue("RNGCasinoSeq").Returns(FakeHelper.Create<string>());
            _appSettings.GetValue("RNGCasinoVendorId").Returns(FakeHelper.Create<int>().ToString());
        }

        private void SetUpForGetRngCasinoPopupUrl()
        {
            // GetQueryString
            _appSettings.GetValue("RNGCasinoTemplate").Returns(FakeHelper.Create<string>());
            _appSettings.GetValue("RNGCasinoRole").Returns(FakeHelper.Create<string>());

            _appSettings.GetValue("RNGCasinoEncryptedKey").Returns(FakeHelper.Create<string>());
            _appSettings.GetValue("RNGCasinoResultPopupUrl").Returns("RNGCasinoResultPopupUrl");
        }
    }
}