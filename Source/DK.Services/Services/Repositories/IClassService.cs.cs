using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DK.Services.Models;
using System.Linq.Expressions;

namespace DK.Services.Services
{
    public interface IClassService : Framework.Services.IBaseService<Class>
    {
        Task AddStudentAsync(int classID, int studentID);

        Task RemoveStudentAsync(int classID, int studentID);

        Task AddSubjectAsync(int classID, int subjectID);

        Task RemoveSubjectAsync(int classID, int subjectID);

        Task<List<Student>> GetListStudentsAsync(int classID);

        Task<List<Subject>> GetListSubjectsAsync(int classID);

        Task<List<StudentClass>> GetListStudentClassesAsync(int classID);

        Task<List<ClassSubject>> GetListClassSubjectsAsync(int classID);
        Task<string> GenerateMSSV();
    }
}
