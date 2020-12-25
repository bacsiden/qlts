using DK.Application.Models;
using System.Collections.Generic;

namespace DK.Application.Repositories
{
    public interface ITaiSanRepository : IBaseRepository<TaiSan>
    {
        IEnumerable<TaiSan> Find(TaiSanSearchModel model);
    }
}