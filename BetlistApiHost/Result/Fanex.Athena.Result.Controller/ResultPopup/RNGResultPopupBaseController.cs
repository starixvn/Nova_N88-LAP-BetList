namespace Fanex.Athena.Result.Controller.ResultPopup
{
    using System.Web.Mvc;
    using Fanex.Result.Core.Casino;

    public class RNGResultPopupBaseController : BaseResultController
    {
        private readonly ICasinoResult _resultApi;

        protected RNGResultPopupBaseController(ICasinoResult resultApi)
        {
            _resultApi = resultApi;
        }

        [HttpPost]
        public string Index(string custId, string gamePlayId, string languageCode)
        {
            return _resultApi.GetCasinoResult(custId, gamePlayId, languageCode);
        }
    }
}