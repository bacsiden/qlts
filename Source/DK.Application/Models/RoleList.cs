using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DK.Application.Models
{
    public class RoleList
    {
        public const string SupperAdmin = "SupperAdmin";
        public const string Admin = "Admin";
        public const string Manage = SupperAdmin + "," + Admin;


        public static List<string> GetAll()
        {
            return new List<string> {
                SupperAdmin, Admin
            };
        }
    }
}
