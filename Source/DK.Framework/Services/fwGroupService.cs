using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DK.Framework.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DK.Framework.Services
{
    public class fwGroupService : BaseRepository<fwGroup>, IfwGroupService
    {
        public fwGroupService(DefaultConnection db) : base(db)
        {
        }

        public async Task UpdateRolesAsync(int groupID, List<int> roleIDs)
        {
            var model = await db.fwGroup.FindAsync(groupID);
            foreach (var item in model.fwRole.ToList())
            {
                if (!roleIDs.Contains(item.ID))
                    model.fwRole.Remove(item);
            }

            var lstRole = await db.fwRole.ToListAsync();
            foreach (var item in roleIDs)
            {
                var role = lstRole.FirstOrDefault(m => m.ID == item);
                model.fwRole.Add(role);
            }
            await db.SaveChangesAsync();
        }

        public async Task AddUserAsync(int groupID, int userID)
        {
            var user = await db.fwUser.FindAsync(userID);
            var group = await db.fwGroup.FindAsync(groupID);
            group.fwUser.Add(user);

            await db.SaveChangesAsync();
        }

        public async Task RemoveUserAsync(int groupID, int userID)
        {
            var user = await db.fwUser.FindAsync(userID);
            var group = await db.fwGroup.FindAsync(groupID);
            group.fwUser.Remove(user);

            await db.SaveChangesAsync();
        }
    }
}
