using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsDotNet.DomainModel.Abstract;
using NewsDotNet.DomainModel.Entities;

namespace NewsDotNet.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IMainPageEntitiesRepository _mainPageRepo;

        public HomeController(IMainPageEntitiesRepository mainPageRepo)
        {
            _mainPageRepo = mainPageRepo;
        }

        //
        // GET: /Home/
        public ActionResult Index()
        {
            var articles = _mainPageRepo.All().Where(e => e.Article.State == ArticleStates.Published).ToList();
            return View(articles);
        }
	}
}