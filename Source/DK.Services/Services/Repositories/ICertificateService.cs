using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DK.Services.Models;
using System.Linq.Expressions;

namespace DK.Services.Services
{
    public interface ICertificateService : Framework.Services.IBaseService<Certificate>
    {
        Task<string> Provision(int classID, List<int> studentIDs, int no);

        Task Download(int certificateID);

        Task DownloadManyAsync(List<Certificate> certificates);
    }
}
