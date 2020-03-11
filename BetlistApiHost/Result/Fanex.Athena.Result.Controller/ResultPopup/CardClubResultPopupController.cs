namespace Fanex.Athena.Result.Controller.ResultPopup
{
    using System.Web.Mvc;
    using Fanex.Result.Core.PokerGames;

    public class CardClubResultPopupController : BaseResultController
    {
        private readonly IPokerGamesResult _resultApi;

        public CardClubResultPopupController(IPokerGamesResult resultApi)
        {
            _resultApi = resultApi;
        }

        [HttpPost]
        public string Index(string custId, string gameTypeId, string transId3rd, string languageCode)
        {
            return _resultApi.GetResult(custId, gameTypeId, transId3rd, Fanex.Result.Core.PokerGames.Models.ViewerLevel.Member, languageCode);
        }
    }
}