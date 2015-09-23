using Microsoft.Practices.Unity;
using System.Web.Http;
using EventTracker.BusinessServices;
using EventTracker.Resolver;
using Unity.WebApi;

namespace EventTrackerAPI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            //AT: This is registration if not using MEF
            // e.g. container.RegisterType<ITestService, TestService>();
            //container.RegisterType<IEventServices, EventServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager()); ;

            //AT: Registration if using MEF
            ComponentLoader.LoadContainer(container, ".\\bin", "EventTrackerAPI.dll");
            ComponentLoader.LoadContainer(container, ".\\bin", "EventTracker.*.dll");

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}