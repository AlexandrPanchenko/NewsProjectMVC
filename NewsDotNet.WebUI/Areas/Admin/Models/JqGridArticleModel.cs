using NewsDotNet.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsDotNet.WebUI.Areas.Admin.Models
{
    public class JqGridArticleModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AddressName { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastChangedTime { get; set; }
        public string AuthorName { get; set; }
        public ArticleStates State { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}