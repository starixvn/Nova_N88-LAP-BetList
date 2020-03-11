namespace Fanex.Athena.Result.Controller.UnitTests
{
    using Fanex.Result.Core.PokerGames;
    using NSubstitute;
    using NUnit.Framework;
    using ResultPopup;

    [TestFixture]
    public class CardClubResultPopupControllerTest
    {
        [Test]
        public void Index_WhenCall_ReturnResultFromApi()
        {
            // Arrange
            const string CustId = "123";
            const string GameTypeId = "345";
            const string TransId3rd = "789";
            const string LanguageCode = "en-US";
            const string ExpectedResult = "aaaaaaaaaaaa";

            var resultApi = Substitute.For<IPokerGamesResult>();

            resultApi.GetResult(CustId, GameTypeId, TransId3rd, Fanex.Result.Core.PokerGames.Models.ViewerLevel.Member, LanguageCode)
                .Returns(ExpectedResult);
            using (var controller = new CardClubResultPopupController(resultApi))
            {
                // Action
                var result = controller.Index(CustId, GameTypeId, TransId3rd, LanguageCode);

                // Assert
                Assert.That(result, Is.EqualTo(ExpectedResult));
            }
        }
    }
}