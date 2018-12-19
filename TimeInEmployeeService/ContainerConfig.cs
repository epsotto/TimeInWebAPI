using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using TimeInEmployeeService.BusinessLayer;
using TimeInEmployeeService.Interfaces;

namespace TimeInEmployeeService
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<TimeInEmployeeAttendance>().As<ITimeInEmployeeAttendance>();
            builder.RegisterType<TimeOutEmployeeAttendance>().As<ITimeOutEmployeeAttendance>();
            builder.RegisterType<TimeInEmployeeReport>().As<ITimeInEmployeeReport>();

            builder.RegisterAssemblyTypes(Assembly.Load(nameof(TimeInRepository)))
                .Where(t => t.Namespace.Contains("Utilities"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            return builder.Build();
        }
    }
}