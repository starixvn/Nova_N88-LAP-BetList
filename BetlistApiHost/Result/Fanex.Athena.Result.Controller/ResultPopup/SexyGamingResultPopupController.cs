using Fanex.Result.Core.SexyGaming;
using System.Web.Mvc;

namespace Fanex.Athena.Result.Controller.ResultPopup
{
    public class SexyGamingResultPopupController : BaseResultController
    {
        private readonly ISexyGamingResultBuilder _resultApi;

        public SexyGamingResultPopupController(ISexyGamingResultBuilder resultApi)
        {
            _resultApi = resultApi;
        }

        [HttpPost]
        public string Index(string transDesc, string matchCode, string betType, string languageCode)
        {
            return _resultApi.Render(transDesc, matchCode, betType, languageCode);
        }
    }
}
