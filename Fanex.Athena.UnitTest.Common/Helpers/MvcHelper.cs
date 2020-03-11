using System;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NSubstitute;

namespace Fanex.Athena.UnitTest.Common.Helpers
{
    public static class MvcHelper
    {
        public const string TestingRequestDomain = "http://unit-test-mode";

        public const string IdentityName = "user";

        public static ControllerContext CreateControllerContext(NameValueCollection header = null)
        {
            var fakeIdentity = new GenericIdentity(IdentityName);
            var principal = new GenericPrincipal(fakeIdentity, null);

            var httpContextBase = Substitute.For<HttpContextBase>();
            httpContextBase.Request.Headers.Returns(header ?? new NameValueCollection());
            httpContextBase.Request.ApplicationPath.Returns("/");
            httpContextBase.Request.Url.Returns(new Uri(TestingRequestDomain));
            httpContextBase.Request.Cookies.Returns(new HttpCookieCollection());
            httpContextBase.Response.Cookies.Returns(new HttpCookieCollection());
            httpContextBase.Server.MapPath(Arg.Any<string>()).Returns(x => (string)x[0]);
            httpContextBase.User.Returns(principal);

            var controllerContext = Substitute.For<ControllerContext>();
            controllerContext.HttpContext = httpContextBase;

            return controllerContext;
        }

        public static UrlHelper CreateUrlHelper(HttpContextBase httpContextBase)
        {
            var requestContext = new RequestContext(httpContextBase, new RouteData());
            return new UrlHelper(requestContext, new RouteCollection());
        }
    }
}