using System;
using System.Web.Mvc;
using Fanex.Result.Core.LinkedGames;

namespace Fanex.Athena.Result.Controller.ResultPopup
{
    public class Happy5LinkedGamesResultPopupController : BaseResultController
    {
        private readonly ILinkedGamesResult _resultApi;

        public Happy5LinkedGamesResultPopupController(ILinkedGamesResult resultApi)
        {
            _resultApi = resultApi;
        }

        [HttpPost]
        public string Index(string custId, string transId3rd, string platform, string languageCode)
        {
            var isDesktop = string.Equals(platform, "d", StringComparison.OrdinalIgnoreCase);
            return _resultApi.GetResult(custId, transId3rd, isDesktop, languageCode);
        }
    }
}
