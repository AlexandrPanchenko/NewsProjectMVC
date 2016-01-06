using NewsDotNet.DomainModel.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using NewsDotNet.WebUI.Infrastracture;
using Microsoft.AspNet.Identity;
using NewsDotNet.DomainModel.Entities;

namespace NewsDotNet.WebUI.Controllers
{
    public class ClientApiController : Controller
    {
        private IArticlesRepository _articleRepo;
        private IMainPageEntitiesRepository _mainPageRepo;

        public ClientApiController(IArticlesRepository articleRepo, IMainPageEntitiesRepository mainPageRepo)
        {
            _articleRepo = articleRepo;
            _mainPageRepo = mainPageRepo;
        }

        public ActionResult MainPageArticles()
        {
            var articles = from article in _mainPageRepo.All().OrderBy(a => a.GridData.Row).Select(a => a.Article).ToList()
                           select new
                           {
                               article.AddressName,
                               article.Title,
                               article.TitleImagePath,
                               Author = new
                               {
                                   Name = GetAuthorName(article.AuthorId),
                                   Login = GetAuthorLogin(article.AuthorId)
                               },
                               Tags = from tag in article.Tags select tag.Name,
                               Time = FormatDate(article.LastChangedTime)
                           };
            return Json(articles, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ArchiveArticles()
        {
            var mainPageArticles = _mainPageRepo.All().Select(e => e.Article);
            var articles = from article in _articleRepo.All()
                           .Where(a => a.State == ArticleStates.Published && !mainPageArticles.Contains(a))
                           .OrderByDescending(a => a.CreatedTime).ToList()
                           select new
                           {
                               article.AddressName,
                               article.Title,
                               Author = new
                               {
                                   Name = GetAuthorName(article.AuthorId),
                                   Login = GetAuthorLogin(article.AuthorId)
                               },
                               Tags = from tag in article.Tags select tag.Name,
                               Time = FormatDate(article.LastChangedTime)
                           };
            return Json(articles, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Article(string addressName)
        {
            var article = _articleRepo.GetByAddressName(addressName);
            dynamic result;
            if (null == article)
                result = new { result = "error", msg = "Не удалось найти статью" };
            else
                result = new
                {
                    article.AddressName,
                    article.Title,
                    article.TitleImagePath,
                    Author = new
                    {
                        Name = GetAuthorName(article.AuthorId),
                        Login = GetAuthorLogin(article.AuthorId)
                    },
                    Body = ParserSingletone.ToHtml(article.Body),
                    Tags = from tag in article.Tags select tag.Name,
                    Time = FormatDate(article.LastChangedTime)
                };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string GetAuthorName(string id)
        {
            var author = GetAuthor(id);
            if (author == null)
                return "Author";
            return String.Join(" ", author.FirstName, author.LastName);
        }

        private string GetAuthorLogin(string id)
        {
            var author = GetAuthor(id);
            if (author == null)
                return "admin";
            return author.UserName;
        }

        private DomainModel.Entities.User GetAuthor(string id)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<IdentityUserManager>();
            return userManager.FindById(id);
        }

        private string FormatDate(DateTime date)
        {
            return date.ToString("f", new System.Globalization.CultureInfo("ru-RU"));
        }
    }
}