using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsDotNet.DomainModel.Entities;
using NewsDotNet.DomainModel.Concrete;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace NewsDotNet.WebUI.Infrastracture
{
    public class IdentityUserManager: UserManager<User>
    {
        public IdentityUserManager(IUserStore<User> store) : base(store) 
        {
            UserValidator = new IdentityUserValidator(this) { AllowOnlyAlphanumericUserNames = false, RequireUniqueEmail = true };
        }
        public static IdentityUserManager Create(IdentityFactoryOptions<IdentityUserManager> options, IOwinContext context)
        {
            IdentityContext dbContext = context.Get<IdentityContext>();
            IdentityUserManager manager = new IdentityUserManager(new UserStore<User>(dbContext));

            //Prevent cituation when there are no users in base and so admin area is unavailable
            //TODO: Replace it whith something better looking
            if(manager.Users.Count() == 0)
            {
                var admin = new User
                {
                    UserName = "admin",
                    FirstName = "Default",
                    LastName = "Admin",
                    Email = "test@example.com",
                };
                manager.Create(admin, "qwerty159753");

                admin = manager.FindByName(admin.UserName);
                manager.AddToRole(admin.Id, "Administrator");
            }

            return manager;
        }
    }
}