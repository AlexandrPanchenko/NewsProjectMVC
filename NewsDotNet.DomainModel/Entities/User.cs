using System;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace NewsDotNet.DomainModel.Entities
{
    public class User: IdentityUser
    {
        [Required(ErrorMessage ="Поле \"Имя\" не может быть пустым")]
        [StringLength(15, ErrorMessage = "Длина имени не может превышать 15 символов")]
        public string FirstName { get; set; }
        [Required(ErrorMessage ="Поле \"Фамилия\" не может быть пустым")]
        [StringLength(15, ErrorMessage = "Длина фамилии не может превышать 15 символов")]
        public string LastName { get; set; }

        [Required(ErrorMessage ="Поле \"E-Mail\" не может быть пустым")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Неверный формат адреса")]
        [StringLength(50, ErrorMessage = "Длина электронной почты не может превышать 50 символов")]
        public override string Email
        {
            get
            {
                return base.Email;
            }
            set
            {
                base.Email = value;
            }
        }
        [Required(ErrorMessage = "Поле \"Логин\" не может быть пустым")]
        [StringLength(15, ErrorMessage = "Длина логина не может превышать 15 символов")]
        public override string UserName
        {
            get
            {
                return base.UserName;
            }
            set
            {
                base.UserName = value;
            }
        }

    }
}
