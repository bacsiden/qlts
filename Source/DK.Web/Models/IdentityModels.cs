using DK.Application.Models;
using Microsoft.AspNet.Identity;
using MongoDB.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DK.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public bool IsSupperAdmin
        {
            get
            {
                return Roles?.Contains(RoleList.SupperAdmin) == true;
            }
        }

        public bool IsAdmin
        {
            get
            {
                return Roles?.Contains(RoleList.Admin) == true;
            }
        }

        public bool IsMember
        {
            get
            {
                return Roles == null || Roles.Count == 0;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, string> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}