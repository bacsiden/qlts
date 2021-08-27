using MongoDB.Driver;
using System.Threading.Tasks;
using System;
using DK.Application.Models;
using System.Collections.Generic;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using System.Linq;

namespace DK.Application.Repositories
{
    public class TaiSanRepository : BaseRepository<TaiSan>, ITaiSanRepository
    {
        public TaiSanRepository(IMongoDatabase db) : base(db)
        {
        }

        public IEnumerable<TaiSan> Find(TaiSanSearchModel model)
        {
            var filter = Builders<TaiSan>.Filter;
            var query = filter.Eq(m => m.IsApproved, model.IsApproved);

            #region Search equals
            if (!string.IsNullOrEmpty(model.CreatedBy))
            {
                query &= filter.Eq(m => m.CreatedBy, model.CreatedBy);
            }
            if (!string.IsNullOrWhiteSpace(model.Code))
            {
                query &= filter.Eq(m => m.Code, model.Code.Trim());
            }
            if (model.Number.HasValue)
            {
                query &= filter.Eq(m => m.Number, model.Number.Value);
            }
            if (!string.IsNullOrWhiteSpace(model.ChungLoai))
            {
                query &= filter.Eq(m => m.ChungLoai, model.ChungLoai.Trim());
            }
            if (!string.IsNullOrWhiteSpace(model.Serial))
            {
                query &= filter.Eq(m => m.Serial, model.Serial.Trim());
            }
            if (!string.IsNullOrWhiteSpace(model.ThuocHopDong))
            {
                query &= filter.Eq(m => m.ThuocHopDong, model.ThuocHopDong.Trim());
            }
            if (!string.IsNullOrWhiteSpace(model.NguonKinhPhi))
            {
                query &= filter.Eq(m => m.NguonKinhPhi, model.NguonKinhPhi.Trim());
            }
            if (!string.IsNullOrWhiteSpace(model.NganSachKhac))
            {
                query &= filter.Eq(m => m.NganSachKhac, model.NganSachKhac.Trim());
            }
            if (model.NganSachNamSearch.HasValue)
            {
                query &= filter.Eq(m => m.NganSachNam, model.NganSachNamSearch);
            }
            if (model.SoLuong.HasValue)
            {
                var soLuongFilter = filter.Eq(m => m.SoLuong, model.SoLuong);
                if (model.SoLuong == 0)
                {
                    soLuongFilter |= filter.Eq(m => m.SoLuong, null);
                }
                query &= soLuongFilter;
            }
            if (model.NamSanXuat.HasValue)
            {
                query &= filter.Eq(m => m.NamSanXuat, model.NamSanXuat);
            }
            if (model.NamSuDung.HasValue)
            {
                query &= filter.Eq(m => m.NamSuDung, model.NamSuDung);
            }
            if (!string.IsNullOrWhiteSpace(model.ChatLuong))
            {
                query &= filter.Eq(m => m.ChatLuong, model.ChatLuong.Trim());
            }
            if (!string.IsNullOrWhiteSpace(model.PhongQuanLy))
            {
                query &= filter.Eq(m => m.PhongQuanLy, model.PhongQuanLy.Trim());
            }
            if (!string.IsNullOrWhiteSpace(model.LoaiXe))
            {
                query &= filter.Eq(m => m.LoaiXe, model.LoaiXe.Trim());
            }
            if (!string.IsNullOrWhiteSpace(model.GroupName))
            {
                query &= filter.Eq(m => m.GroupName, model.GroupName.Trim());
            }
            #endregion

            #region Search Contains
            if (!string.IsNullOrWhiteSpace(model.BienSo))
            {
                query &= filter.Regex(m => m.BienSo, new BsonRegularExpression(model.BienSo, "i"));
            }
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                query &= filter.Regex(m => m.Name, BsonRegularExpression.Create(new Regex(model.Name, RegexOptions.IgnoreCase)));
            }
            if (!string.IsNullOrWhiteSpace(model.NhanHieu))
            {
                query &= filter.Regex(m => m.NhanHieu, BsonRegularExpression.Create(new Regex(model.NhanHieu, RegexOptions.IgnoreCase)));
            }
            if (!string.IsNullOrWhiteSpace(model.XuatXu))
            {
                query &= filter.Regex(m => m.XuatXu, BsonRegularExpression.Create(new Regex(model.XuatXu, RegexOptions.IgnoreCase)));
            }
            if (!string.IsNullOrWhiteSpace(model.ThuocGoiThau))
            {
                query &= filter.Regex(m => m.ThuocGoiThau, BsonRegularExpression.Create(new Regex(model.ThuocGoiThau, RegexOptions.IgnoreCase)));
            }
            if (!string.IsNullOrWhiteSpace(model.NguoiSuDung))
            {
                query &= filter.Regex(m => m.NguoiSuDung, BsonRegularExpression.Create(new Regex(model.NguoiSuDung, RegexOptions.IgnoreCase)));
            }
            if (!string.IsNullOrWhiteSpace(model.NguoiQuanLy))
            {
                query &= filter.Regex(m => m.NguoiQuanLy, BsonRegularExpression.Create(new Regex(model.NguoiQuanLy, RegexOptions.IgnoreCase)));
            }
            if (!string.IsNullOrWhiteSpace(model.HeThongTS))
            {
                query &= filter.Regex(m => m.HeThongTS, BsonRegularExpression.Create(new Regex(model.HeThongTS, RegexOptions.IgnoreCase)));
            }
            #endregion

            if (model.Tags.Count > 0)
            {
                query &= filter.All(m => m.Tags, model.Tags);
            }

            if (model.ChungLoais?.Any() == true)
            {
                query &= filter.In(m => m.ChungLoai, model.ChungLoais);
            }

            return _collection.Find(query).SortBy(m => m.Number).ToEnumerable();
        }
    }
}
