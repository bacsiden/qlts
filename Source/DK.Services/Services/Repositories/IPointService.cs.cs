using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DK.Services.Models;

namespace DK.Services.Services
{
    public interface IPointService : Framework.Services.IBaseService<StudentClassSubject>
    {
        Task SaveManyAsync(List<StudentClassSubject> StudentClassSubjects);

        Task SaveRenLuyenAsync(List<StudentClass> StudentClasses);

        Task<List<StudentClassSubject>> ViewPoints(int classID);

        Task<List<StudentClassSubject>> ViewPoints(int classID, int subjectID);

        Task CalculateAsync(IClassService classService, List<Student> students, List<Subject> subjects, List<StudentClassSubject> points, Class classModel);

        Task ExportPointsAsync(List<Student> students, List<Subject> subjects, List<StudentClassSubject> points, Class classModel);
    }
}
