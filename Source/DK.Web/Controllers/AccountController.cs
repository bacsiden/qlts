using DK.Application;
using DK.Application.Repositories;
using DK.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DK.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ITaiSanRepository _TaiSanRepository;
        //private readonly TaiSanService _taiSanService;


        public AccountController(ITaiSanRepository taiSanRepository)
        {
            _TaiSanRepository = taiSanRepository;
            //_taiSanService = new TaiSanService(taiSanRepository);
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
            //_taiSanService.Import();
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
    }
}