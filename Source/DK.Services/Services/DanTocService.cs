using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DK.Services.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DK.Services.Services
{
    public class DanTocService : BaseRepository<DanToc>, IDanTocService
    {
        public DanTocService(DefaultConnection db) : base(db)
        {
        }
    }
}
