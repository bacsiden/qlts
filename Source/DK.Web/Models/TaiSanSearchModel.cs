using DK.Application.Models;
using System;

namespace DK.Web.Models
{
    public class TaiSanSearchModel : TaiSan
    {
        public int Page { get; set; } = 1;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}