using AutoMapper;
using UMC.Model.Models;
using UMC.Web.Models;

namespace UMC.Web.Mappings
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
                cfg.CreateMap<ApplicationUser, ApplicationUserViewModel>();
                cfg.CreateMap<ApplicationGroup, ApplicationGroupViewModel>();
                cfg.CreateMap<ApplicationRole, ApplicationRoleViewModel>();
            });
        }
    }
}