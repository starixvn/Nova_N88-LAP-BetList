namespace Fanex.Athena.Result.Controller.UnitTests
{
    using Fanex.Result.Core.Casino;
    using NSubstitute;
    using NUnit.Framework;
    using ResultPopup;

    [TestFixture]
    public class SGResultPopupControllerTest
    {
        [Test]
        public void Index_WhenCall_ReturnResultFromApi()
        {
            // Arrange
            const string CustId = "123";
            const string GamePlayId = "345";
            const string LanguageCode = "en-US";
            const string ExpectedResult = "SGSGSGSGS";

            var resultApi = Substitute.For<ICasinoResult>();

            resultApi.GetCasinoResult(CustId, GamePlayId, LanguageCode)
                .Returns(ExpectedResult);
            using (var controller = new SGResultPopupController(resultApi))
            {
                // Action
                var result = controller.Index(CustId, GamePlayId, LanguageCode);

                // Assert
                Assert.That(result, Is.EqualTo(ExpectedResult));
            }
        }
    }
}