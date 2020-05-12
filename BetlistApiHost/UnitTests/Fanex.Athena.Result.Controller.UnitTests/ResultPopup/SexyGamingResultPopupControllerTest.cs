namespace Fanex.Athena.Result.Controller.UnitTests
{
    using Fanex.Result.Core.SexyGaming;
    using NSubstitute;
    using NUnit.Framework;
    using ResultPopup;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class SexyGamingResultPopupControllerTest
    {
        [Test]
        public void Index_WhenCall_ReturnResultFromApi()
        {
            // Arrange
            var transDesc = FakeHelper.Create<string>();
            var betCheck = FakeHelper.Create<string>();
            var betType = FakeHelper.Create<string>();
            var languageCode = FakeHelper.Create<string>();

            var resultApi = Substitute.For<ISexyGamingResultBuilder>();

            using (var controller = new SexyGamingResultPopupController(resultApi))
            {
                // Action
                var result = controller.Index(transDesc, betCheck, betType, languageCode);

                // Assert
                resultApi.Received().Render(transDesc, betCheck, betType, languageCode);
            }
        }
    }
}