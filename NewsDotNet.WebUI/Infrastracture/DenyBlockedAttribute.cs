using System;
using System.Web;
using System.Web.Mvc.Filters;
using System.Web.Mvc;

namespace NewsDotNet.WebUI.Infrastracture
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class DenyBlockedAttribute : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var user = filterContext.Principal;

            if (user.Identity.IsAuthenticated && user.IsInRole("Blocked"))
            {
                filterContext.HttpContext.GetOwinContext().Authentication.SignOut();
                filterContext.Controller.TempData["notification"] = "Ваша учетная запись была заблокирована. Обратитесь к администратору или войдите под другой учетной записью";
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            //Not needed
        }
    }
}