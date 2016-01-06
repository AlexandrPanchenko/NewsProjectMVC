using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsDotNet.DomainModel.Entities
{
    public enum ArticleStates
    {
        [Display(Name = "Черновик")]
        Draft,
        [Display(Name = "Опубликованная")]
        Published,
        [Display(Name = "Удаленная")]
        Deleted
    }

    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(200)]
        [Required]
        public string Title { get; set; }
        /// <summary>
        /// Name that will be displayed in adress string in browser
        /// </summary>
        [Index(IsUnique=true)]
        [StringLength(60)]
        [Required]
        public string AddressName { get; set; }
        [StringLength(100)]
        public string TitleImagePath { get; set; }
        public string Body { get; set; }
        [Required]
        public DateTime CreatedTime { get; set; }
        public DateTime LastChangedTime { get; set; }
        public string AuthorId { get; set; }
        public ArticleStates State { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
