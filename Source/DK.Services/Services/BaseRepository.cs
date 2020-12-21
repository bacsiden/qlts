using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DK.Services.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Web;
using System.Globalization;

namespace DK.Services.Services
{
    public abstract class BaseRepository<T> where T : class
    {
        protected readonly string TemplateFolder = HttpContext.Current.Server.MapPath("~/ReportTemplates\\");
        protected readonly DefaultConnection db;

        public BaseRepository(DefaultConnection db)
        {
            this.db = db;
        }
        protected async Task SendToBrowser(Stream OutStream, string MimeType, string FileName)
        {
            var ms = new MemoryStream();
            if (!(OutStream is MemoryStream))
            {
                await OutStream.CopyToAsync(ms);
            }
            else
                ms = OutStream as MemoryStream;
            var response = HttpContext.Current.Response;
            response.Clear();
            response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);
            byte[] MemData = ms.ToArray();
            response.AddHeader("Content-Length", Convert.ToString(MemData.Length, CultureInfo.InvariantCulture));
            response.ContentType = MimeType;
            response.BinaryWrite(MemData);
            response.End();
        }
        protected List<Tx> CreateEmptyGenericList<Tx>(Tx example)
        {
            return new List<Tx>();
        }
        public T Get(int id)
        {
            return db.Set<T>().Find(id);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return db.Set<T>().FirstOrDefault(predicate);
        }

        public async Task<T> GetAsync(int id)
        {
            return await db.Set<T>().FindAsync(id);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await db.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public virtual IQueryable<T> GetList(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ? db.Set<T>().AsQueryable() : db.Set<T>().Where(predicate);
        }

        public virtual T Add(T model)
        {
            model = db.Set<T>().Add(model);
            db.SaveChanges();
            return model;
        }

        public virtual async Task<T> AddAsync(T model)
        {
            model = db.Set<T>().Add(model);
            await db.SaveChangesAsync();
            return model;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> list)
        {
            list = db.Set<T>().AddRange(list);
            db.SaveChanges();
            return list;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> list)
        {
            list = db.Set<T>().AddRange(list);
            await db.SaveChangesAsync();
            return list;
        }

        public T AddOrUpdate(T model)
        {
            if ((int)typeof(T).GetProperty("ID").GetValue(model) == 0)
                return Add(model);
            else
                return Update(model);
        }

        public virtual async Task<T> AddOrUpdateAsync(T model)
        {
            if ((int)typeof(T).GetProperty("ID").GetValue(model) == 0)
                return await AddAsync(model);
            else
                return await UpdateAsync(model);
        }

        public T Update(T model)
        {
            db.Set<T>().AddOrUpdate(model);
            db.SaveChanges();
            return model;
        }

        public async Task<T> UpdateAsync(T model)
        {
            db.Set<T>().AddOrUpdate(model);
            await db.SaveChangesAsync();
            return model;
        }

        public virtual void Delete(int id)
        {
            var model = db.Set<T>().Find(id);
            if (model != null)
            {
                db.Set<T>().Remove(model);
                db.SaveChanges();
            }
        }

        public virtual async Task DeleteAsync(int id)
        {
            var model = await db.Set<T>().FindAsync(id);
            if (model != null)
            {
                db.Set<T>().Remove(model);
                await db.SaveChangesAsync();
            }
        }
    }
}