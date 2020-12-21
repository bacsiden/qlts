using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DK.Services.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DK.Framework;
using System.IO;

namespace DK.Services.Services
{
    public class StudentService : BaseRepository<Student>, IStudentService
    {
        public StudentService(DefaultConnection db) : base(db)
        {
        }

        public override IQueryable<Student> GetList(Expression<Func<Student, bool>> predicate = null)
        {
            if (predicate == null) return db.Student.Where(m => !m.Deleted);

            return db.Student.Where(predicate).Where(m => !m.Deleted);
        }

        public IQueryable<Student> GetList(Expression<Func<StudentClass, bool>> predicate)
        {
            return db.StudentClass.Where(predicate).Where(m => !m.Student.Deleted).Select(m => m.Student);
        }

        public override void Delete(int id)
        {
            var model = db.Student.Find(id);
            model.Deleted = true;
            Update(model);
        }

        public override async Task DeleteAsync(int id)
        {
            var model = await db.Student.FindAsync(id);
            model.Deleted = true;
            await UpdateAsync(model);
        }

        public async Task ExportStudentsAsync(List<Student> students, int classID)
        {
            var table = new List<List<DocField>>();
            var _class = await db.Class.FindAsync(classID);
            var outputFilename = $"{StringHelper.CreateURLParam(_class.Title)} - K{_class.Khoa}.docx";
            var fullOutputFilename = TemplateFolder + outputFilename;
            var fields = new List<DocField>() { new DocField { Name = "Lop", Value = $"{_class.Title}-K{_class.Khoa}" } };

            var i = 0;
            foreach (var item in students.OrderBy(m => m.LastName))
            {
                var row = new List<DocField>();
                row.Add(new DocField { Name = "No", Value = (++i).ToString() });
                row.Add(new DocField { Name = "Name", Value = item.FullName });
                row.Add(new DocField { Name = "CapBac", Value = $"{item.CapBac}" });
                row.Add(new DocField { Name = "NhapNgu", Value = $"{item.NgayNhapNgu}" });
                row.Add(new DocField { Name = "ChucVu", Value = $"{item.ChucVu}" });
                row.Add(new DocField { Name = "VaoDangChinhThuc", Value = $"{item.NgayVaoDang}" });
                row.Add(new DocField { Name = "VaoDoan", Value = $"{item.NgayVaoDang}" });
                row.Add(new DocField { Name = "QuaTruong", Value = $"{item.QuaTruong}" });
                row.Add(new DocField { Name = "VanHoaSucKhoe", Value = $"{item.TrinhDoVanHoa + Environment.NewLine + item.SucKhoe}" });
                row.Add(new DocField { Name = "TPGiaDinh", Value = $"{item.ThanhPhanGiaDinh}" });
                row.Add(new DocField { Name = "DanTocTonGiao", Value = $"{item.DanToc + Environment.NewLine + item.TonGiao}" });
                row.Add(new DocField { Name = "HoTenLienQuan", Value = $"{item.GiaDinh}" });
                row.Add(new DocField { Name = "SDT", Value = $"{item.SDT}" });
                row.Add(new DocField { Name = "GhiChu", Value = $"{item.GhiChu}" });

                table.Add(row);
            }

            Docx.FillSingleTable(TemplateFolder + "Students.docx", fullOutputFilename, table, fields);

            using (StreamReader stream = new StreamReader(fullOutputFilename))
            {
                await SendToBrowser(stream.BaseStream, "application/vnd.ms-word", outputFilename);
            }
        }
    }
}
