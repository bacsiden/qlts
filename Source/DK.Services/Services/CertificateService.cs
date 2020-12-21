using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DK.Services.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DK.Framework;
using System.IO;
using System.IO.Compression;

namespace DK.Services.Services
{
    public class CertificateService : BaseRepository<Certificate>, ICertificateService
    {
        public CertificateService(DefaultConnection db) : base(db)
        {
        }

        public override IQueryable<Certificate> GetList(Expression<Func<Certificate, bool>> predicate = null)
        {
            if (predicate == null) return db.Certificate.Where(m => !m.Deleted);

            return db.Certificate.Where(predicate).Where(m => !m.Deleted);
        }

        public override void Delete(int id)
        {
            var model = db.Certificate.Find(id);
            model.Deleted = true;
            Update(model);
        }

        public override async Task DeleteAsync(int id)
        {
            var model = await db.Certificate.FindAsync(id);
            model.Deleted = true;
            await UpdateAsync(model);
        }

        public async Task<string> Provision(int classID, List<int> studentIDs, int no)
        {
            var lstCer = new List<Certificate>();
            foreach (var studentID in studentIDs)
            {
                var studentClass = db.StudentClass.FirstOrDefault(m => m.ClassID == classID && m.StudentID == studentID);
                if (string.IsNullOrEmpty(studentClass.XepLoaiTotNghiep))
                {
                    return $"Cần tính xếp loại tốt nghiệp cho học viên {studentClass.Student.FullName} trước khi cấp chứng nhận";
                }

                var certificate = db.Certificate.FirstOrDefault(m => m.StudentID == studentID && m.ClassID == classID);
                if (certificate == null)
                {
                    certificate = new Certificate();
                }

                var _class = await db.Class.FindAsync(classID);

                certificate.Title = _class.TenChungChi;
                certificate.Type = _class.LoaiChungChi;
                certificate.StudentID = studentID;
                certificate.ClassID = classID;
                certificate.FullName = studentClass.Student.FullName;
                certificate.DOB = studentClass.Student.BirthDate;
                certificate.MSSV = studentClass.Student.MSSV;
                certificate.Truong = studentClass.Student.Truong;
                certificate.ChucVu = studentClass.Student.ChucVu;
                certificate.DonViCongTac = studentClass.Student.DonVi;
                certificate.QueQuan = studentClass.Student.NguyenQuan;
                certificate.XepLoai = studentClass.XepLoaiTotNghiep;
                certificate.Point = studentClass.DiemTrungBinh;
                certificate.SoHieu = no;
                certificate.FromDate = _class.StartDate.HasValue? _class.StartDate.Value: new DateTime();
                certificate.ToDate = _class.EndDate.HasValue ? _class.EndDate.Value : new DateTime();

                lstCer.Add(certificate);
            }
            foreach (var item in lstCer)
            {
                await AddOrUpdateAsync(item);
            }

            return null;
        }

        public async Task Download(int certificateID)
        {
            var cer = await db.Certificate.FindAsync(certificateID);
            if (cer == null) return;

            var fullname = new { Name = "Name", Value = cer.FullName };
            var dob = new { Name = "DoB", Value = cer.DOB.ToStringVN() };
            var mssv = new { Name = "MSSV", Value = cer.MSSV + ""};
            var lop = new { Name = "Class", Value = cer.Class.Title };
            var truong = new { Name = "Truong", Value = cer.Truong + "" };
            var time = new { Name = "TimeSpan", Value = $"Từ ngày {cer.FromDate.ToStringVN()} đến ngày {cer.ToDate.ToStringVN()}" };
            var point = new { Name = "Point", Value = cer.Point.HasValue ? cer.Point.Value.ToString("N1") : "" };
            var level = new { Name = "Level", Value = cer.XepLoai + "" };

            var outputFilename = StringHelper.CreateURLParam(cer.FullName) + ".docx";
            var fullOutputFilename = TemplateFolder + outputFilename;

            Docx.FillField(TemplateFolder + "ChungNhan.docx", fullOutputFilename,
                fullname,
                dob,
                mssv,
                lop,
                truong,
                time,
                point,
                level);

            using (StreamReader stream = new StreamReader(fullOutputFilename))
            {
                await SendToBrowser(stream.BaseStream, "application/vnd.ms-word", outputFilename);
            }
        }

        public async Task DownloadManyAsync(List<Certificate> certificates)
        {
            var folder = TemplateFolder + "ChungNhan\\";
            var zipfile = TemplateFolder + "ChungNhan.zip";
            if (Directory.Exists(folder)) Directory.Delete(folder, true);
            if (File.Exists(zipfile)) File.Delete(zipfile);

            Directory.CreateDirectory(folder);
            foreach (var cer in certificates)
            {
                var fullname = new { Name = "Name", Value = cer.FullName };
                var dob = new { Name = "DoB", Value = cer.DOB.ToStringVN() };
                var mssv = new { Name = "MSSV", Value = cer.MSSV + "" };
                var lop = new { Name = "Class", Value = cer.Class.Title };
                var truong = new { Name = "Truong", Value = cer.Truong + "" };
                var time = new { Name = "TimeSpan", Value = $"Từ ngày {cer.FromDate.ToStringVN()} đến ngày {cer.ToDate.ToStringVN()}" };
                var point = new { Name = "Point", Value = cer.Point.HasValue ? cer.Point.Value.ToString("N1") : "" };
                var level = new { Name = "Level", Value = cer.XepLoai + "" };

                var outputFilename = StringHelper.CreateURLParam(cer.FullName) + ".docx";
                if (File.Exists(outputFilename)) outputFilename = Guid.NewGuid().ToString() + outputFilename;
                var fullOutputFilename = folder + outputFilename;

                Docx.FillField(TemplateFolder + "ChungNhan.docx", fullOutputFilename,
                    fullname,
                    dob,
                    mssv,
                    lop,
                    truong,
                    time,
                    point,
                    level);
            }
            ZipFile.CreateFromDirectory(folder, zipfile);
            using (StreamReader stream = new StreamReader(zipfile))
            {
                await SendToBrowser(stream.BaseStream, "application/zip", "ChungNhan.zip");
            }
        }
    }
}
