namespace Fanex.Athena.Result.Controller.ResultPopup
{
    using System.Web.Mvc;
    using Fanex.Result.Core.SabaClub;

    public class SabaClubResultPopupController : BaseResultController
    {
        private readonly ISabaClubResult _resultApi;

        public SabaClubResultPopupController(ISabaClubResult resultApi)
        {
            _resultApi = resultApi;
        }

        [HttpPost]
        public string Index(string custId, string gameRoundId, string gameId, string languageCode)
        {
            return _resultApi.GetResult(custId, gameRoundId, gameId, languageCode);
        }
    }
}