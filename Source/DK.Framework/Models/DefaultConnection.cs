using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DK.Framework.Models
{
    public class DefaultConnection : DbContext
    {
        public DefaultConnection() : base("name=DefaultConnection")
        {
            Database.SetInitializer<DefaultConnection>(null);
            
            //Configuration.AutoDetectChangesEnabled = false;
            //Configuration.ValidateOnSaveEnabled = false;
        }

        public virtual DbSet<fwConfig> fwConfig { get; set; }
        public virtual DbSet<fwGroup> fwGroup { get; set; }
        public virtual DbSet<fwMenu> fwMenu { get; set; }
        public virtual DbSet<fwNotification> fwNotification { get; set; }
        public virtual DbSet<fwPage> fwPage { get; set; }
        public virtual DbSet<fwRole> fwRole { get; set; }
        public virtual DbSet<fwUser> fwUser { get; set; }
    }
}
