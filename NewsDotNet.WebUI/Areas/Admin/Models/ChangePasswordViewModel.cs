using System.ComponentModel.DataAnnotations;

namespace NewsDotNet.WebUI.Areas.Admin.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Введите старый пароль")]
        public string oldPassword { get; set; }
        [Required(ErrorMessage = "Новый пароль не может быть пустым")]
        public string newPassword { get; set; }
        [Required(ErrorMessage = "Новый пароль не может быть пустым")]
        [Compare("newPassword", ErrorMessage = "Введенные пароли не совпадают")]
        public string confirmNewPassword { get; set; }
    }
}