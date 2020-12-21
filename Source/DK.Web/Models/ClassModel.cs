using DK.Services.Models;
using System;
using System.Collections.Generic;

namespace DK.Web.Models
{
    public class ClassModel
    {
        public void ClassSubject()
        {
            StudentClasses = new List<StudentClass>();
            ClassSubjects = new List<ClassSubject>();
            Subjects = new List<Subject>();
        }
        public Class Class { get; set; }
        public List<Subject> Subjects { get; internal set; }
        public List<Student> Students { get; internal set; }
        public List<StudentClass> StudentClasses{ get; set; }
        public List<ClassSubject> ClassSubjects { get; set; }

        public bool CanAddOrEdit { get; set; }
        public bool CanAddStudent { get; set; }
        public bool CanRemoveStudent { get; set; }
        public bool CanAddSubject { get; set; }
        public bool CanRemoveSubject { get; set; }
        public bool  CanAddCertificate { get; set; }
        public bool CanDeleteCertificate { get; set; }
        public bool CanEditCertificate { get; set; }
    }
}