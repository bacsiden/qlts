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
    [System.ComponentModel.DataAnnotations.Schema.Table("fwGroup")]
    public partial class fwGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public fwGroup()
        {
            this.fwRole = new HashSet<fwRole>();
            this.fwUser = new HashSet<fwUser>();
        }

        public int ID { get; set; }
        public string Title { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fwRole> fwRole { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fwUser> fwUser { get; set; }
    }
}
