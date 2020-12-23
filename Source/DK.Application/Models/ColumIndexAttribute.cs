using System;

namespace DK.Application.Models
{
    public class ColumnIndexAttribute : Attribute
    {
        public ColumnIndexAttribute(int index)
        {
            Index = index;
        }
        public int Index { get; set; }
    }
}