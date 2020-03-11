namespace Fanex.Athena.Result.Controller.UnitTests
{
    using Fanex.Result.Core.PP;
    using NSubstitute;
    using NUnit.Framework;
    using ResultPopup;

    [TestFixture]
    public class PPResultPopupControllerTest
    {
        [Test]
        public void Index_WhenCall_ReturnResultFromApi()
        {
            // Arrange
            const string CustId = "123";
            const string GamePlayId = "345";
            const string LanguageCode = "en-US";
            const string ExpectedResult = "aaaaaaaaaaaa";

            var resultApi = Substitute.For<IPPResult>();

            resultApi.GetPPResult(CustId, GamePlayId, LanguageCode)
                .Returns(ExpectedResult);
            using (var controller = new PPResultPopupController(resultApi))
            {
                // Action
                var result = controller.Index(CustId, GamePlayId, LanguageCode);

                // Assert
                Assert.That(result, Is.EqualTo(ExpectedResult));
            }
        }
    }
}