using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DK.Services.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DK.Services.Services
{
    public class BookService : BaseRepository<Book>, IBookService
    {
        public BookService(DefaultConnection db) : base(db)
        {
        }

        public override IQueryable<Book> GetList(Expression<Func<Book, bool>> predicate = null)
        {
            if (predicate == null) return db.Book.Where(m => !m.Deleted);

            return db.Book.Where(predicate).Where(m => !m.Deleted);
        }

        public override void Delete(int id)
        {
            var model = db.Book.Find(id);
            model.Deleted = true;
            Update(model);
        }

        public override async Task DeleteAsync(int id)
        {
            var model = await db.Book.FindAsync(id);
            model.Deleted = true;
            await UpdateAsync(model);
        }
    }
}
