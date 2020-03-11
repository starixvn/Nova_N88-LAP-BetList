// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena
{
    using System;
    using System.Configuration;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using App_Start;
    using Data;
    using Fanex.Athena.Common.Utility.Helpers;
    using Fanex.BetList.Core;
    using Fanex.BetList.Core.RemotingRef;
    using Fanex.Data.SqlServer;
    using Fanex.Logging.Sentry.RavenClient;
    using Fanex.RemotingRef;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.RemotingRef;
    using Logging;
    using SharpRaven;

    public class MvcApplication : HttpApplication
    {
        public void Application_Error(object sender, EventArgs e)
        {
            //// Code that runs when an unhandled error occurs
            var exception = Server.GetLastError();
            var isIgnore = exception is HttpAntiForgeryException
               || exception is HttpException
            || (exception.TargetSite.DeclaringType == typeof(ActionDescriptor) &&
                 exception.TargetSite.Name == "ExtractParameterFromDictionary");
            //// Exception when passing wrong data type to action method
            if (!isIgnore)
            {
                LogService.Error(LogMessageHelper.GenerateMessage(exception));
            }
        }

        public void Application_Start(object sender, EventArgs e)
        {
            LogManager
                .SetDefaultLogCategory(ConfigurationManager.AppSettings["Fanex.Logging-LogCategory"])
                .Use(new SentryLogging(
                new SentryEngineOptions
                {
                    Dsn = new Dsn(ConfigurationManager.AppSettings["Sentry-Dsn"])
                }));

            const string SpNameConfigSource = "~/SpNameConfig/FanexSpName.xml";

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            AreaRegistration.RegisterAllAreas();

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            DbSettingProviderManager
                .StartNewSession()
                .WithSqlServer(resourcePath: SpNameConfigSource)
                .Run();
            RefDataTypeContainer
           .ConnectTo(new Uri(ConfigurationManager.AppSettings["Fanex.RemotingRef.Service.Url"]))
           .FromApplication("Fanex.LAP.BetListApi");
            BetListManager.RegisterRemotingRef(new CacheRef());
            ResultCoreManager.RegisterRefDataHelper(new RefDataHelper());

            DIConfig.Register();
        }
    }
}