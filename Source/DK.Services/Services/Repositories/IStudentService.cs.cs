using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DK.Services.Models;
using System.Linq.Expressions;

namespace DK.Services.Services
{
    public interface IStudentService : Framework.Services.IBaseService<Student>
    {
        IQueryable<Student> GetList(Expression<Func<StudentClass, bool>> predicate);

        Task ExportStudentsAsync(List<Student> students, int classID);
    }
}
