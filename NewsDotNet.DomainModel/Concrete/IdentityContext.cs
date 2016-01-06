using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using NewsDotNet.DomainModel.Entities;

namespace NewsDotNet.DomainModel.Concrete
{
    public class IdentityContext: IdentityDbContext<User>
    {
        public IdentityContext() : base("IdentityDb") { }
        static IdentityContext()
        {
            Database.SetInitializer<IdentityContext>(new IdentityInitializer());
        }

        public static IdentityContext Create()
        {
            return new IdentityContext();
        }
    }
}
