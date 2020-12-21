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
    public class fwMenuService : BaseRepository<fwMenu>, IfwMenuService
    {
        public fwMenuService(DefaultConnection db) : base(db)
        {
        }

        public async Task UpdateRolesAsync(int menuID, List<int> roleIDs)
        {
            var model = await db.fwMenu.FindAsync(menuID);
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

        public IQueryable<fwMenu> GetList(int userID)
        {
            string query = $@"select distinct m.* from fwMenu m
inner join fwMenufwRoles mr on mr.fwMenu_ID=m.ID
inner join fwRolefwGroups rg on rg.fwRole_ID=mr.fwRole_ID
inner join fwUserfwGroups ug on ug.fwGroup_ID=rg.fwGroup_ID
where ug.fwUser_ID={userID} and m.Actived=1 order by [Order]";

            return db.fwMenu.SqlQuery(query).AsQueryable();
        }
    }
}
