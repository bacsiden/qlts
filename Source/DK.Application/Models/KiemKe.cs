﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace DK.Application.Models
{
    public class KiemKe : BaseEntity
    {
        public static int? GetChenhLech(int? value1, int? value2)
        {
            if (value1.HasValue && value1.HasValue) return Math.Abs(value1.Value - value2.Value);
            if (value1.HasValue) return value1.Value;
            if (value2.HasValue) return value2.Value;
            return null;
        }
        public static decimal? GetChenhLech(decimal? value1, decimal? value2)
        {
            if (value1.HasValue && value1.HasValue) return Math.Abs(value1.Value - value2.Value);
            if (value1.HasValue) return value1.Value;
            if (value2.HasValue) return value2.Value;
            return null;
        }

        public Guid KiemKeId { get; set; }

        [BsonIgnore]
        public int No { get; set; }

        // search chính xác
        [ColumnIndex(2)]
        [Display(Name = "Mã tài sản")]
        public string Code { get; set; }

        // search contains
        [ColumnIndex(3)]
        [Display(Name = "Tên tài sản")]
        public string Name { get; set; }

        [ColumnIndex(4)]
        [Display(Name = "Năm sử dụng")]
        public int? NamSuDung { get; set; }

        [ColumnIndex(5)]
        [Display(Name = "Số lượng")]
        public int? SoLuongKeToan { get; set; }

        [ColumnIndex(6)]
        [Display(Name = "Nguyên giá")]
        public decimal? NguyenGiaKeToan { get; set; }

        [ColumnIndex(7)]
        [Display(Name = "Giá trị còn lại")]
        public decimal? GiaTriConLaiKeToan { get; set; }

        [ColumnIndex(8)]
        [Display(Name = "Số lượng")]
        public int? SoLuongKiemKe { get; set; }

        [ColumnIndex(9)]
        [Display(Name = "Nguyên giá")]
        public decimal? NguyenGiaKiemKe { get; set; }

        [ColumnIndex(10)]
        [Display(Name = "Giá trị")]
        public decimal? GiaTriConLaiKiemKe { get; set; }

        [ColumnIndex(11)]
        [Display(Name = "Số lượng")]
        public int? SoLuongChenhLech { get; set; }

        [ColumnIndex(12)]
        [Display(Name = "Nguyên giá")]
        public decimal? NguyenGieChenhLech { get; set; }

        [ColumnIndex(13)]
        [Display(Name = "Giá trị còn lại")]
        public decimal? GiaTriConLaiChenhLech { get; set; }

        [ColumnIndex(14)]
        [Display(Name = "Ghi chú")]
        public string GhiChu { get; set; }
    }
}