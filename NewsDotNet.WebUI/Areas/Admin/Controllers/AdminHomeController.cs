using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsDotNet.WebUI.Infrastracture;
using NewsDotNet.DomainModel.Entities;
using NewsDotNet.DomainModel.Abstract;
using Microsoft.AspNet.Identity.Owin;


namespace NewsDotNet.WebUI.Areas.Admin.Controllers
{
    [Authorize]
    [DenyBlocked]
    [DenyIfPasswordChangeRequired]
    public class AdminHomeController : Controller
    {
        /// <summary>
        /// Return page to manage Articles and tags.
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }
     
        private IArticlesRepository _articleRepo;

        public AdminHomeController(IArticlesRepository articleRepo)
        {
            _articleRepo = articleRepo;
        }
        
        public ActionResult AccessDenied()
        {
            return View();
        }

        private IdentityUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IdentityUserManager>();
            }
        }
	}
}