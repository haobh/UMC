using AutoMapper;
using Microsoft.Owin.BuilderProperties;
using UMC.Model.Models;
using UMC.Web.Models;

namespace TeduShop.Web.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<MenuGroup, MenuGroupViewModel>();
                cfg.CreateMap<Menu,MenuViewModel>();
                cfg.CreateMap<Footer, FooterViewModel>();
            });
        }
    }
}