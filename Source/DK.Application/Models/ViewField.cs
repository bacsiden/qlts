using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DK.Application.Models
{
    public class ViewField : BaseEntity
    {
        public string FieldName { get; set; }
        public string ObjectName { get; set; }
        public int Order { get; set; }
        public bool Display { get; set; }
        public string DisplayText { get; set; }
    }
}
