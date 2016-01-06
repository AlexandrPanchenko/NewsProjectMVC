using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsDotNet.WebUI.Infrastracture;
using NewsDotNet.DomainModel.Entities;
using NewsDotNet.DomainModel.Abstract;
using Newtonsoft.Json;
using System.Text;

namespace NewsDotNet.WebUI.Areas.Admin.Controllers
{
    [Authorize]
    [DenyBlocked]
    [DenyIfPasswordChangeRequired]
    public class AdminTagsController : Controller
    {
        private ITagsRepository _tagsRepo;
        public AdminTagsController(ITagsRepository tagsRepo)
        {
            _tagsRepo = tagsRepo;
        }

        /// <summary>
        /// Return all the tags as JSON.
        /// </summary>
        /// <returns></returns>
        public ContentResult Tags()
        {
            var tags = _tagsRepo.All().ToList();

            return Content(JsonConvert.SerializeObject(new
            {
                page = 1,
                records = tags.Count(),
                rows = tags,
                total = 1
            }), "application/json");
        }

        /// <summary>
        /// Add a new tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult AddTag([Bind(Exclude = "ID")]Tag tag)
        {
            string json;

            var addedTag = _tagsRepo.Add(tag);

            if (null != addedTag)
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = addedTag.ID,
                    success = true,
                    message = "Tag added successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Тег с таким именем уже существует"
                });
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Edit an existing tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult EditTag(Tag tag)
        {
            string json;

            if (ModelState.IsValid)
            {
                _tagsRepo.Update(tag);
                json = JsonConvert.SerializeObject(new
                {
                    id = tag.ID,
                    success = true,
                    message = "Changes saved successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to save the changes."
                });
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Delete an existing tag.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult DeleteTag(int id)
        {
            _tagsRepo.Delete(id);

            var json = JsonConvert.SerializeObject(new
            {
                success = true,
                message = "Tag deleted successfully."
            });

            return Content(json, "application/json");
        }

        public JsonResult Autocomplete(string term)
        {
            if (null == term)
                return Json("", JsonRequestBehavior.AllowGet);
            var tags = _tagsRepo.All().Select(t => t.Name)
                .Where(t => t.StartsWith(term,
                                         StringComparison.CurrentCultureIgnoreCase))
                .Take(5)
                .ToList();
            return Json(tags, JsonRequestBehavior.AllowGet);
        }
    }
}