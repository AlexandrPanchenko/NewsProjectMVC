using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsDotNet.WebUI.Areas.Admin.Models
{
    public class CreateUserModel
    {
        [Required(ErrorMessage="Это поле обязательно")]
        [StringLength(15, ErrorMessage = "Длина логина не может превышать 15 символов")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Это поле обязательно")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Это поле обязательно")]
        public string PasswordConfirm { get; set; }
        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(15, ErrorMessage = "Длина имени не может превышать 15 символов")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Это поле обязательно")]
        public string Role { get; set; }
        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(15, ErrorMessage = "Длина фамилии не может превышать 15 символов")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Это поле обязательно")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage="Неверный формат адреса")]
        [StringLength(50, ErrorMessage = "Длина электронной почты не может превышать 50 символов")]
        public string Email { get; set; }
    }
}