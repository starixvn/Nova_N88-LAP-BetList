namespace Fanex.Athena.Common.Utility.Controllers
{
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class CultureController : Controller
    {
        private const string LANGUAGEKEY = "Language";

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var languageCode = requestContext.HttpContext.Request.Headers[LANGUAGEKEY];
            if (!string.IsNullOrWhiteSpace(languageCode))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCode);
            }
        }
    }
}
