using AutoMapper;
using System.Web.Http;
using TaskManagerAPI.App_Start;
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

            //For AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            container.RegisterInstance<IMapper>(config.CreateMapper());
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}