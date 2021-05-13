namespace Fanex.Athena.Result.Controller.ResultPopup
{
    using Fanex.Result.Core.CardClub;
    using System.Web.Mvc;

    public class CardClubResultPopupController : BaseResultController
    {
        private readonly ICardClubResult _resultApi;

        public CardClubResultPopupController(ICardClubResult resultApi)
        {
            _resultApi = resultApi;
        }

        [HttpPost]
        public string Index(string custId, string gameTypeId, string transId3rd, string languageCode)
        {
            return _resultApi.GetResult(custId, gameTypeId, transId3rd, languageCode);
        }
    }
}