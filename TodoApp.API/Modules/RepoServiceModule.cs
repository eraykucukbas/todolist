using Autofac;
using TodoApp.Core.Repositories;
using TodoApp.Core.Services;
using TodoApp.Core.UnitOfWorks;
using TodoApp.Repository;
using TodoApp.Repository.Repositories;
using TodoApp.Repository.UnitOfWorks;
using TodoApp.Service.Mapping;
using TodoApp.Service.Services;
using System.Reflection;
using TodoApp.Repository.Models;
using Module = Autofac.Module;
namespace TodoApp.API.Modules
{
    public class RepoServiceModule:Module
    {

        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();



            var apiAssembly = Assembly.GetExecutingAssembly();
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext));
            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile));

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();


            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();


           // builder.RegisterType<ProductServiceWithCaching>().As<IProductService>();

        }
    }
}
