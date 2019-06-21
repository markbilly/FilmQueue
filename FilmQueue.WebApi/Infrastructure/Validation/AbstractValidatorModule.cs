//using Autofac;
//using FluentValidation;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace FilmQueue.WebApi.Infrastructure.Validation
//{
//    public class AbstractValidatorModule : Module
//    {
//        protected override void Load(ContainerBuilder builder)
//        {
//            builder
//                .RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
//                .Where(x => x.IsClosedTypeOf(typeof(FluentValidation.IValidator<>)))
//                .AsImplementedInterfaces()
//                .InstancePerLifetimeScope();
//        }
//    }
//}
