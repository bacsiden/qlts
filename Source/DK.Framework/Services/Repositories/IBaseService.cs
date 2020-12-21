using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace DK.Framework.Services
{
    public interface IBaseService<T> where T : class
    {
        T Get(int id);

        T Get(Expression<Func<T, bool>> predicate);

        Task<T> GetAsync(int id);

        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetList(Expression<Func<T, bool>> predicate = null);

        T Add(T model);

        Task<T> AddAsync(T model);

        IEnumerable<T> AddRange(IEnumerable<T> list);

        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> list);

        T AddOrUpdate(T model);

        Task<T> AddOrUpdateAsync(T model);

        T Update(T model);

        Task<T> UpdateAsync(T model);

        void Delete(int id);

        Task DeleteAsync(int id);
    }
}
