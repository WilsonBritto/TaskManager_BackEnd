using System.Web.Http;
using TaskManagerAPI.Core;
using TaskManagerAPI.Persisitance;
using Unity;
using Unity.WebApi;

namespace TaskManagerAPI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IUnitOfWork, UnitOfWork>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}