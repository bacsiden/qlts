using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DK.Framework.Models;

namespace DK.Framework.Services
{
    public class fwNotificationService : BaseRepository<fwNotification>, IfwNotificationService
    {
        public fwNotificationService(DefaultConnection db) : base(db)
        {
        }
    }
}
