using Autofac;
using BookStoreApi.Services;
using BookStoreApi.Interfaces;
using BookStoreApi.Controllers;
namespace BookStoreApi.Autofac
{
    public class AutofacModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RolesService>().As<IRoleService>();
            builder.RegisterType<UsersService>().As<IUserService>();
            builder.RegisterType<CategoryService>().As<ICategoryService>();
            builder.RegisterType<BooksService>().As<IBookService>();
            builder.RegisterType<BillsService>().As<IBillService>();
            builder.RegisterType<BillDetailService>().As<IBillDetailService>();
        }
    }
}