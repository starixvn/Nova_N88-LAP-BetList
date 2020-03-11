namespace Fanex.Athena.Result.Controller.UnitTests
{
    using Fanex.Result.Core.Voidbridge;
    using NSubstitute;
    using NUnit.Framework;
    using ResultPopup;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class VoidbridgeResultPopupControllerTest
    {
        [Test]
        public void Index_WhenCall_ReturnResultFromApi()
        {
            // Arrange
            var custId = FakeHelper.Create<string>();
            var gamePlayId = FakeHelper.Create<string>();
            var languageCode = FakeHelper.Create<string>();

            var resultApi = Substitute.For<VoidbridgeApi>();

            using (var controller = new VoidbridgeResultPopupController(resultApi))
            {
                // Action
                var result = controller.Index(custId, gamePlayId, languageCode);

                // Assert
                resultApi.Received().GetBetHistoryUrl(custId, gamePlayId, languageCode);
            }
        }
    }
}