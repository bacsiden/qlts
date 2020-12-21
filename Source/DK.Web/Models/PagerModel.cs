using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DK.Web.Models
{
    public class PagerModel
    {
        public PagedList.IPagedList list { get; set; }
        public Func<int, string> generatePageUrl { get; set; }
        public PagedList.Mvc.PagedListRenderOptions options { get; set; }
    }
}