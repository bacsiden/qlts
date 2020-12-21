using DK.Web.DependencyResolution;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace DK.Web
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        private string[] TheRoles;
        public CustomAuthorize(params string[] roles)
        {
            TheRoles = roles;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //var result = IoC.Container.GetInstance<IfwUserService>().Authorize(TheRoles);

            //if (result == Framework.Models.AuthorizeResultModel.Accepted) return;

            //if (result == Framework.Models.AuthorizeResultModel.NotLogedIn)
            //    filterContext.Result = new RedirectToRouteResult(new
            //        RouteValueDictionary(new { area = "", controller = "Account", action = "Login", returnUrl = filterContext.RequestContext.HttpContext.Request.RawUrl }));
            //else
            //    filterContext.Result = new RedirectToRouteResult(new
            //        RouteValueDictionary(new { area = "", controller = "Home", action = "AccessDenied" }));
        }
    }

    public class GZipOrDeflateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string acceptencoding = filterContext.HttpContext.Request.Headers["Accept-Encoding"];

            if (!string.IsNullOrEmpty(acceptencoding))
            {
                acceptencoding = acceptencoding.ToLower();
                var response = filterContext.HttpContext.Response;
                if (acceptencoding.Contains("gzip"))
                {
                    response.AppendHeader("Content-Encoding", "gzip");
                    response.Filter = new GZipStream(response.Filter,
                                          CompressionMode.Compress);
                }
                else if (acceptencoding.Contains("deflate"))
                {
                    response.AppendHeader("Content-Encoding", "deflate");
                    response.Filter = new DeflateStream(response.Filter,
                                      CompressionMode.Compress);
                }
            }
        }
    }
}