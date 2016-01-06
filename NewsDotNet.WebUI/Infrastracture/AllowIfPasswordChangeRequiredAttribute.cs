using System;
using System.Web.Mvc.Filters;
using System.Web.Mvc;

namespace NewsDotNet.WebUI.Infrastracture
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AllowIfPasswordChangeRequiredAttribute : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            
        }
    }
}