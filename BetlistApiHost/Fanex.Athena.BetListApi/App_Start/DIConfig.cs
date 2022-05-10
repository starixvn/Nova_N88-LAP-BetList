namespace Fanex.Athena
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web.Compilation;
    using System.Web.Mvc;
    using Autofac;
    using Autofac.Features.AttributeFilters;
    using Autofac.Integration.Mvc;
    using Fanex.Athena.Common.Utility.Services.Internal;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Logging;
    using Fanex.RemotingRef;
    using Fanex.Result.Core.AllBetCasino;
    using Fanex.Result.Core.SabaClub;
    using Fanex.Result.Core.SexyGaming;
    using Fanex.Result.Core.VGaming;
    using Fanex.Athena.BetList.Service.Interfaces;
    using Fanex.Athena.BetList.Service;

    public static class DIConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            var assemblies = GetAssemblies();
            var controllerAss = GetControllerAssemblies(assemblies);
            builder.RegisterAssemblyTypes(assemblies).AsImplementedInterfaces();
            builder.RegisterFilterProvider();

            RegisterThirdpartyServices(builder);

            // static services
            builder.RegisterInstance(new CustomMemoryCache()).As<ICustomMemoryCache>();
            builder.RegisterType<IPService>().As<IIPService>().SingleInstance();

            builder.RegisterControllers(controllerAss).WithAttributeFiltering();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void RegisterThirdpartyServices(ContainerBuilder builder)
        {
            builder.RegisterType<TicketHelper>().As<ITicketHelper>().InstancePerRequest();
            builder.RegisterType<BetListHTMLBuilder>().As<IBetListHTMLBuilder>().InstancePerRequest();
            builder.RegisterType<BetListExcelBuilder>().As<IBetListExcelBuilder>().InstancePerRequest();
            builder.RegisterType<AllbetResultBuilder>().As<IAllbetResultBuilder>();
            builder.RegisterType<SabaClubResult>().As<ISabaClubResult>();
            builder.RegisterType<RemotingRefService>().As<IRemotingRefService>().SingleInstance();
            builder.RegisterInstance(Logger.Log).As<Logging.ILogger>();
            builder.RegisterType<SexyGamingResultBuilder>().As<ISexyGamingResultBuilder>();
            builder.RegisterType<VGamingResult>().As<IVGamingResult>();
        }

        private static Assembly[] GetAssemblies()
        {
            Assembly[] assemblies = null;
            try
            {
                assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray();
            }
            catch
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(i => !i.IsDynamic).ToArray();
            }

            return assemblies.Where(x => x.FullName.StartsWith("Fanex.Athena", StringComparison.InvariantCulture)).ToArray();
        }

        private static Assembly[] GetControllerAssemblies(Assembly[] assemblies)
        {
            return assemblies.Where(ass =>
                ass.FullName.Contains("Fanex.Athena.BetList.Controller")
                || ass.FullName.Contains("Fanex.Athena.Result.Controller")).ToArray();
        }
    }
}