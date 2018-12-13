using Autofac;
using LoginService.BusinessLayer;
using LoginService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace LoginService
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<LoginBusinessRules>().As<ILoginBusinessRules>();

            builder.RegisterAssemblyTypes(Assembly.Load(nameof(TimeInRepository)))
                .Where(t => t.Namespace.Contains("Utilities"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            return builder.Build();
        }
    }
}