using System;
using System.Threading.Tasks;
using UMC.Model.Models;
using UMC.Web.Models;

namespace UMC.Web.Infrastructure.Extensions
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
        //public static void UpdateApplicationGroup(this ApplicationGroup appGroup, ApplicationGroupViewModel appGroupViewModel)
        //{
        //    appGroup.ID = appGroupViewModel.ID;
        //    appGroup.Name = appGroupViewModel.Name;
        //}

        //public static void UpdateApplicationRole(this ApplicationRole appRole, ApplicationRoleViewModel appRoleViewModel, string action = "add")
        //{
        //    if (action == "update")
        //        appRole.Id = appRoleViewModel.Id;
        //    else
        //        appRole.Id = Guid.NewGuid().ToString();
        //    appRole.Name = appRoleViewModel.Name;
        //    appRole.Description = appRoleViewModel.Description;
        //}
        public static void UpdateUser(this ApplicationUser appUser, ApplicationUserViewModel appUserViewModel, string action = "add")
        {

            appUser.Id = appUserViewModel.Id;
            appUser.FullName = appUserViewModel.FullName;
            appUser.BirthDay = appUserViewModel.BirthDay;
            appUser.Email = appUserViewModel.Email;
            appUser.UserName = appUserViewModel.UserName;
            appUser.PhoneNumber = appUserViewModel.PhoneNumber;
        }
    }
}