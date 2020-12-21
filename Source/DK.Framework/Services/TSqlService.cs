using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace DK.Framework.Services
{
    public class TSqlService
    {
        private readonly Models.DefaultConnection db;

        public TSqlService(Models.DefaultConnection db)
        {
            this.db = db;
        }

        public async Task<int> DeleteByKeyAsync(object key, string tableName, string columnKeyName = "ID")
        {
            return await db.Database.ExecuteSqlCommandAsync($"delete from {tableName} where {columnKeyName}=@{columnKeyName}", new SqlParameter($"@{columnKeyName}", key));
        }
    }
}