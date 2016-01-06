using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsDotNet.WebUI.Infrastracture;
using NewsDotNet.DomainModel.Entities;
using NewsDotNet.DomainModel.Abstract;

namespace NewsDotNet.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles="Administrator, Editor")]
    [DenyBlocked]
    [DenyIfPasswordChangeRequired]
    public class IssueController : Controller
    {
        IMainPageEntitiesRepository _mainPageRepo;
        IArticlesRepository _articlesRepo;

        public IssueController(IMainPageEntitiesRepository mainPageRepo, IArticlesRepository articlesRepo)
        {
            _mainPageRepo = mainPageRepo;
            _articlesRepo = articlesRepo;
        }
        //
        // GET: /Admin/Issue/
        public ActionResult Index()
        {
            return View(_mainPageRepo.All().Where(e => !e.IsFeatured).ToList());
        }


        [HttpPost]
        public ActionResult Save(IEnumerable<MainPageEntity> newEntities)
        {
            _mainPageRepo.Refill(newEntities);
            return new JsonResult { Data = new { result = "OK" } };
        }
	}
}