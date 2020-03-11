using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Fanex.Athena.BetList.DataAccess.Interfaces;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.Common.Utility.Services.Interfaces;
using Fanex.Logging;
using Newtonsoft.Json.Linq;

namespace Fanex.Athena.BetList.Controller
{
    public class ColossusBetBetListController : BaseController
    {
        private readonly IAppSettings _appSettings;

        private readonly ILogger _logger;

        private readonly IColossusBetBetListDataAccess _colossusBetBetListDataAccess;

        private readonly IWebClientService _webClientService;

        public ColossusBetBetListController(
            IAppSettings appSettings,
            ILogger logger,
            IWebClientService webClientService,
            IColossusBetBetListDataAccess colossusBetBetListDataAccess)
        {
            _appSettings = appSettings;
            _logger = logger;
            _colossusBetBetListDataAccess = colossusBetBetListDataAccess;
            _webClientService = webClientService;
        }

        [HttpPost]
        public async Task<JsonResult> Detail(ColossusBetDetailMetaModel metaModel)
        {
            var viewModel = await _colossusBetBetListDataAccess.GetData(metaModel).ConfigureAwait(false);

            var iframeUrl = GetIframeAddress(metaModel.UserId, metaModel.BetCheck, metaModel.CustId);
            viewModel.IframeUrl = iframeUrl;
            return Json(viewModel);
        }

        private string GetIframeAddress(int userId, long betCheck, int memberId)
        {
            var token = GetToken(userId, memberId);
            var siteId = _colossusBetBetListDataAccess.GetSiteId(memberId);
            var url = _appSettings.GetValue("CBIframeUrlSiteId" + siteId.ToString());
            var colossusBetSiteId = _appSettings.GetValue("ColossusBetSiteId");
            return string.Format(url, betCheck, colossusBetSiteId, token);
        }

        private string GetToken(int userId, int memberId)
        {
            var tokenApiUrl = _appSettings.GetValue("ColossusBetTokenAPI");
            tokenApiUrl = string.Format(tokenApiUrl, userId, memberId);
            var json = string.Empty;
            try
            {
                json = _webClientService.DownloadString(tokenApiUrl);
            }
            catch (Exception exception)
            {
                _logger.ErrorAsync("ColossusBet Get Token Error - ApiUrl: " + tokenApiUrl, exception);
                return string.Empty;
            }

            var jobject = JObject.Parse(json);
            var token = (string)jobject.SelectToken("data.authorization");
            if (token == null)
            {
                var message = $"Errors from Colossus Bets API (memberid: {memberId}).\r\n" + jobject;
                _logger.ErrorAsync(message);
                return string.Empty;
            }

            return token;
        }
    }
}