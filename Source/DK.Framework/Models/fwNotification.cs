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
    [System.ComponentModel.DataAnnotations.Schema.Table("fwNotification")]
    public partial class fwNotification
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public int UserID { get; set; }
        public bool Read { get; set; }
    }
}
