using DK.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DK.Web.Models
{
    public class PointListModel
    {
        public PointListModel()
        {
            Subjects = new List<Subject>();
            Students = new List<Student>();
            StudentClasses = new List<StudentClass>();
            Points = new List<StudentClassSubject>();
        }

        public bool AddPermission { get; set; }
        public int ClassID { get; set; }
        public Class Class { get; set; }
        public List<Class> Classes { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<Student> Students { get; set; }
        public List<StudentClass> StudentClasses { get; set; }
        public List<StudentClassSubject> Points { get; set; }
    }
}