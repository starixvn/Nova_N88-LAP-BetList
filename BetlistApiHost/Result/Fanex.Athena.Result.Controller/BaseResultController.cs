namespace Fanex.Athena.Result.Controller
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.Athena.Common.Utility.Attributes;
    using Fanex.Athena.Common.Utility.Controllers;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CustomAuth]
    [HandleJsonException]
    public class BaseResultController : CultureController
    {
    }
}