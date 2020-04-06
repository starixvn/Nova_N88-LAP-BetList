namespace Fanex.Athena.Result.Controller.ResultPopup
{
    using System.Web.Mvc;
    using Fanex.Result.Core.VirtualGames;

    public class VirtualGamesResultPopupController : BaseResultController
    {
        private readonly IVirtualGamesResult _resultApi;

        public VirtualGamesResultPopupController(IVirtualGamesResult resultApi)
        {
            _resultApi = resultApi;
        }

        [HttpPost]
        public string GetDetailResult(string gamePlayId, string languageCode, string platform = "d")
        {
            return _resultApi.GetDetailResult(gamePlayId, platform, languageCode);
        }

        [HttpPost]
        public string GetResultPage(string languageCode, string platform = "d")
        {
            return _resultApi.GetResult(platform, languageCode);
        }
    }
}