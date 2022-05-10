namespace Fanex.Athena.Result.Controller.ResultPopup
{
    using Fanex.Athena.Result.Controller.MetaModel;
    using Fanex.Result.Core.ThirdParty;
    using Fanex.Result.Core.ThirdParty.Models;

    public class VirtualGamesResultPopupController : BaseResultController
    {
        private readonly IThirdPartyResult _resultApi;

        public VirtualGamesResultPopupController(IThirdPartyResult resultApi = null)
        {
            _resultApi = resultApi ?? new ThirdPartyResult("VirtualGamesDetailResultConfigs");
        }

        public string Index(ThirdPartyMetaModel metaModel)
        {
            var request = new ResultRequest(metaModel.CustId, metaModel.TransId3rd, metaModel.GameId3rd, metaModel.CurrencyId, metaModel.SiteId, metaModel.LanguageCode);

            return _resultApi.GetResultGET(request);
        }
    }
}