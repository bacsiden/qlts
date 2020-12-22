using DK.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DK.Application.Models
{
    public class Type : BaseEntity
    {
        /// <summary>
        /// Trong suốt với người dùng. Có rất nhiều danh mục, dùng name để phân biệt loại danh mục
        /// Chủng loại; Danh mục; Nguồn kinh phí; Chất lượng; Tags (tag ko hiển thị lên menu)
        /// </summary>
        public string Name { get; set; }

        public string Title { get; set; }

        public bool IsSubmenu { get; set; }

        public Guid ParentId { get; set; }

        public static List<string> MenuCategories = new List<string> { "Chủng loại", "Danh mục", "Nguồn kinh phí", "Chất lượng" };
    }
}
