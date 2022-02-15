using Fanex.Athena.Result.Controller.MetaModel;
using Fanex.Result.Core.ThirdParty;
using Fanex.Result.Core.ThirdParty.Models;
using System.Web.Mvc;

namespace Fanex.Athena.Result.Controller.ResultPopup
{
    public class SportsLotteryResultPopupController : BaseResultController
    {
        private readonly IThirdPartyResult _thirdPartyResultApi;

        public SportsLotteryResultPopupController(IThirdPartyResult thirdPartyResultApi = null)
        {
            _thirdPartyResultApi = thirdPartyResultApi ?? new ThirdPartyResult("SportsLotteryDetailResultConfigs");
        }

        [HttpPost]
        public string Index(ThirdPartyMetaModel metaModel)
        {
            const string defaultPlatform = "D";

            var request = new ResultRequest(metaModel.CustId, metaModel.TransId3rd, metaModel.GameId3rd, metaModel.CurrencyId, metaModel.SiteId, metaModel.LanguageCode)
            {
                Platform = defaultPlatform,
                BetTypeId = metaModel.BetTypeId
            };

            return _thirdPartyResultApi.GetResultGET(request);
        }
    }
}