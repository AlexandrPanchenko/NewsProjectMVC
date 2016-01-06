using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace NewsDotNet.WebUI.Areas.Admin.Models
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Поле \"Имя\" не может быть пустым")]
        [StringLength(15, ErrorMessage = "Длина имени не может превышать 15 символов")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Поле \"Фамилия\" не может быть пустым")]
        [StringLength(15, ErrorMessage = "Длина фамилии не может превышать 15 символов")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Поле \"E-Mail\" не может быть пустым")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Неверный формат адреса")]
        [Remote("ValidateEmailAvailability", "Users")]
        public string Email { get; set; }
    }
}