using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreDesktop.Automapper;
namespace BookStoreDesktop.Automapper
{
    public static class ConfigMapper
    {
        public static IMapper configMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperBookStoreDesktop());
            });
            return config.CreateMapper();
        }
    }
}