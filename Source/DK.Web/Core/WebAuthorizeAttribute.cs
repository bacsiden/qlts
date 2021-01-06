using DK.Web.DependencyResolution;
using DK.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DK.Web.Core
{
    public class WebAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var userManager = IoC.Container.GetInstance<ApplicationUserManager>();
                var currentUser = userManager.FindById(filterContext.HttpContext.User.Identity.GetUserId());

                if (CanAccess(currentUser, Roles))
                    return;
                else
                    filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { area = "", controller = "Home", action = "Index" }));
            }

            base.OnAuthorization(filterContext);
        }

        private bool CanAccess(ApplicationUser user, string roles)
        {
            if (string.IsNullOrEmpty(roles)) return true;

            var listRoles = roles.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var role in listRoles)
            {
                if (user.Roles.Contains(role)) return true;
            }

            return false;
        }
    }
}