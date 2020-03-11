// --------------------------------------------------------------------------
// <copyright file="BaseApiController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.BetList.Controller
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Common.Utility.Attributes;
    using Fanex.Athena.BetList.Controller.CustomActionResult;
    using Fanex.Athena.Common.Utility.Controllers;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CustomAuth]
    [HandleJsonException]
    public class BaseController : CultureController
    {
        protected new JsonResult Json(object data)
        {
            return new JsonNetResult
            {
                Data = data,
                MaxJsonLength = int.MaxValue
            };
        }
    }
}