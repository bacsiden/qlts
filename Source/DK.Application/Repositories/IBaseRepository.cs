using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DK.Application.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        T Get(Guid id);
        Task<T> GetAsync(Guid id);

        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        IMongoQueryable<T> Find(Expression<Func<T, bool>> predicate);

        Task<T> AddAsync(T model);

        Task AddRangeAsync(IEnumerable<T> list);

        Task<T> UpsertAsync(T model);

        Task<T> UpdateAsync(T model);

        Task DeleteAsync(Guid id);
        Task DeleteAsync(string id);
        Task DeleteManyAsync(string fieldName, object value);
        Task SetAsync(Guid id, string fieldName, dynamic value);
        Task SetAsync(string id, string fieldName, dynamic value);

        IFindFluent<T, T> Find(IEnumerable<Guid> Ids);
    }
}
