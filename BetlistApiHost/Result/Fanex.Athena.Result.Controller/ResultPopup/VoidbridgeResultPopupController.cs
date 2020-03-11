namespace Fanex.Athena.Result.Controller.ResultPopup
{
    using System.Web.Mvc;
    using Fanex.Result.Core.Voidbridge;

    public class VoidbridgeResultPopupController : BaseResultController
    {
        private readonly VoidbridgeApi _resultApi;

        public VoidbridgeResultPopupController(VoidbridgeApi resultApi)
        {
            _resultApi = resultApi;
        }

        [HttpPost]
        public string Index(string custId, string gamePlayId, string languageCode)
        {
            return _resultApi.GetBetHistoryUrl(custId, gamePlayId, languageCode);
        }
    }
}