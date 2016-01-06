using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsDotNet.WebUI.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}