using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DK.Framework.Models;
using System.Linq.Expressions;

namespace DK.Framework.Services
{
    public interface IfwUserService : IBaseService<fwUser>
    {
        fwUser CurrentUser { get; }

        AuthorizeResultModel Authorize(params string[] roles);

        bool Login(string username, string pass, bool rememberMe = true);

        void Logout();

        bool UserInRole(params string[] roles);

        bool UserInRole(fwUser user, params string[] roles);

        bool UserInRole(int userID, params string[] roles);
    }
}
