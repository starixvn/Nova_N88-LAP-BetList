namespace Fanex.Athena.Result.Controller.ResultPopup
{
    using System.Web.Mvc;
    using Fanex.Athena.Result.Controller.MetaModel;
    using Fanex.Result.Core.ThirdParty.Models;
    using Fanex.Result.Core.ThirdParty;

    public class VGamingResultPopupController : BaseResultController
    {
        private readonly IThirdPartyResult _resultApi;

        public VGamingResultPopupController(IThirdPartyResult resultApi = null)
        {
            _resultApi = resultApi ?? new ThirdPartyResult("VGamingConfigs");
        }

        [HttpPost]
        public string Index(ThirdPartyMetaModel metaModel)
        {
            var request = new ResultRequest(metaModel.CustId, metaModel.TransId3rd, metaModel.GameId3rd, metaModel.CurrencyId, metaModel.SiteId, metaModel.LanguageCode)
            {
                BetTypeId = metaModel.BetTypeId
            };

            return _resultApi.GetResultGET(request);
        }
    }
}