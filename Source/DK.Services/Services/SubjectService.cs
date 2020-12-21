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
    public class SubjectService : BaseRepository<Subject>, ISubjectService
    {
        public SubjectService(DefaultConnection db) : base(db)
        {
        }

        public override IQueryable<Subject> GetList(Expression<Func<Subject, bool>> predicate = null)
        {
            if (predicate == null) return db.Subject.Where(m => !m.Deleted);

            return db.Subject.Where(predicate).Where(m => !m.Deleted);
        }

        public List<Subject> GetSubjectByClass(int classID)
        {
            var subject = from lst1 in db.Subject
                          join lst2 in db.ClassSubject on lst1.ID equals lst2.SubjectID
                          where !lst1.Deleted && !lst2.Deleted && lst2.ClassID == classID
                          select lst1;
            return subject.ToList();
        }

        public override void Delete(int id)
        {
            var model = db.Subject.Find(id);
            model.Deleted = true;
            Update(model);
        }

        public override async Task DeleteAsync(int id)
        {
            var model = await db.Subject.FindAsync(id);
            model.Deleted = true;
            await UpdateAsync(model);
        }
    }
}
