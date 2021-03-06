﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DK.Application.Models
{
    public class TaiSanSearchModel : TaiSan, ISearchModel
    {
        [Display(Name = "Ngân sách năm")]
        public int? NganSachNamSearch { get; set; }

        [Display(Name = "Năm sản xuất")]
        public int? NamSanXuatSearch { get; set; }

        [Display(Name = "Năm sử dụng")]
        public int? NamSuDungSearch { get; set; }

        [Display(Name = "Số lượng")]
        public int? SoLuongSearch { get; set; }

        [Display(Name = "Giá kế toán từ")]
        public decimal? GiaKeToanTu { get; set; }

        [Display(Name = "Giá kế toán đến")]
        public decimal? GiaKeToanDen { get; set; }

        [Display(Name = "Giá kiểm kê từ")]
        public decimal? GiaKiemKeTu { get; set; }

        [Display(Name = "Giá kiểm kê đến")]
        public decimal? GiaKiemKeDen { get; set; }

        [Display(Name = "Chủng loại")]
        public List<string> ChungLoais { get; set; } = new List<string>();

        public string SearchText { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IncludeSub { get; set; }
        public bool Preview { get; set; }
        public object ToPagingModel()
        {
            return new
            {
                Code,
                Name,
                GroupName,
                ChungLoai,
                ChungLoais = string.Join($"&{nameof(ChungLoais)}=", ChungLoais),
                NhanHieu,
                Serial,
                XuatXu,
                ThuocHopDong,
                ThuocGoiThau,
                NguonKinhPhi,
                NganSachNamSearch,
                NamSanXuatSearch,
                NamSuDungSearch,
                ChatLuong,
                NguoiSuDung,
                NguoiQuanLy,
                PhongQuanLy,
                LoaiXe,
                PageSize,
                Tags = string.Join($"&{nameof(Tags)}=", Tags)
            };
        }

        public string pattern { get; set; }
    }
}