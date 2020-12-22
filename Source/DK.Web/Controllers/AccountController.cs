using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using DK.Web.Models;
using DK.Application.Repositories;

namespace DK.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ITaiSanRepository _TaiSanRepository;

        public AccountController(ITaiSanRepository taiSanRepository)
        {
            _TaiSanRepository = taiSanRepository;
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
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                //if (userService.Login(model.UserName, model.Password, model.RememberMe))
                //{
                //    if (string.IsNullOrEmpty(returnUrl))
                //        return RedirectToAction("Index", "Home");
                //    return Redirect(returnUrl);
                //}
                //else
                //{
                //    ModelState.AddModelError("UserName", "Sai mật khẩu hoặc tên đăng nhập.");
                //    return View(model);
                //}
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Sai mật khẩu hoặc tên đăng nhập.");
            return View(model);
        }
        public ActionResult Logout()
        {
            //userService.Logout();
            return RedirectToAction("Login", "Account");
        }
    }
}