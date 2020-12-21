using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DK.Services.Models
{
    public class DefaultConnection : DbContext
    {
        public DefaultConnection() : base("name=DefaultConnection")
        {
            Database.SetInitializer<DefaultConnection>(null);
            
            //Configuration.AutoDetectChangesEnabled = false;
            //Configuration.ValidateOnSaveEnabled = false;
        }

        public virtual DbSet<Class> Class { get; set; }
        public virtual DbSet<ClassSubject> ClassSubject { get; set; }
        public virtual DbSet<DanToc> DanToc { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<StudentClass> StudentClass { get; set; }
        public virtual DbSet<StudentClassSubject> StudentClassSubject { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<TimeLine> TimeLine { get; set; }
        public virtual DbSet<Certificate> Certificate { get; set; }
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<BookBorrow> BookBorrow { get; set; }
    }
}
