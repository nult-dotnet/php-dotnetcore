using AutoMapper;
using BookStoreApi.Models;
namespace BookStoreApi.Mappers
{
    public class BookStoreApiMapper : Profile
    {
        public BookStoreApiMapper()
        {
            CreateMap<Book,BookDTO>().ForMember(dest => dest.Name,opt => opt.MapFrom(src=> src.BookName)).ReverseMap();
            CreateMap<User,UpdateUser>().ReverseMap();
            CreateMap<Role, RoleShow>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Category, CategoryShow>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name)).ReverseMap();
            CreateMap<User, CreateUser>().ReverseMap();
            CreateMap<Category,CategoryDTO>().ReverseMap();
            CreateMap<Book, BookInBill>().ForMember(dest => dest.Name,opt => opt.MapFrom(src=>src.BookName)).ReverseMap();
            CreateMap<User,UserShow>().ReverseMap();
        }
    }
}