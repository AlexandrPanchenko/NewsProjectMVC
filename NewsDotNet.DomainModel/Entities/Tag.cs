using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NewsDotNet.DomainModel.Entities
{
    ///<summary>
    ///is a tag of Article</summary>
    public class Tag
    {
        ///<summary>
        ///is an id spesificator</summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        ///<summary>
        ///is a name</summary>
        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Name: Field is required")]
        [StringLength(20, ErrorMessage = "Name: Length should not exceed 20 characters")]
        public string Name { get; set; }
        [Required]
        [Index(IsUnique = true)]
        [StringLength(30, ErrorMessage= "Длина адреса не может превышать 50 символов")]
        public string AddressName { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<Article> Articles { get; set; }
    }

    class TagEqualityComparer : IEqualityComparer<Tag>
    {
        public bool Equals(Tag x, Tag y)
        {
            return (x.ID == y.ID);
        }

        public int GetHashCode(Tag obj)
        {
            // TODO: look for a better hash function
            return obj.ID % 1013;
        }
    }
}
