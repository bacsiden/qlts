using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DK.Services.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DK.Framework;
using FlexCel.Report;
using FlexCel.XlsAdapter;
using System.IO;

namespace DK.Services.Services
{
    public class TimeLineService : BaseRepository<TimeLine>, ITimeLineService
    {
        public TimeLineService(DefaultConnection db) : base(db)
        {
        }

        public List<TimeLine> GetListToAdd(int classID, DateTime fromDate, DateTime toDate)
        {
            var lst = new List<TimeLine>();

            while (fromDate <= toDate)
            {
                var model = db.TimeLine.FirstOrDefault(m => m.ClassID == classID && m.Date == fromDate);
                if (model == null)
                {
                    model = new TimeLine()
                    {
                        Date = fromDate,
                        ClassID = classID
                    };
                }

                lst.Add(model);
                fromDate = fromDate.AddDays(1);
            }

            return lst;
        }

        public async Task UpdateTimeLinesAsync(List<TimeLine> listTimeLine)
        {
            foreach (var item in listTimeLine)
            {
                await this.AddOrUpdateAsync(item);
            }
        }

        public async Task ExportTimeLineAsync(List<TimeLine> lst, DateTime? fromDate, DateTime? toDate, int? classID)
        {
            var classTitle = "";
            var dateTitle = "";
            if (classID.HasValue)
            {
                var _class = await db.Class.FindAsync(classID.Value);
                classTitle = $"Lớp {_class.Title} khóa {_class.Khoa}" + (_class.StartDate.HasValue ? $" năm {_class.StartDate.Value.Year}" : null);
            }
            if (fromDate.HasValue && toDate.HasValue)
            {
                dateTitle = $"Từ ngày {fromDate.ToStringVN()} đến ngày {toDate.ToStringVN()}";
            }
            var temp = new
            {
                Date = "",
                NoiDung = "",
                ThoiGian = "",
                NguoiPhuTrach = "",
                GiaoVien = "",
                DiaDiem = "",
                VatChat = ""
            };
            using (FlexCelReport fr = new FlexCelReport(true))
            {
                var timelines = CreateEmptyGenericList(temp);
                foreach (var item in lst)
                {
                    var timeline = new
                    {
                        Date = item.Date.ToStringVN() + Environment.NewLine + item.Date.DayOfWeekVN(),
                        NoiDung = item.NoiDung,
                        ThoiGian = item.ThoiGian,
                        NguoiPhuTrach = item.NguoiPhuTrach,
                        GiaoVien = item.GiaoVien,
                        DiaDiem = item.DiaDiem,
                        VatChat = item.VatChat
                    };
                    timelines.Add(timeline);
                }

                fr.AddTable("row", timelines);

                fr.SetValue("ClassTitle", classTitle);
                fr.SetValue("DateTitle", dateTitle);

                var xlsx = new XlsFile(true);
                xlsx.Open(TemplateFolder + "TimeLine.xlsx");
                fr.Run(xlsx);
                using (MemoryStream XlsStream = new MemoryStream())
                {
                    xlsx.Save(XlsStream);
                    await SendToBrowser(XlsStream, "application/excel", $"Tien-trinh-bieu-{DK.Framework.StringHelper.CreateURLParam(classTitle)}.xlsx");
                }
                //using (var pdf = new FlexCelPdfExport(xlsx, true))
                //{
                //    using (MemoryStream stream = new MemoryStream())
                //    {
                //        pdf.Export(stream);
                //        SendToBrowser(stream, "application/pdf", "Bao-cao.GoiThau.HopDong.pdf");
                //    }
                //}
            }
        }
    }
}
