﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DK.Framework.Models;
using System.Linq.Expressions;

namespace DK.Framework.Services
{
    public interface IfwMenuService : IBaseService<fwMenu>
    {
        Task UpdateRolesAsync(int menuID, List<int> roleIDs);

        IQueryable<fwMenu> GetList(int userID);
    }
}
