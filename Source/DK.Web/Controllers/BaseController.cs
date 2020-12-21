using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FlexCel.XlsAdapter;
using FlexCel.Render;
using FlexCel.Report;
using FlexCel.Core;
using System.Threading.Tasks;
using PagedList;
using DK.Web.DependencyResolution;

namespace DK.Web.Controllers
{
    public class BaseController : Controller
    {
        public string DateTimeReport { get { return DateTime.Now.ToString("_dd-MM-yyyy"); } }
        protected string ReportTempleFolder
        {
            get
            {
                return Server.MapPath(@"~/ReportTemplates\");
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
        public ActionResult AccessDenied()
        {
            return View("_AccessDenied");
        }
        public List<T> JsonDeserializeListObject<T>(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            return new List<T>();

            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(value);
        }
        public List<T> CreateEmptyGenericList<T>(T example)
        {
            return new List<T>();
        }
        public string NumberToText(decimal number)
        {
            return Functions.NumberToTextVN(number);
        }
        public void ShowMessage(string message, bool isSuccess = true)
        {
            //if (isSuccess)
            //{
            //    SessionUtilities.Set(Constant.SESSION_MessageSuccess, message);
            //}
            //else
            //{
            //    SessionUtilities.Set(Constant.SESSION_MessageError, message);
            //}

        }
        public ActionResult HideModalReload()
        {
            //return Content("<html><head><script>location.reload();</script></head></html>");
            return JavaScript(@"$('#GeneralModal').modal('hide'); location.reload();");
        }
        public void SendToBrowser(ExcelFile xlsx, string filename)
        {
            using (MemoryStream XlsStream = new MemoryStream())
            {
                xlsx.Save(XlsStream);
                SendToBrowser(XlsStream, "application/excel", filename);
            }
        }
        public void SendToBrowser(MemoryStream OutStream, string MimeType, string FileName)
        {
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);
            byte[] MemData = OutStream.ToArray();
            Response.AddHeader("Content-Length", Convert.ToString(MemData.Length, CultureInfo.InvariantCulture));
            Response.ContentType = MimeType;
            Response.BinaryWrite(MemData);
            Response.End();
        }
        public void ShowSuccessMessage(string mess)
        {
            ViewBag.Success = mess;
        }
        public void ShowErrorMessage(string mess)
        {
            ViewBag.Error = mess;
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult LastView(string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
                return RedirectToAction("Index");
            else
                return Redirect(returnUrl);
        }
    }
}