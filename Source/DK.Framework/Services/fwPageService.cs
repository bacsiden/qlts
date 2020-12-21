using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DK.Framework.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DK.Framework.Services
{
    public class fwPageService : BaseRepository<fwPage>, IfwPageService
    {
        public fwPageService(DefaultConnection db) : base(db)
        {
        }
    }
}
