using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsDotNet.DomainModel.Entities;

namespace NewsDotNet.WebUI.Models
{
    public class ArchiveArticles
    {
        public string Date { get; set; }
        public ICollection<Article> Articles { get; set; }
        public System.TimeSpan tspan = new System.TimeSpan(1, 0, 0, 0);
    }
}