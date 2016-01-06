using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsDotNet.DomainModel.Entities
{
    public class MainPageEntity
    {
        [Key]
        public int ID { get; set; }
        public int ArticleID { get; set; }
        public virtual Article Article { get; set; }
        public bool IsFeatured { get; set; }
        [Required]
        public virtual GridData GridData { get; set; }
    }
}
