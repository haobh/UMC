using System.Threading.Tasks;
using UMC.Model.Models;
using UMC.Web.Models;

namespace TeduShop.Web.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        public static void UpdateMenuGroup(this MenuGroup menuGroup, MenuGroupViewModel menuGroupVm)
        {
            menuGroup.ID = menuGroupVm.ID;
            menuGroup.Name = menuGroupVm.Name;          
        }
        public static void UpdateMenu(this Menu menu, MenuViewModel menuVm)
        {
            menu.ID = menuVm.ID;
            menu.Name = menuVm.Name;
            menu.Status = menuVm.Status;
            menu.Target = menuVm.Target;
            menu.URL = menuVm.URL;
            menu.GroupID = menuVm.GroupID;
        }
        public static void UpdateFooter(this Footer footer, FooterViewModel footerVm)
        {
            footer.ID = footerVm.ID;
            footer.Content = footerVm.Content;
        }
    }
}