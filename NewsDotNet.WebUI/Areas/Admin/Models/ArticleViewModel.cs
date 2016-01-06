using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsDotNet.DomainModel.Abstract;
using NewsDotNet.DomainModel.Entities;

namespace NewsDotNet.WebUI.Areas.Admin.Models
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Введите заголовок статьи")]
        [StringLength(200, ErrorMessage = "Длина заголовка не должна превышать 200 символов")]
        public string Title { get; set; }

        public string TitleImagePath { get; set; }

        [Required(ErrorMessage = "Введите адрес статьи")]
        [StringLength(60, ErrorMessage = "Длина адреса не должна превышать 60 символов")]
        [RegularExpression(@"^[a-zA-Z0-9][a-zA-Z0-9\-_]*$",
            ErrorMessage = "Адрес должен состоять из символов латинского алфавита, цифр, дефисов и подчеркиваний")]
        [Remote("ValidateAddressNameAvailability", "AdminArticles", AdditionalFields = "Id")]
        public string AddressName { get; set; }

        [Required(ErrorMessage = "Тело статьи не может быть пустым")]
        public string Body { get; set; }

        public DateTime CreatedTime { get; set; }

        public string AuthorId { get; set; }

        public ArticleStates State { get; set; }
        
        [Required(ErrorMessage = "Введите хотя бы один тег")]
        public string TagsString { get; set; }
    }
}