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
    [System.ComponentModel.DataAnnotations.Schema.Table("fwPage")]
    public partial class fwPage
    {
        public int ID { get; set; }
        public string KeyUrl { get; set; }
        public string Content { get; set; }
    }
}
