using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DK.Web
{
    public class Constant
    {
        public const int PAGE_SIZE = 30;

        public const string SESSION_MessageSuccess = "SESSION_MessageSuccess";
        public const string SESSION_MessageError = "SESSION_MessageError";

        public static List<Tuple<int, string>> TinhTrangMuon = new List<Tuple<int, string>>()
        {
            new Tuple<int, string> (0,"Quá hạn"),
            new Tuple<int, string> (1,"Chưa trả"),
            new Tuple<int, string> (2,"Đã trả")
        };
    }
    public static class Roles
    {
        public const string SystemManager = "SystemManager";
        public class Subject
        {
            public const string View = "ViewSubject";
            public const string Add = "AddNewSubject";
            public const string Edit = "EditSubject";
            public const string Delete = "DeleteSubject";
        }
        public class Student
        {
            public const string View = "ViewStudent";
            public const string Add = "AddNewStudent";
            public const string Edit = "EditStudent";
            public const string Delete = "DeleteStudent";
        }
        public class Class
        {
            public const string View = "ViewClass";
            public const string Add = "AddNewClass";
            public const string Edit = "EditClass";
            public const string Delete = "DeleteClass";

            public const string AddStudent = "AddStudent";
            public const string RemoveStudent = "RemoveStudent";
            public const string AddSubject = "AddSubject";
            public const string RemoveSubject = "RemoveSubject";
        }

        public class Point
        {
            public const string View = "ViewPoint";
            public const string Add = "AddPoint";
            public const string Edit = "EditPoint";
            public const string Delete = "DeletePoint";
        }

        public class TimeLine
        {
            public const string View = "ViewTimeLine";
            public const string Add = "AddNewTimeLine";
            public const string Edit = "EditTimeLine";
            public const string Delete = "DeleteTimeLine";
        }
        public class Certificate
        {
            public const string View = "ViewCertificate";
            public const string Add = "AddNewCertificate";
            public const string Edit = "EditCertificate";
            public const string Delete = "DeleteCertificate";
        }
        public class Library
        {
            public const string BookManager = "BookManager";
            public const string BookBorrowManager = "BookBorrowManager";
        }
    }
}