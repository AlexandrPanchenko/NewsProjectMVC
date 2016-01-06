using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsDotNet.WebUI.Areas.Admin.Models
{
    public class UserListViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsBlocked { get; set; }
    }
}