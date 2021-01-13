using DK.Application.Models;
using System.Collections.Generic;

namespace DK.Application.Repositories
{
    public interface IViewFieldRepository : IBaseRepository<ViewField>
    {
        List<ViewField> ListForTaiSan(string username);
    }
}