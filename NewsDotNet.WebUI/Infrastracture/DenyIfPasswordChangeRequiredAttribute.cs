using System;
using System.Web;
using System.Web.Mvc.Filters;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity.Owin;

namespace NewsDotNet.WebUI.Infrastracture
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class DenyIfPasswordChangeRequiredAttribute : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var user = filterContext.Principal;
            
            if (user.Identity.IsAuthenticated && user.IsInRole("RequirePasswordChange"))
            {
                bool hasAllowAttribute = filterContext.ActionDescriptor.IsDefined(typeof(AllowIfPasswordChangeRequiredAttribute), false);
                bool hasAllowAnonymousAttribute = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), false);
                if (hasAllowAttribute || hasAllowAnonymousAttribute)
                    return;

                filterContext.Controller.TempData["notification"] = "Пароль Вашей учетной записи был изменен администратором. Для продолжения работы измените пароль на желаемый";
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"controller", "Users"},
                    {"action", "ChangePasswordAgain"},
                    {"returnUrl", filterContext.HttpContext.Request.Url.LocalPath}
                });
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            //Not needed
        }
    }
}