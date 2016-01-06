using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NewsDotNet.DomainModel.Entities
{
    public class UserRole : IdentityRole
    {
        public UserRole() : base() { }
        public UserRole(string roleName) : base(roleName) { }
    }
}
