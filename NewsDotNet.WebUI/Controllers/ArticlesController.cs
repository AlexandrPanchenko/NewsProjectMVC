using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsDotNet.DomainModel.Entities;
using NewsDotNet.DomainModel.Abstract;
using NewsDotNet.WebUI.Models;
using System.Globalization;
using NewsDotNet.WebUI.Infrastracture;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;


namespace NewsDotNet.WebUI.Controllers
{
    public class ArticlesController : Controller
    {
        private IArticlesRepository _articleRepo;
        private ITagsRepository _tagsRepo;
        private IMainPageEntitiesRepository _mainPageRepo;
        
        public ArticlesController(IArticlesRepository articleRepo, ITagsRepository tagRepo, IMainPageEntitiesRepository mainPageRepo)
        {
            _articleRepo = articleRepo;
            _tagsRepo = tagRepo;
            _mainPageRepo = mainPageRepo;
        }
        
        public async Task<ActionResult> Show(string addressName)
        {
            var article = _articleRepo.GetByAddressName(addressName);
            if (null == article)
                return View("Http404", "/" as object);

            var userManager = HttpContext.GetOwinContext().GetUserManager<IdentityUserManager>();
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            bool canUserSeeDraftedArticle = user != null &&
                (!User.IsInRole("User") || user.Id == article.AuthorId);

            if (article.State == ArticleStates.Deleted ||
                article.State == ArticleStates.Draft && !canUserSeeDraftedArticle)
                    return View("Http404", "/" as object);

            article.Body = ParserSingletone.ToHtml(article.Body);
            return View("Article", article);
        }
       
     public ActionResult Archive(string search){
            var mainPageArticles = _mainPageRepo.All().Select(e => e.Article);
            var articles = _articleRepo.All()
                .Where(a => a.State == ArticleStates.Published && !mainPageArticles.Contains(a))
                .OrderByDescending(a => a.CreatedTime)
                .GroupBy(a => a.CreatedTime.ToShortDateString())
                .Select(group => new ArchiveArticles { Date = group.Key, Articles = group.ToList() });
            return View("ArchiveView",articles);
        }
        public ActionResult ArticlesWithTag(string tagAddressName, int page = 1)
        {
            var tag = _tagsRepo.All().Where(t => t.AddressName == tagAddressName).FirstOrDefault();
            IEnumerable<Article> articles;

            if (tag == null)
            {
                articles = new List<Article>();
            }
            else
            {
                articles = tag.Articles.Where(a => a.State == ArticleStates.Published);
            }

            var oftenTag = articles.SelectMany(article => article.Tags)
                            .Where(t => t.ID != tag.ID)
                            .GroupBy(t => t.ID)
                            .OrderByDescending(group => group.Count())
                            .Select(group => group.First())
                            .FirstOrDefault();

            if (oftenTag != null)
            {
                ViewBag.MostOftenTagAddressName = oftenTag.AddressName;
                ViewBag.MostOftenTagName = oftenTag.Name;
            }

            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                TotalItems = articles.Count(),
                ItemsPerPage = 10
            };
            ViewBag.Title = String.Format("Статьи с тегом {0}", tag.Name);
            ViewBag.TagAddressName = tag.AddressName;
            ViewBag.TagName = tag.Name;

            var result = new ArticleListViewModel
            {
                Articles = articles
                .Skip(pagingInfo.ItemsPerPage * (pagingInfo.CurrentPage - 1))
                .Take(pagingInfo.ItemsPerPage)
                .ToList(),
                PagingInfo = pagingInfo
            };

            return View(result);
        }
        public ActionResult ArticlesForSearch(string search)
        {
            var keywords = search.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries).Select(word => word.ToLower());

            var keywordMatch = _articleRepo.All().Where(a => 
                a.State == ArticleStates.Published && 
                keywords.Any(kw => 
                    a.Title.ToLower()
                    .Contains(kw) || 
                    a.Body.Contains(kw)))
                    .ToList();

            var tagsMatch = _tagsRepo.All()
                .Where(tag => keywords.Any(kw => tag.Name.ToLower().Contains(kw))).ToList()
                .SelectMany(tag => tag.Articles.ToList());

            IEnumerable<Article> dateMatch = new List<Article>();

            DateTime searchDate;

            var cultureName = Request.UserLanguages.FirstOrDefault() ?? "ru-RU";
            CultureInfo culture;

            try
            {
                culture = CultureInfo.CreateSpecificCulture(cultureName);
            }
            catch(CultureNotFoundException)
            {
                culture = CultureInfo.CreateSpecificCulture("ru-RU");
            }

            bool isDate = DateTime.TryParse(search, culture, DateTimeStyles.None, out searchDate);

            var articles = keywordMatch.Concat(tagsMatch);

            if(isDate)
            {
                dateMatch = _articleRepo.All().Where(a => a.CreatedTime.Date == searchDate.Date);
                articles = articles.Concat(dateMatch);
            }

            articles = articles
                .Where(a => a.State == ArticleStates.Published)
                .GroupBy(a => a.Id)
                .OrderBy(group => group.Count())
                .Select(group => group.First())
                .ToList();

            return PartialView(articles);
        }

        public ActionResult ArticleAuthorInit()
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<IdentityUserManager>();

            var author = userManager.Users.FirstOrDefault();
            var articles = _articleRepo.All().Where(a => a.AuthorId == null).ToList(); 
            
            if(author != null)
            {
                articles.ForEach(a => {
                    a.AuthorId = author.Id;
                    _articleRepo.Update(a);
                });
            }

            return RedirectToAction("Index","Home");
        }

        public async Task<ActionResult> ArticlesByAuthor(string authorName, int page = 1)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<IdentityUserManager>();
            var author = await userManager.FindByNameAsync(authorName);
            if (null == author)
                return View("Author", null);
            var articles = _articleRepo.All()
                .Where(article => article.AuthorId.Equals(author.Id) &&
                       article.State == ArticleStates.Published)
                .OrderByDescending(article => article.CreatedTime);


            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                TotalItems = articles.Count(),
                ItemsPerPage = 10
            };

            ViewBag.AuthorName = authorName;
            ViewBag.AuthorFirstName = author.FirstName;
            ViewBag.AuthorLastName = author.LastName;

            var result = new ArticleListViewModel
            {
                Articles = articles
                .Skip(pagingInfo.ItemsPerPage * (pagingInfo.CurrentPage - 1))
                .Take(pagingInfo.ItemsPerPage)
                .ToList(),
                PagingInfo = pagingInfo
            };

            return View("Author", result);
        }
      
	}
}