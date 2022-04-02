using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using BookStoreDesktop.Autofac;
namespace BookStoreDesktop.Autofac
{
    public class AutofacInstance
    {
        public static T GetInstance<T>()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AutofacModule());
            IContainer Container = builder.Build();
            using(var scope = Container.BeginLifetimeScope())
            {
                return scope.Resolve<T>();
            }
        }
    }
}