//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DK.Services.Models
{
    using System;
    using System.Collections.Generic;
    [System.ComponentModel.DataAnnotations.Schema.Table("BookBorrow")]
    public partial class BookBorrow
    {
        public int ID { get; set; }
        public int BookID { get; set; }
        public System.DateTime NgayMuon { get; set; }
        public string NguoiMuon { get; set; }
        public string DonVi { get; set; }
        public string SDT { get; set; }
        public Nullable<System.DateTime> NgayHenTra { get; set; }
        public Nullable<System.DateTime> NgayTra { get; set; }
        public bool DaTra { get; set; }
        public string Note { get; set; }

        public virtual Book Book { get; set; }
    }
}
