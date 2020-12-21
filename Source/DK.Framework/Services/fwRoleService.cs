using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DK.Framework.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DK.Framework.Services
{
    public class fwRoleService : BaseRepository<fwRole>, IfwRoleService
    {
        public fwRoleService(DefaultConnection db) : base(db)
        {
        }
    }
}
