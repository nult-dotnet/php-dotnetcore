using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using BookStoreDesktop.Interfaces;
using BookStoreDesktop.Services;
using AutoMapper;
using BookStoreDesktop.Interfaces.Services;

namespace BookStoreDesktop.Autofac
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Services
            builder.RegisterType<CategoryService>().As<ICategoryService>();
            builder.RegisterType<RoleService>().As<IRoleService>();
            builder.RegisterType<BookService>().As<IBookService>();
        }
    }
}
