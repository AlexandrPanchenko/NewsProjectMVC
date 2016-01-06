using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace NewsDotNet.DomainModel.Concrete
{
    class IdentityInitializer: DropCreateDatabaseIfModelChanges<IdentityContext>
    {
        protected override void Seed(IdentityContext context)
        {
            base.Seed(context);
        }
    }
}
