namespace Fanex.Athena.Result.Controller.ResultPopup
{
    using System.Web.Mvc;
    using Fanex.Result.Core.GlobalGaming;
    using Fanex.Result.Core.GlobalGaming.DTO;

    public class GlobalGamingResultPopupController : BaseResultController
    {
        [HttpPost]
        public string Index(GlobalGamingDTO param)
        {
            var globalGamingResultApi = new GlobalGamingResultApi();

            return globalGamingResultApi.GetResultUrl(param);
        }
    }
}