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
        public static void UpdateMenu(this MenuGroup menuGroup, MenuGroupViewModel menuGroupVm)
        {
            menuGroup.ID = menuGroupVm.ID;
            menuGroup.Name = menuGroupVm.Name;
        }
    }
}