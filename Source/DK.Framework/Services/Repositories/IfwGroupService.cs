using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DK.Framework.Models;

namespace DK.Framework.Services
{
    public interface IfwGroupService : IBaseService<fwGroup>
    {
        Task UpdateRolesAsync(int groupID, List<int> roleIDs);

        Task AddUserAsync(int groupID, int userID);

        Task RemoveUserAsync(int groupID, int userID);
    }
}
