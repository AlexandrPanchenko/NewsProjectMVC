using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using NewsDotNet.WebUI.Infrastracture;
using NewsDotNet.DomainModel.Entities;
using Newtonsoft.Json;
using NewsDotNet.WebUI.Areas.Admin.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using NewsDotNet.WebUI.Areas.Admin.Mappers;
using System.Reflection;
using System.Security.Claims;
using Microsoft.Owin.Security;

namespace NewsDotNet.WebUI.Areas.Admin.Controllers
{
    [Authorize]
    [DenyBlocked]
    [DenyIfPasswordChangeRequired]
    public class UsersController : Controller
    {
        private IMapper _mapper;

        public UsersController(IMapper mapper)
        {
            _mapper = mapper;
        }

        //
        // GET: /Admin/Users/
        [Authorize(Roles="Administrator")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> List()
        {
            var currentUser = await UserManager.FindByNameAsync(User.Identity.Name);
            var userList = (from user in UserManager.Users.Where(user => user.Id != currentUser.Id).ToList()
                            select _mapper.Map<User, UserListViewModel>(user)).ToList();
            userList.ForEach(model => model.Role = IdentityRoleManager.UserRoles.FirstOrDefault(role => UserManager.IsInRole(model.Id, role)));
            userList.ForEach(model => model.IsBlocked = UserManager.IsInRole(model.Id, "Blocked"));
            var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject(userList, jsonSettings);
            return Content(result, "application/json");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult GetUserInfo(string id)
        {
            var user = UserManager.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var result = _mapper.Map<User, UserListViewModel>(user);
            result.Role = IdentityRoleManager.UserRoles.FirstOrDefault(role => UserManager.IsInRole(result.Id, role));
            var convertedResult = JsonConvert.SerializeObject(result);
            return Content(convertedResult, "application/json");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Create(CreateUserModel newUser)
        {
            string response;
            if(newUser.Password != newUser.PasswordConfirm)
            {
                ModelState.AddModelError("Password", "Введенные пароли не совпадают");
            }

            if(!IdentityRoleManager.UserRoles.Any(role => role == newUser.Role))
            {
                ModelState.AddModelError("Role", "Выбранная роль не существует");
            }
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<CreateUserModel, User>(newUser);
                IdentityResult result = await UserManager.CreateAsync(user, newUser.Password);
                if(result.Succeeded)
                {
                    var createdUser = await UserManager.FindByNameAsync(newUser.UserName);
                    result = await UserManager.AddToRolesAsync(createdUser.Id, new string[] { newUser.Role, "RequirePasswordChange" });

                    if(result.Succeeded)
                    {
                        var createdUserModel = _mapper.Map<User, UserListViewModel>(createdUser);
                        createdUserModel.Role = IdentityRoleManager.UserRoles.FirstOrDefault(role => UserManager.IsInRole(createdUserModel.Id, role));

                        response = JsonConvert.SerializeObject(new
                        {
                            Result = "success",
                            User = createdUserModel
                        });
                        return Content(response, "application/json");
                    }
                    else
                    {
                        UserManager.Delete(createdUser);
                        AddErrorsFromResult(result);
                    }
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }

            var errors = ModelState.Where(item=> item.Value.Errors.Any()).ToDictionary(
                    item => item.Key,
                    item => item.Value.Errors.Select(e => e.ErrorMessage).ToList()
                );

            response = JsonConvert.SerializeObject(new
            {
                Result = "error",
                Errors = errors
            });
            return Content(response, "application/json");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult IsUserNameAvailable(string userName)
        {
            bool result = !UserManager.Users.Any(user => user.UserName == userName);
            return Json(result);
        }

        public ActionResult GetRoleList()
        {
            var displayNames = new List<string> { "Автор", "Корректор", "Редактор", "Администратор" };
            var result = IdentityRoleManager.UserRoles.Zip(displayNames, (r, d) => new { value = r, text = d });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> InlineUpdate(string name, string pk, string value)
        {
            string status;
            string msg;

            var whiteList = new[] { "FirstName", "LastName", "Email" };

            var user = UserManager.FindById(pk);
            if(user == null)
            {
                status = "error";
                msg = "Пользователь с таким ID не найден";
            }
            else
            {
                PropertyInfo propertyInfo = user.GetType().GetProperty(name);
                if(!whiteList.Contains(name) || propertyInfo == null)
                {
                    status = "error";
                    msg = "Свойство с таким именем не найдено";
                }
                else
                {
                    Type t = propertyInfo.PropertyType;
                    object convertedValue;
                    convertedValue = Convert.ChangeType(value, t);
                    propertyInfo.SetValue(user, convertedValue);

                    IdentityResult result = await UserManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        status = "success";
                        msg = "Record updated";
                    }
                    else
                    {
                        status = "error";
                        msg = String.Join(",", result.Errors);
                    }
                }
            }
            return Json(new { status = status, msg = msg });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> InlineRoleChange(string name, string pk, string value)
        {
            string status;
            string msg;

            var user = UserManager.FindById(pk);
            var admin = UserManager.FindByName(User.Identity.Name);
            if (user == null)
            {
                status = "error";
                msg = "Пользователь с таким ID не найден";
            }
            else if (!IdentityRoleManager.UserRoles.Any(role => role == value))
            {
                status = "error";
                msg = "Выбранная роль не найдена";
            }
            else if (admin.Id.Equals(pk))
            {
                status = "error";
                msg = "Вы не можете изменить свою роль";
            }
            else
            {
                if(UserManager.IsInRole(pk, value))
                {
                    status = "error";
                    msg = "Пользователь уже в выбранной роли";
                }
                else
                {
                    IdentityResult result;

                    result = await UserManager.RemoveFromRolesAsync(user.Id, IdentityRoleManager.UserRoles.Where(role => UserManager.IsInRole(user.Id, role)).ToArray());
                    if(result.Succeeded)
                    {
                        result = await UserManager.AddToRoleAsync(user.Id, value);
                        if(result.Succeeded)
                        {
                            status = "success";
                            msg = "Record updated";
                        }
                        else
                        {
                            status = "error";
                            msg = String.Join(",", result.Errors);
                        }
                    }
                    else
                    {
                        status = "error";
                        msg = String.Join(",", result.Errors);
                    }
                }
            }
            return Json(new { status = status, msg = msg });
        }

        [AllowAnonymous]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Login(string returnUrl)
        {
            if(User.Identity.IsAuthenticated)
            {
                //If logged user tried to access login page directly by himself
                if(String.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(Url.Action("Index", "AdminHome"));
                }
                //User was redirected to this page from other page he don't have access to
                else
                {
                    return Redirect(Url.Action("AccessDenied", "AdminHome"));
                }
            }

            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [OutputCache(NoStore=true, Duration=0)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.Login, model.Password);
                if(user == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль");
                }
                else
                {
                    ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthManager.SignOut();
                    AuthManager.SignIn(new AuthenticationProperties { IsPersistent = model.RememberMe }, ident);
                    return Redirect(returnUrl ?? Url.Action("Index", "AdminHome"));
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        [AllowIfPasswordChangeRequired]
        public ActionResult Logout()
        {
            AuthManager.SignOut();
            return Redirect(Url.Action("Login"));
        }

        public ActionResult UserPopup()
        {
            var user = UserManager.Users.First(u => u.UserName == User.Identity.Name);
            return PartialView("UserPopupPartial",user);
        }

        public ActionResult EditProfile()
        {
            var user = _mapper.Map<User, EditProfileViewModel>(
                UserManager.Users.First(u => u.UserName == User.Identity.Name)
            );
            return View("ProfileEditor", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile(EditProfileViewModel user)
        {
            var existingUser = UserManager.Users.First(u => u.UserName == User.Identity.Name);
            if (ModelState.IsValid && null != existingUser)
            {
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;
                IdentityResult result = await UserManager.UpdateAsync(existingUser);
                if (result.Succeeded)
                    return RedirectToAction("Index", "AdminHome");
                else
                    AddErrorsFromResult(result);
            }
            return View("ProfileEditor", user);
        }

        public async Task<JsonResult> ValidateEmailAvailability(string email)
        {
            var existingUser = await UserManager.FindByEmailAsync(email);
            if (null != existingUser && !existingUser.UserName.Equals(User.Identity.Name))
                return Json(String.Format("Адрес {0} уже используется другим пользователем", email), JsonRequestBehavior.AllowGet);
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeOwnPassword(ChangePasswordViewModel passwords)
        {
            var user = await UserManager.FindAsync(User.Identity.Name, passwords.oldPassword);
            
            string response = await SetNewPassword(user, passwords.newPassword, passwords.confirmNewPassword, isPasswordOwn: true);

            return Content(response, "application/json");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> ChangeUserPassword(string userId, ChangePasswordViewModel passwords)
        {
            string adminPassword = passwords.oldPassword;
            var admin = UserManager.Users.First(u => u.UserName == User.Identity.Name);
            var user = await UserManager.FindByIdAsync(userId);
            bool isAdminPasswordCorrect = await UserManager.CheckPasswordAsync(admin, adminPassword);

            if (admin.Id.Equals(userId))
                ModelState.AddModelError("", "Для изменения своего пароля перейдите на страницу редактирования профиля");
            else if (null == user)
                ModelState.AddModelError("", "Не удалось найти пользователя с заданным ID");
            else if (!isAdminPasswordCorrect)
                ModelState.AddModelError("oldPassword", "Введен неверный пароль администратора");

            string response = await SetNewPassword(user, passwords.newPassword, passwords.confirmNewPassword, isPasswordOwn: false);

            return Content(response, "application/json");
        }

        [Authorize(Roles = "RequirePasswordChange")]
        [AllowIfPasswordChangeRequired]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult ChangePasswordAgain(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "RequirePasswordChange")]
        [AllowIfPasswordChangeRequired]
        [OutputCache(NoStore = true, Duration = 0)]
        public async Task<ActionResult> ChangePasswordAgain(ChangePasswordViewModel passwords, string returnUrl)
        {
            var user = await UserManager.FindAsync(User.Identity.Name, passwords.oldPassword);
            await SetNewPassword(user, passwords.newPassword, passwords.confirmNewPassword, isPasswordOwn: true);
            if (ModelState.IsValid)
            {
                TempData["notification"] = "Пароль был успешно изменен. Теперь вы можете войти в учетную запись с новым паролем";
                return Redirect(Url.Action("Login", "Users", new { returnUrl }));
            }
            ViewBag.returnUrl = returnUrl;
            return View(passwords);
        }

        private async Task<string> SetNewPassword(User user, string newPassword, string confirmNewPassword, bool isPasswordOwn)
        {
            string response;

            if (ModelState.IsValid && isPasswordOwn && user == null)
                ModelState.AddModelError("oldPassword", "Введен неверный старый пароль");
            
            if (ModelState.IsValid)
            {
                IdentityResult validPass = null;
                validPass = await UserManager.PasswordValidator.ValidateAsync(newPassword);
                if (validPass.Succeeded)
                {
                    user.PasswordHash = UserManager.PasswordHasher.HashPassword(newPassword);
                    IdentityResult result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        response = JsonConvert.SerializeObject(new
                        {
                            result = "success"
                        });
                        if (isPasswordOwn)
                        {
                            AuthManager.SignOut();
                            if (UserManager.IsInRole(user.Id, "RequirePasswordChange"))
                                UserManager.RemoveFromRole(user.Id, "RequirePasswordChange");
                            UserManager.UpdateSecurityStamp(user.Id);
                        }
                        else if (!UserManager.IsInRole(user.Id, "RequirePasswordChange"))
                        {
                            IdentityResult roleResult = await UserManager.AddToRoleAsync(user.Id, "RequirePasswordChange");
                        }
                        return response;
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
                else
                {
                    AddErrorsFromResult(validPass);
                }
            }

            var errors = ModelState.Where(item => item.Value.Errors.Any()).ToDictionary(
                    item => item.Key,
                    item => item.Value.Errors.Select(e => e.ErrorMessage).ToList()
                );

            response = JsonConvert.SerializeObject(new
            {
                result = "error",
                errors = errors
            });

            return response;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> BlockUser(string id)
        {
            return await ChangeBlockedState(id, true);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> UnblockUser(string id)
        {
            return await ChangeBlockedState(id, false);
        }

        /// <summary>
        /// Blocks or unblocks user with given id
        /// </summary>
        /// <param name="id">User's ID</param>
        /// <param name="shouldBeBlocked">If set to "true", user should be blocked, otherwise - unblocked</param>
        /// <returns>JSON with error message or resulting user state</returns>
        private async Task<ActionResult> ChangeBlockedState(string id, bool shouldBeBlocked)
        {
            string result = null;
            string msg = null;
            var user = await UserManager.FindByIdAsync(id);
            if (null == user)
            {
                result = "error";
                msg = "Пользователь не найден";
            }
            else if (user.UserName == User.Identity.Name)
            {
                result = "error";
                msg = "Вы не можете заблокировать самого себя";
            }
            else
            {
                bool wasBlocked = UserManager.IsInRole(user.Id, "Blocked");
                IdentityResult roleResult = null;
                if (wasBlocked && !shouldBeBlocked)
                    roleResult = await UserManager.RemoveFromRolesAsync(user.Id, "Blocked");
                else if (!wasBlocked && shouldBeBlocked)
                    roleResult = await UserManager.AddToRoleAsync(user.Id, "Blocked");
                if ((wasBlocked == shouldBeBlocked) || roleResult.Succeeded)
                {
                    result = "success";
                    msg = shouldBeBlocked ? "blocked" : "active";
                    if (roleResult != null && roleResult.Succeeded && shouldBeBlocked)
                        UserManager.UpdateSecurityStamp(user.Id);
                }
                else
                {
                    result = "error";
                    msg = String.Join(",", roleResult.Errors);
                }
            }
            return Json(new { result = result, msg = msg });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> EndAllSessions(string id)
        {
            string result = null;
            string msg = null;
            var user = await UserManager.FindByIdAsync(id);
            if (null == user)
            {
                result = "error";
                msg = "Пользователь не найден";
            }
            else
            {
                var stampResult = await UserManager.UpdateSecurityStampAsync(id);
                if (stampResult.Succeeded)
                {
                    result = "success";
                    msg = "Все сеансы пользователя успешно завершены";
                }
                else
                {
                    result = "error";
                    msg = String.Join(",", stampResult.Errors);
                }
            }
            return Json(new { result = result, msg = msg });
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private IdentityUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IdentityUserManager>();
            }
        }

        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
	}
}