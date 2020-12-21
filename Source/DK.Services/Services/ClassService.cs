using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using DK.Services.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DK.Services.Services
{
    public class ClassService : BaseRepository<Class>, IClassService
    {
        public ClassService(DefaultConnection db) : base(db)
        {
        }

        public override IQueryable<Class> GetList(Expression<Func<Class, bool>> predicate = null)
        {
            if (predicate == null) return db.Class.Where(m => !m.Deleted);

            return db.Class.Where(predicate).Where(m => !m.Deleted);
        }
        public override async Task<Class> AddOrUpdateAsync(Class model)
        {
            if (model.ID == 0)
            {
                model = await AddAsync(model);
                var kichienthuat = await db.Subject.FirstOrDefaultAsync(m => m.Code == "KyChienThuat");
                var ctd = await db.Subject.FirstOrDefaultAsync(m => m.Code == "CTD");
                var dieulenh = await db.Subject.FirstOrDefaultAsync(m => m.Code == "DieuLenh");

                if (kichienthuat != null) await AddSubjectAsync(classID: model.ID, subjectID: kichienthuat.ID);
                if (ctd != null) await AddSubjectAsync(classID: model.ID, subjectID: ctd.ID);
                if (dieulenh != null) await AddSubjectAsync(classID: model.ID, subjectID: dieulenh.ID);

                return model;
            }
            return await UpdateAsync(model);
        }
        public override void Delete(int id)
        {
            var model = db.Class.Find(id);
            model.Deleted = true;
            Update(model);
        }

        public override async Task DeleteAsync(int id)
        {
            var model = await db.Class.FindAsync(id);
            model.Deleted = true;
            await UpdateAsync(model);
        }

        public async Task AddStudentAsync(int classID, int studentID)
        {
            var model = db.StudentClass.FirstOrDefault(m => m.ClassID == classID && m.StudentID == studentID);
            if (model != null)
            {
                model.Deleted = false;
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                model = new StudentClass() { ClassID = classID, StudentID = studentID, Deleted = false };
                db.StudentClass.Add(model);
            }

            await db.SaveChangesAsync();
        }

        public async Task RemoveStudentAsync(int classID, int studentID)
        {
            var model = db.StudentClass.FirstOrDefault(m => m.ClassID == classID && m.StudentID == studentID);
            if (model != null)
            {
                model.Deleted = true;
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;

                await db.SaveChangesAsync();
            }
        }

        public async Task AddSubjectAsync(int classID, int subjectID)
        {
            var model = db.ClassSubject.FirstOrDefault(m => m.ClassID == classID && m.SubjectID == subjectID);
            if (model != null)
            {
                model.Deleted = false;
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                model = new ClassSubject() { ClassID = classID, SubjectID = subjectID, Deleted = false };
                db.ClassSubject.Add(model);
            }

            await db.SaveChangesAsync();
        }

        public async Task RemoveSubjectAsync(int classID, int subjectID)
        {
            var model = db.ClassSubject.FirstOrDefault(m => m.ClassID == classID && m.SubjectID == subjectID);
            if (model != null)
            {
                model.Deleted = true;
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;

                await db.SaveChangesAsync();
            }
        }

        public async Task<List<Student>> GetListStudentsAsync(int classID)
        {
            return await db.StudentClass.Where(m => m.ClassID == classID && !m.Deleted && !m.Student.Deleted).Select(m => m.Student
            ).ToListAsync();
        }

        public async Task<List<Subject>> GetListSubjectsAsync(int classID)
        {
            return await db.ClassSubject.Where(m => m.ClassID == classID && !m.Deleted && !m.Subject.Deleted).Select(m => m.Subject
            ).ToListAsync();
        }

        public async Task<List<StudentClass>> GetListStudentClassesAsync(int classID)
        {
            return await db.StudentClass.Where(m => m.ClassID == classID && !m.Deleted && !m.Class.Deleted).ToListAsync();
        }

        public async Task<List<ClassSubject>> GetListClassSubjectsAsync(int classID)
        {
            return await db.ClassSubject.Where(m => m.ClassID == classID && !m.Deleted && !m.Subject.Deleted).ToListAsync();
        }

        public async Task<string> GenerateMSSV()
        {
            var student = await db.Student.OrderByDescending(m => m.ID).FirstOrDefaultAsync();

            return "QP" + (student != null ? student.ID : 1).ToString().PadLeft(4, '0');
        }
    }
}
