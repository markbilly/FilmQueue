using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.Autofac
{
    public class DependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
                .AssignableTo<IDependency>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
