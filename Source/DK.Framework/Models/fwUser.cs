//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DK.Framework.Models
{
    using System;
    using System.Collections.Generic;
    [System.ComponentModel.DataAnnotations.Schema.Table("fwUser")]
    public partial class fwUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public fwUser()
        {
            this.fwGroup = new HashSet<fwGroup>();
        }

        public int ID { get; set; }
        public string AspnetUserID { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }
        public bool Locked { get; set; }
        public string Avata { get; set; }
        public Nullable<int> NotiCount { get; set; }
        public string Pass { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fwGroup> fwGroup { get; set; }
    }
}
