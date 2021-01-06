using DK.Application;
using DK.Application.Models;
using DK.Application.Repositories;
using DK.Web.Core;
using DK.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DK.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ITaiSanRepository _TaiSanRepository;
        private readonly ITaiSanService _taiSanService;

        public AccountController(ITaiSanRepository taiSanRepository, ITaiSanService taiSanService)
        {
            _TaiSanRepository = taiSanRepository;
            _taiSanService = taiSanService;
        }

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            if (result == SignInStatus.Success)
            {
                returnUrl = returnUrl ?? "/";
                return Redirect(returnUrl);
            }

            ModelState.AddModelError("", "Sai mật khẩu hoặc tên đăng nhập.");
            return View(model);
        }
        public ActionResult Logout()
        {
            Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(ManageUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(CurrentUserId);
                if (user == null) return null;

                var result = await UserManager.ChangePasswordAsync(CurrentUserId, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("Settings");
                }
                AddErrors(result);
            }
            return View(model);
        }

        [WebAuthorize(Roles = RoleList.Manage)]
        public ActionResult Employees()
        {
            var users = UserManager.Users.Where(m => !m.Roles.Contains(RoleList.SupperAdmin));
            if (!User.IsInRole(RoleList.SupperAdmin))
            {
                users = users.Where(m => !m.Roles.Contains(RoleList.Admin));
            }

            return View(users.ToList());
        }

        [WebAuthorize(Roles = RoleList.Manage)]
        public ActionResult AddEmployee()
        {
            return View();
        }

        [WebAuthorize(Roles = RoleList.Manage)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddEmployee(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.UserName,
                        Email = $"{model.UserName}@localhost.localhost",
                    };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        if (model.IsAdmin)
                        {
                            result = await UserManager.AddToRoleAsync(user.Id, RoleList.Admin);
                        }

                        if (result.Succeeded)
                        {
                            ShowSuccessMessage("Add employee successfully.");
                            return RedirectToAction("Employees");
                        }
                    }

                    var usernameTaken = string.Format("{0} is already taken.", user.UserName);
                    if (result.Errors.Any(m => m.Contains(usernameTaken)))
                    {
                        ModelState.AddModelError("", "Tên tài khoản đã tồn tại. Vui lòng chọn tài khoản khác.");
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra.");
            }
            return View(model);
        }

        [WebAuthorize(Roles = RoleList.Manage)]
        public ActionResult Delete(string id)
        {
            try
            {
                var user = UserManager.FindById(id);
                UserManager.Delete(user);
            }
            catch (Exception)
            {

            }

            return RedirectToAction("Employees");
        }

        [WebAuthorize(Roles = RoleList.Manage)]
        public async Task<JsonResult> ToggleRole(string id)
        {
            try
            {
                var user = UserManager.FindById(id);
                if (user.IsMember)
                {
                    user.Roles.Add(RoleList.Admin);
                }
                else
                {
                    user.Roles.Remove(RoleList.Admin);
                }

                await UserManager.UpdateAsync(user);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}