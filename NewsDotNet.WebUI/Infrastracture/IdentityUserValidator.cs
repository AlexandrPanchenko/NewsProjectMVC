using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsDotNet.DomainModel.Entities;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NewsDotNet.WebUI.Infrastracture
{
    /// <summary>
    /// Validates user fields before saving it to Database
    /// </summary>
    public class IdentityUserValidator : UserValidator<User>
    {
        public IdentityUserValidator(UserManager<User> userManager) : base(userManager) { }

        public override async Task<IdentityResult> ValidateAsync(User item)
        {
            var baseResult = await base.ValidateAsync(item);
            List<string> errors = new List<string>(baseResult.Errors);

            ValidationContext context = new ValidationContext(item);

            var validationErrors = new List<ValidationResult>();

            bool validationSuccess = Validator.TryValidateObject(item, context, validationErrors, true);

            

            if(!validationSuccess)
            {
                foreach(var error in validationErrors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }

            if(errors.Count == 0)
            {
                return IdentityResult.Success;
            }
            else
            {
                return new IdentityResult(errors);
            }

        }
    }
}