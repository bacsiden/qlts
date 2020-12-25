using System.Web;

namespace DK.Web.Utilities
{
    public static class Common
    {
        public static HttpContext HttpContext
        {
            get { return HttpContext.Current; }
        }
        public static System.Web.Mvc.UrlHelper Url
        {
            get { return new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext); }
        }
    }
}
