using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using NewsDotNet.WebUI.Infrastracture;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using NewsDotNet.DomainModel.Concrete;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace NewsDotNet.WebUI.App_Start
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<IdentityContext>(IdentityContext.Create);
            app.CreatePerOwinContext<IdentityRoleManager>(IdentityRoleManager.Create);
            app.CreatePerOwinContext<IdentityUserManager>(IdentityUserManager.Create);

            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Admin/Users/Login"),
                AuthenticationMode = AuthenticationMode.Active,
                Provider = new Microsoft.Owin.Security.Cookies.CookieAuthenticationProvider
                {
                    OnValidateIdentity = async context =>
                    {
                        if (context.Identity.IsAuthenticated)
                        {
                            var userManager = context.OwinContext.GetUserManager<IdentityUserManager>();
                            var user = await userManager.FindByNameAsync(context.Identity.Name);
                            var securityStamp =
                                    context.Identity.FindFirstValue(Constants.DefaultSecurityStampClaimType);
                            if (securityStamp == await userManager.GetSecurityStampAsync(user.Id))
                            {
                                var identity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                                var isPersistent = context.Properties.IsPersistent;
                                context.ReplaceIdentity(identity);
                            }
                            else
                            {
                                context.RejectIdentity();
                                context.OwinContext.Authentication.SignOut(context.Options.AuthenticationType);
                            }
                        }
                    }
                }
            });
        }
    }
}