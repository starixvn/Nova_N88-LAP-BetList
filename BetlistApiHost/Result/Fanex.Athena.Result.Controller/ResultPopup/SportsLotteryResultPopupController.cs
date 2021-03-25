using Fanex.Result.Core.SportsLottery;
using System;
using System.Web.Mvc;

namespace Fanex.Athena.Result.Controller.ResultPopup
{
    public class SportsLotteryResultPopupController : BaseResultController
    {
        private readonly ISportsLotteryResult _resultApi;

        public SportsLotteryResultPopupController(ISportsLotteryResult resultApi)
        {
            _resultApi = resultApi;
        }

        [HttpPost]
        public string Index(string custId, string transId3rd, string platform, string languageCode)
        {
            var isDesktop = string.Equals(platform, "d", StringComparison.OrdinalIgnoreCase);
            return _resultApi.GetResultDetail(custId, transId3rd, isDesktop, languageCode);
        }
    }
}