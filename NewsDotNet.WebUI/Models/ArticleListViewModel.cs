using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsDotNet.DomainModel.Entities;

namespace NewsDotNet.WebUI.Models
{
    public class ArticleListViewModel
    {
        public IEnumerable<Article> Articles { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}