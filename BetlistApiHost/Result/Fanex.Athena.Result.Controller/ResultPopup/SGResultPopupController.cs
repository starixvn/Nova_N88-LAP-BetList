namespace Fanex.Athena.Result.Controller.ResultPopup
{
    using Autofac.Features.AttributeFilters;
    using Fanex.Result.Core.Casino;

    public class SGResultPopupController : RNGResultPopupBaseController
    {
        public SGResultPopupController([KeyFilter("SG")] ICasinoResult resultApi) : base(resultApi)
        {
        }
    }
}