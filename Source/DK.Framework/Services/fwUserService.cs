using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DK.Framework.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace DK.Framework.Services
{
    public class fwUserService : BaseRepository<fwUser>, IfwUserService
    {

        public fwUserService(DefaultConnection db) : base(db)
        {
        }

        private static HttpContext HttpContext { get { return HttpContext.Current; } }

        public fwUser CurrentUser
        {
            get
            {
                if (!SessionUtilities.Exist(Constant.Session_CurrentUser))
                    if (HttpContext.Request.Cookies.Get(Constant.Session_CurrentUser) != null)
                    {
                        var id = int.Parse(HttpContext.Request.Cookies.Get(Constant.Session_CurrentUser).Value);
                        var u = db.fwUser.Find(id);
                        SessionUtilities.Set(Constant.Session_CurrentUser, u);
                        return u;
                    }
                    else return null;
                return (fwUser)SessionUtilities.Get(Constant.Session_CurrentUser);
            }
        }

        public AuthorizeResultModel Authorize(params string[] roles)
        {
            var user = CurrentUser;
            if (user == null) return AuthorizeResultModel.NotLogedIn;
            if (roles == null || roles.Length == 0) return AuthorizeResultModel.Accepted;
            if (!UserInRole(user.ID, roles)) return AuthorizeResultModel.Denied;
            return AuthorizeResultModel.Accepted;
        }

        public bool Login(string username, string pass, bool rememberMe = true)
        {
            var user = db.fwUser.FirstOrDefault(m => m.UserName == username);
            if (user == null || user.Pass != pass) return false;
            if (rememberMe)
                HttpContext.Response.Cookies.Add(new System.Web.HttpCookie(Constant.Session_CurrentUser, user.ID.ToString()));
            SessionUtilities.Add(Constant.Session_CurrentUser, user);
            return true;
        }

        public void Logout()
        {
            if (HttpContext.Request.Cookies[Constant.Session_CurrentUser] != null)
            {
                var c = new System.Web.HttpCookie(Constant.Session_CurrentUser);
                c.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Response.Cookies.Add(c);
            }
            SessionUtilities.Remove(Constant.Session_CurrentUser);
        }


        public bool UserInRole(params string[] roles)
        {
            if (CurrentUser == null) return false;
            return UserInRole(CurrentUser.ID, roles);
        }

        public bool UserInRole(fwUser user, params string[] roles)
        {
            if (user == null) return false;
            return UserInRole(user.ID, roles);
        }

        public bool UserInRole(int userID, params string[] roles)
        {
            string query = @"select r.* from fwRole r 
inner join fwRolefwGroups rg on rg.fwRole_ID=r.ID
inner join fwUserfwGroups ug on rg.fwGroup_ID=ug.fwGroup_ID
where ug.fwUser_ID=" + userID;
            var currentRoles = db.fwRole.SqlQuery(query).Select(m => m.Code).ToList();
            bool kq = false;
            foreach (var role in currentRoles)
            {
                if (roles.Contains(role))
                {
                    kq = true;
                    break;
                }
            }
            return kq;
        }
    }
}
