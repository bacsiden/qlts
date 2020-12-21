using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DK.Services.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DK.Framework;
using System.IO;

namespace DK.Services.Services
{
    public class BookBorrowService : BaseRepository<BookBorrow>, IBookBorrowService
    {
        public BookBorrowService(DefaultConnection db) : base(db)
        {
        }

        public async Task ExportPhieuMuonAsync(int id)
        {
            var table = new List<List<DocField>>();
            var bbr = await db.BookBorrow.FindAsync(id);
            var outputFilename = $"Phieu-muon-tra - {StringHelper.CreateURLParam(bbr.NguoiMuon+"")}.docx";
            var fullOutputFilename = TemplateFolder + outputFilename;
            var fields = new List<DocField>() {
                new DocField { Name = "FullName", Value = bbr.NguoiMuon + "" },
                new DocField { Name = "DonVi", Value = bbr.DonVi + "" },
                new DocField { Name = "SDT", Value = bbr.SDT + "" },
                new DocField { Name = "NgayMuon", Value = bbr.NgayMuon.ToStringVN() },
                new DocField { Name = "NgayHenTra", Value = bbr.NgayHenTra.ToStringVN() + "" }
            };

            var i = 0;
            //foreach (var item in students.OrderBy(m => m.LastName))
            //{
                var row = new List<DocField>();
                row.Add(new DocField { Name = "No", Value = (++i).ToString() });
                row.Add(new DocField { Name = "Name", Value = bbr.Book.Title });
                row.Add(new DocField { Name = "Author", Value = $"{bbr.Book.TacGia}" });
                row.Add(new DocField { Name = "Quantity", Value = $"{1}" });
                row.Add(new DocField { Name = "GhiChu", Value = $"{bbr.Note}" });

                table.Add(row);
            //}

            Docx.FillSingleTable(TemplateFolder + "BookBorrow.docx", fullOutputFilename, table, fields);

            using (StreamReader stream = new StreamReader(fullOutputFilename))
            {
                await SendToBrowser(stream.BaseStream, "application/vnd.ms-word", outputFilename);
            }
        }
    }
}
