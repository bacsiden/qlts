using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DK.Services.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using FlexCel.Report;
using System.IO;
using FlexCel.XlsAdapter;

namespace DK.Services.Services
{
    public class PointService : BaseRepository<StudentClassSubject>, IPointService
    {
        public PointService(DefaultConnection db) : base(db)
        {
        }

        public async Task SaveManyAsync(List<StudentClassSubject> StudentClassSubjects)
        {
            db.StudentClassSubject.AddRange(StudentClassSubjects.Where(m => m.ID == 0));
            await db.SaveChangesAsync();
            foreach (var item in StudentClassSubjects.Where(m => m.ID != 0))
            {
                await UpdateAsync(item);
            }
        }

        public async Task SaveRenLuyenAsync(List<StudentClass> studentClasses)
        {
            foreach (var item in studentClasses)
            {
                db.Set<StudentClass>().AddOrUpdate(item);
            }
            await db.SaveChangesAsync();
        }

        private Tuple<string, string> GetXepLoaiTitle(double? avr)
        {
            if (avr.HasValue)
            {
                if (avr.Value >= 8) return new Tuple<string, string>(item1: "G", item2: "Giỏi");
                if (avr.Value >= 7) return new Tuple<string, string>(item1: "K", item2: "Khá");
                if (avr.Value >= 6) return new Tuple<string, string>(item1: "TBK", item2: "TB khá");
                if (avr.Value >= 5) return new Tuple<string, string>(item1: "TB", item2: "Trung bình");
                return new Tuple<string, string>(item1: "Y", item2: "Yếu");
            }
            return new Tuple<string, string>(null, null);
        }
        private async Task SetCommonPoint(List<StudentClassSubject> studentClassSubject, StudentClass studentClass, List<double> points)
        {
            double? avr = null;
            if (points.Count > 0)
            {
                avr = points.Sum() / points.Count;
            }
            studentClass.TBKT = avr;
            var xeploai = GetXepLoaiTitle(avr);
            studentClass.HocLuc = xeploai.Item1;

            var kichienthuat = db.Subject.FirstOrDefault(m => m.Code == "KyChienThuat");
            var ctd = db.Subject.FirstOrDefault(m => m.Code == "CTD");
            var dieulenh = db.Subject.FirstOrDefault(m => m.Code == "DieuLenh");

            var point = studentClassSubject.FirstOrDefault(m => m.StudentClassID == studentClass.ID && m.SubjectID == kichienthuat.ID);
            if (point != null && point.Point.HasValue) studentClass.KyChienThuat = point.Point.Value;

            point = studentClassSubject.FirstOrDefault(m => m.StudentClassID == studentClass.ID && m.SubjectID == ctd.ID);
            if (point != null && point.Point.HasValue) studentClass.CTD = point.Point.Value;

            point = studentClassSubject.FirstOrDefault(m => m.StudentClassID == studentClass.ID && m.SubjectID == dieulenh.ID);
            if (point != null && point.Point.HasValue) studentClass.DieuLenh = point.Point.Value;

            if (studentClass.KyChienThuat.HasValue && studentClass.CTD.HasValue && studentClass.DieuLenh.HasValue && avr.HasValue)
            {
                var p = ((studentClass.KyChienThuat.Value + studentClass.CTD.Value + studentClass.DieuLenh.Value) / 3 + avr.Value * 2) / 3;
                studentClass.DiemTrungBinh = p;
                xeploai = GetXepLoaiTitle(p);
                studentClass.XepLoaiTotNghiep = xeploai.Item2;
            }

            db.Set<StudentClass>().AddOrUpdate(studentClass);
            await db.SaveChangesAsync();
        }
        public async Task CalculateAsync(IClassService classService, List<Student> students, List<Subject> subjects, List<StudentClassSubject> points, Class classModel)
        {
            var subs = subjects.Where(m => m.Code == null).ToList();
            foreach (var student in students)
            {
                var studentClasses = await classService.GetListStudentClassesAsync(classModel.ID);
                var pointDs = new List<double>();
                var studentClass = studentClasses.First(m => m.StudentID == student.ID);
                foreach (var subject in subs)
                {
                    var point = points.FirstOrDefault(m => m.StudentClassID == studentClass.ID && m.SubjectID == subject.ID);
                    if (point != null && point.Point != null)
                        pointDs.Add(point.Point.Value);
                }

                await SetCommonPoint(points, studentClass, pointDs);
            }
        }

        public async Task<List<StudentClassSubject>> ViewPoints(int classID)
        {
            var points = await db.StudentClass
                .Where(m => m.ClassID == classID && !m.Deleted && !m.Class.Deleted && !m.Student.Deleted)
                .SelectMany(m => m.StudentClassSubject)
                .Where(m => !m.Subject.Deleted).ToListAsync();

            foreach (var item in await db.StudentClass
                .Where(m => m.ClassID == classID && !m.Deleted && !m.Class.Deleted && !m.Student.Deleted).ToListAsync())
            {
                foreach (var classSubject in await db.ClassSubject
                .Where(m => m.ClassID == classID && !m.Deleted && !m.Class.Deleted && !m.Subject.Deleted).ToListAsync())
                {
                    if (points.Any(m => m.StudentClassID == item.ID && m.SubjectID == classSubject.SubjectID)) continue;

                    var obj = new StudentClassSubject();
                    obj.StudentClass = item;
                    obj.StudentClassID = item.ID;
                    obj.SubjectID = classSubject.SubjectID;
                    obj.Subject = classSubject.Subject;
                    points.Add(obj);
                }
            }

            return points;
        }

        public async Task<List<StudentClassSubject>> ViewPoints(int classID, int subjectID)
        {
            var points = await db.StudentClass
                .Where(m => m.ClassID == classID && !m.Deleted && !m.Class.Deleted && !m.Student.Deleted)
                .SelectMany(m => m.StudentClassSubject)
                .Where(m => m.SubjectID == subjectID && !m.Subject.Deleted).ToListAsync();

            foreach (var item in await db.StudentClass
                .Where(m => m.ClassID == classID && !m.Deleted && !m.Class.Deleted && !m.Student.Deleted).ToListAsync())
            {
                if (points.Any(m => m.StudentClassID == item.ID && m.SubjectID == subjectID)) continue;

                var obj = new StudentClassSubject();
                obj.StudentClass = item;
                obj.StudentClassID = item.ID;
                obj.SubjectID = subjectID;
                obj.Subject = await db.Subject.FindAsync(subjectID);
                points.Add(obj);
            }

            return points;
        }

        public async Task ExportPointsAsync(List<Student> students, List<Subject> subjects, List<StudentClassSubject> points, Class classModel)
        {
            subjects = subjects.Where(m => m.Code == null).ToList();
            var classTitle = $"LỚP {classModel.Title.ToUpper()} KHÓA {classModel.Khoa}";
            using (FlexCelReport fr = new FlexCelReport(true))
            {
                var studentClasses = await db.StudentClass.Where(m => m.ClassID == classModel.ID && !m.Deleted).ToListAsync();
                fr.AddTable("subject", subjects);
                fr.SetValue("ClassTitle", classTitle);
                var xls = new XlsFile(true);
                xls.Open(TemplateFolder + "Points.xlsx");
                fr.Run(xls);
                var cellFormat = xls.GetRowFormat(9);
                for (int i = 0; i < students.Count; i++)
                {
                    var rowIndex = i + 9;
                    xls.SetRowFormat(rowIndex, cellFormat, false);
                    xls.SetCellValue(rowIndex, 1, i + 1);
                    xls.SetCellValue(rowIndex, 2, students[i].FullName);
                    var pointDs = new List<double>();
                    var studentClass = studentClasses.First(m => m.StudentID == students[i].ID);
                    var colIndex = 0;
                    for (int j = 0; j < subjects.Count; j++)
                    {
                        colIndex = j + 3;
                        var point = points.FirstOrDefault(m => m.StudentClassID == studentClass.ID && m.SubjectID == subjects[j].ID);
                        xls.SetCellValue(rowIndex, colIndex, point == null || point.Point == null ? null : point.Point.Value.ToString("N1"));
                        if (point != null && point.Point != null)
                            pointDs.Add(point.Point.Value);
                    }
                    colIndex = subjects.Count + 3;
                    xls.SetCellValue(rowIndex, colIndex++, studentClass.TBKT.HasValue ? studentClass.TBKT.Value.ToString("N1") : null);
                    xls.SetCellValue(rowIndex, colIndex++, studentClass.HocLuc);
                    xls.SetCellValue(rowIndex, colIndex++, studentClass.RenLuyen);
                    xls.SetCellValue(rowIndex, colIndex++, studentClass.KyChienThuat.HasValue ? studentClass.KyChienThuat.Value.ToString("N1") : null);
                    xls.SetCellValue(rowIndex, colIndex++, studentClass.CTD.HasValue ? studentClass.CTD.Value.ToString("N1") : null);
                    xls.SetCellValue(rowIndex, colIndex++, studentClass.DieuLenh.HasValue ? studentClass.DieuLenh.Value.ToString("N1") : null);
                    xls.SetCellValue(rowIndex, colIndex++, studentClass.DiemTrungBinh.HasValue ? studentClass.DiemTrungBinh.Value.ToString("N1") : null);
                    xls.SetCellValue(rowIndex, colIndex++, studentClass.XepLoaiTotNghiep);

                    if (i > 0)
                    {
                        for (int j = 0; j < subjects.Count + 10; j++)
                        {
                            var fmt = xls.GetCellFormat(9, j + 1);
                            xls.SetCellFormat(rowIndex, j + 1, fmt);
                        }
                    }
                }
                using (MemoryStream XlsStream = new MemoryStream())
                {
                    xls.Save(XlsStream);
                    await SendToBrowser(XlsStream, "application/excel", $"Bang-diem-{DK.Framework.StringHelper.CreateURLParam(classModel.Title)}.xlsx");
                }
            }
        }
    }
}
