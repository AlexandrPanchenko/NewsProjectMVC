using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using NewsDotNet.DomainModel.Entities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using NewsDotNet.DomainModel.Concrete;

namespace NewsDotNet.WebUI.Infrastracture
{
    public class IdentityRoleManager: RoleManager<UserRole>, IDisposable
    {
        public static List<string> UserRoles { get; private set; }

        static IdentityRoleManager()
        {
            UserRoles = new List<string> { "User", "Corrector", "Editor", "Administrator" };
        }
        public IdentityRoleManager(RoleStore<UserRole> store) : base(store) { }

        public static IdentityRoleManager Create(IdentityFactoryOptions<IdentityRoleManager> options, IOwinContext context)
        {
            var manager = new IdentityRoleManager(new RoleStore<UserRole>(context.Get<IdentityContext>()));

            var mainRoles = UserRoles.Concat(new string[] { "Blocked", "RequirePasswordChange" });

            foreach(var role in mainRoles)
            {
                if(!manager.RoleExists(role))
                {
                    manager.Create(new UserRole(role));
                }
            }

            return manager;
        }
    }
}