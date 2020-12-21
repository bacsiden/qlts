using DK.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DK.Web.Models
{
    public class AddPointModel
    {
        public Class Class { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<Student> Students { get; set; }
        public List<StudentClassSubject> Points { get; set; }
    }
}