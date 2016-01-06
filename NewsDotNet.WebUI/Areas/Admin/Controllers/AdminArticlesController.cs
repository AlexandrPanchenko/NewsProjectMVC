using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsDotNet.DomainModel.Entities;
using NewsDotNet.DomainModel.Abstract;
using NewsDotNet.WebUI.Areas.Admin.Mappers;
using NewsDotNet.WebUI.Areas.Admin.Models;
using Ninject;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using NewsDotNet.WebUI.Infrastracture;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using NewsDotNet.WebUI.Models;
using System.Text.RegularExpressions;

namespace NewsDotNet.WebUI.Areas.Admin.Controllers
{
    [Authorize]
    [DenyBlocked]
    [DenyIfPasswordChangeRequired]
    public class AdminArticlesController : Controller
    {
        private IArticlesRepository _articleRepo;
        private ITagsRepository _tagsRepo;
        private IMapper _modelMapper;

        public AdminArticlesController(IArticlesRepository articleRepo, ITagsRepository tagsRepo, IMapper modelMapper)
        {
            _articleRepo = articleRepo;
            _tagsRepo = tagsRepo;
            _modelMapper = modelMapper;
        }

        public ActionResult List(IEnumerable<int> articleIds)
        {
            //seletct only articles with ids form the list
            var result = _articleRepo.All().Where(article => articleIds.Any(id => id == article.Id)).ToList();

            var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

            var convertedResult = JsonConvert.SerializeObject(result, jsonSettings);

            return Content(convertedResult, "application/json");
        }

        public ActionResult PagingList(int page = 1, int itemsPerPage = 10)
        {
            var publishedArticles = _articleRepo.All().Where(a => a.State == ArticleStates.Published);
            var articlesOnPage = publishedArticles.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = itemsPerPage,
                TotalItems = publishedArticles.Count()
            };

            var result = new ArticleListViewModel
            {
                Articles = articlesOnPage,
                PagingInfo = pagingInfo
            };

            var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

            var convertedResult = JsonConvert.SerializeObject(result, jsonSettings);
            return Content(convertedResult, "application/json");
        }

        [HttpGet]
        [Authorize(Roles="User, Editor, Administrator")]
        public ActionResult New()
        {
            var article = new ArticleViewModel();
            ViewBag.Action = ActionType.NewArticle;
            return View("ArticleEditor", article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User, Editor, Administrator")]
        public async Task<ActionResult> New(ArticleViewModel newArticleView, HttpPostedFileBase titleImage)
        {
            CheckAddressNameAvailability(newArticleView.AddressName, newArticleView.Id);
            if (ModelState.IsValid)
            {
                var article = _modelMapper.Map<ArticleViewModel, Article>(newArticleView);
                article.CreatedTime = DateTime.Now;
                article.LastChangedTime = DateTime.Now;
                article.Tags = TagStringToList(newArticleView.TagsString);
                article.Body = Regex.Replace(article.Body, @"=([0-9])+px", "");
                SaveUploadedImage(article, titleImage);

                var author = await UserManager.FindByNameAsync(User.Identity.Name);
                article.AuthorId = author.Id;

                _articleRepo.Add(article);
                return RedirectToAction("Index", "AdminHome");
            }
            ViewBag.Action = ActionType.NewArticle;
            return View("ArticleEditor", newArticleView);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string addressName)
        {
            var article = _articleRepo.GetByAddressName(addressName);
            if (null == article)
                return View("Http404", "/Admin" as object);
            if (await UserCanEditArticle(article))
            {
                var articleView = _modelMapper.Map<Article, ArticleViewModel>(article);
                articleView.TagsString = TagListToString(article.Tags);
                ViewBag.Action = ActionType.EditArticle;
                return View("ArticleEditor", articleView); 
            }
            else
            {
                return RedirectToAction("AccessDenied", "AdminHome");
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ArticleViewModel articleView, HttpPostedFileBase titleImage)
        {
            CheckAddressNameAvailability(articleView.AddressName, articleView.Id);
            if (ModelState.IsValid)
            {
                var article = _modelMapper.Map<ArticleViewModel, Article>(articleView);
                if (await UserCanEditArticle(article))
                {
                    article.Tags = TagStringToList(articleView.TagsString);
                    article.LastChangedTime = DateTime.Now;
                    article.Body = Regex.Replace(article.Body, @"=([0-9])+px", "");
                    SaveUploadedImage(article, titleImage);
                    _articleRepo.Update(article);
                    return RedirectToAction("Index", "AdminHome"); 
                }
                else
                {
                    return RedirectToAction("AccessDenied", "AdminHome");
                }
            }
            ViewBag.Action = ActionType.EditArticle;
            return View("ArticleEditor", articleView);
        }

        private string TagListToString(ICollection<Tag> tags)
        {
            if (tags.Count == 0)
                return String.Empty;
            return String.Join<string>(",", tags.Select(tag => tag.Name));
        }

        private ICollection<Tag> TagStringToList(string tagString)
        {
            List<Tag> tags = new List<Tag>();
            if (!String.IsNullOrWhiteSpace(tagString))
            {
                var tagArray = tagString.Split(',');
                foreach (var s in tagArray)
                {
                    if (!String.IsNullOrWhiteSpace(s))
                    {
                        string name = s.Trim();
                        Tag t = _tagsRepo.GetByName(name);
                        if (null == t)
                        {
                            t = _tagsRepo.Add(new Tag { Name = name, AddressName = name });
                        }
                        tags.Add(t);
                    }
                }
            }
            return tags;
        }

        private void SaveUploadedImage(Article article, HttpPostedFileBase image)
        {
            string newPath;
            string oldPath = article.TitleImagePath;
            bool pathChanged = !isOldImagePathRelevant(oldPath, article.AddressName);
            FileInfo oldImageInfo = null;
            if (pathChanged)
                oldImageInfo = new FileInfo(Server.MapPath(oldPath));
            if (null == image)
            {
                if (pathChanged)
                {   //there is an old image and article address changed => rename old image
                    newPath = String.Format("~/Content/Images/Articles/{0}{1}",
                                        article.AddressName,
                                        oldImageInfo.Extension);
                    oldImageInfo.MoveTo(Server.MapPath(newPath));
                }
                else
                {
                    if (null == oldPath)
                        article.TitleImagePath = "";
                    return;
                }
            }
            else
            {   //there is a new image => save new image
                FileInfo newImageInfo = new FileInfo(image.FileName);
                newPath = String.Format("~/Content/Images/Articles/{0}{1}",
                                            article.AddressName,
                                            newImageInfo.Extension);
                image.SaveAs(Server.MapPath(newPath));

                if (pathChanged)
                {   //path changed and there is an old image => delete old image
                    oldImageInfo.Delete();
                }
            }
            article.TitleImagePath = newPath;
        }

        public JsonResult ValidateAddressNameAvailability(string AddressName, int Id)
        {
            if (isAddressNameAvailable(AddressName, Id))
                return Json("true", JsonRequestBehavior.AllowGet);
            else
                return Json(String.Format("Адрес {0} уже принадлежит другой статье", AddressName), JsonRequestBehavior.AllowGet);
        }

        private void CheckAddressNameAvailability(string addressName, int id)
        {
            if (!isAddressNameAvailable(addressName, id))
                ModelState.AddModelError("addressName", String.Format("Адрес {0} уже принадлежит другой статье", addressName));
        }

        private bool isAddressNameAvailable(string addressName, int id)
        {
            var existingArticle = _articleRepo.GetByAddressName(addressName);
            return (null == existingArticle || existingArticle.Id == id);
        }

        private bool isOldImagePathRelevant(string imagePath, string articleAddressName)
        {
            if (String.IsNullOrEmpty(imagePath))
                return true;
            FileInfo info = new FileInfo(imagePath);
            string name = info.Name.Remove(info.Name.LastIndexOf('.'));
            return name.Equals(articleAddressName);
        }

        public async Task<ContentResult> Articles(JqInViewModel jqParams)
        {
  
            var articles = _articleRepo.All();

            if(User.IsInRole("User"))
            {
                var loggedUser = await UserManager.FindByNameAsync(User.Identity.Name);
                articles = articles.Where(article => article.AuthorId == loggedUser.Id);
            }

            switch (jqParams.sidx)
            {
                case "Title":
                    articles = articles.OrderByDescending(a => a.Title);
                    break;
                case "State":
                    articles = articles.OrderByDescending(a => a.State);
                    break;
                case "CreatedTime":
                    articles = articles.OrderByDescending(a => a.CreatedTime);
                    break;
                case "LastChangedTime":
                    articles = articles.OrderByDescending(a => a.LastChangedTime);
                    break;
                default:
                    articles = articles.OrderByDescending(a => a.CreatedTime);
                    break;
            }

            if (jqParams.sord == "asc") 
            {
                articles = articles.Reverse();
            }
               

            articles = articles
                        .Skip((jqParams.page-1) * jqParams.rows)
                        .Take(jqParams.rows)
                        .ToList();

            var rows = articles.Select(article => _modelMapper.Map<Article, JqGridArticleModel>(article)).ToList();
            //I am really sorry for this...
            rows.ForEach(row => {
                var art = articles.Where(a => a.Id == row.Id).First();
                var author = UserManager.Users.Where(u => u.Id == art.AuthorId).First();
                row.AuthorName = author.UserName;
            });

            var totalArticles = _articleRepo.All().Count();
            return Content(JsonConvert.SerializeObject(new
            {
                page = jqParams.page,
                records = totalArticles,
                rows = rows,
                total = Math.Ceiling(Convert.ToDouble(totalArticles) / jqParams.rows)
            }, new CustomDateTimeConverter()), "application/json");
        }

        public async Task<ActionResult> DeletedArticles()
        {
            var articles = _articleRepo.All().Where(a => a.State == ArticleStates.Deleted);

            if (User.IsInRole("User"))
            {
                var loggedUser = await UserManager.FindByNameAsync(User.Identity.Name);
                articles = articles.Where(article => article.AuthorId == loggedUser.Id);
            }

            return View("DeletedArticles", articles.ToList());
        }

        /// <summary>
        /// Delete an existing Article.
        /// </summary>
        [HttpPost]
        public async Task<ContentResult> DeleteArticle(int id)
        {
            bool success = false;
            string msg;

            var article = _articleRepo.GetById(id);
            
            if(article == null)
            {
                msg = "Статья с таким ID не найдена";
            }
            else if (!(await UserCanEditArticle(article)))
            {
                msg = "У Вас нет прав на выполнение этой операции!";
            }
            else
            {
                _articleRepo.Delete(article);
                success = true;
                msg = "Статья успешно удалена";
            }

            var result = JsonConvert.SerializeObject(new
            {
                success = success,
                message = msg
            });

            return Content(result, "application/json");
        }

        public async Task<ContentResult> EditArticleState(int id, int state)
        {
            bool success = false;
            string msg;

            var article = _articleRepo.GetById(id);

            if (article == null)
            {
                msg = "Статья с таким ID не найдена";
            }
            else if (!Enum.IsDefined(typeof(ArticleStates), state))
            {
                msg = "Указано неверное состояние статьи";
            }
            else if (!(await UserCanEditArticle(article)))
            {
                msg = "У Вас нет прав на выполнение этой операции!";
            }
            else
            {
                article.State = (ArticleStates)state;
                _articleRepo.Update(article);
                success = true;
                msg = "Состояние статьи успешно изменено!";
            }

            var result = JsonConvert.SerializeObject(new
            {
                success = success,
                message = msg
            });

            return Content(result, "application/json");
        }

        private async Task<bool> UserCanEditArticle(Article article)
        {
            return User.IsInRole("Editor") || User.IsInRole("Corrector") || User.IsInRole("Administrator") || await UserIsArticleAuthor(article);
        }

        private async Task<bool> UserIsArticleAuthor(Article article)
        {
            var articleAuthor = await UserManager.FindByIdAsync(article.AuthorId);

            return User.Identity.Name == articleAuthor.UserName;
        }

        private IdentityUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IdentityUserManager>();
            }
        }
    }

    public enum ActionType
    {
        NewArticle,
        EditArticle
    }
}