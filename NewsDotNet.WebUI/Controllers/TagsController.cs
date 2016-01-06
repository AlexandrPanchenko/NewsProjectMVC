using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsDotNet.DomainModel.Abstract;
using NewsDotNet.DomainModel.Entities;

namespace NewsDotNet.WebUI.Controllers
{
    public class TagsController : Controller
    {
        ITagsRepository _tagRepo;

        public TagsController(ITagsRepository tagRepo)
        {
            _tagRepo = tagRepo;
        }
        /// <summary>
        /// list of all tags in repository
        /// </summary>
        public PartialViewResult TagsCloud()
        {
            ///throw new NotImplementedException();
            return PartialView(_tagRepo.All().ToList());
        }
	}
}