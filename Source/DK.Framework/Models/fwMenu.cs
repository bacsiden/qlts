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
    [System.ComponentModel.DataAnnotations.Schema.Table("fwMenu")]
    public partial class fwMenu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public fwMenu()
        {
            this.fwRole = new HashSet<fwRole>();
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public Nullable<int> Order { get; set; }
        public bool Actived { get; set; }
        public string SubAction { get; set; }
        public string Area { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fwRole> fwRole { get; set; }
    }
}
