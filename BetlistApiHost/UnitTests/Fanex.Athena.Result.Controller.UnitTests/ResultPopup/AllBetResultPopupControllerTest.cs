namespace Fanex.Athena.Result.Controller.UnitTests
{
    using NSubstitute;
    using NUnit.Framework;
    using ResultPopup;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class AllBetResultPopupControllerTest
    {
        [Test]
        public void Index_WhenCall_ReturnResultFromApi()
        {
            // Arrange
            var transDesc = FakeHelper.Create<string>();
            var betCheck = FakeHelper.Create<string>();
            var betType = FakeHelper.Create<string>();
            var languageCode = FakeHelper.Create<string>();

            var resultApi = Substitute.For<Fanex.Result.Core.AllBetCasino.IAllbetResultBuilder>();

            using (var controller = new AllBetResultPopupController(resultApi))
            {
                // Action
                var result = controller.Index(transDesc, betCheck, betType, languageCode);

                // Assert
                resultApi.Received().Render(transDesc, betCheck, betType, languageCode);
            }
        }
    }
}