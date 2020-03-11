namespace Fanex.Athena.Result.Controller.ResultPopup
{
    using System.Web.Mvc;
    using Fanex.Result.Core.AllBetCasino;

    public class AllBetResultPopupController : BaseResultController
    {
        private readonly IAllbetResultBuilder _resultApi;

        public AllBetResultPopupController(IAllbetResultBuilder resultApi)
        {
            _resultApi = resultApi;
        }

        [HttpPost]
        public string Index(string transDesc, string betCheck, string betType, string languageCode)
        {
            return _resultApi.Render(transDesc, betCheck, betType, languageCode);
        }
    }
}