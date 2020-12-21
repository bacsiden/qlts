using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DK.Services.Models;
using System.Linq.Expressions;

namespace DK.Services.Services
{
    public interface ITimeLineService : Framework.Services.IBaseService<TimeLine>
    {
        List<TimeLine> GetListToAdd(int classID, DateTime fromDate, DateTime toDate);

        Task UpdateTimeLinesAsync(List<TimeLine> listTimeLine);

        Task ExportTimeLineAsync(List<TimeLine> lst, DateTime? fromDate, DateTime? toDate, int? classID);
    }
}
