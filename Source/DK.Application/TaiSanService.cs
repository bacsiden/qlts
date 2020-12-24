using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DK.Application.Models;
using DK.Application.Repositories;
using FlexCel.Core;
using FlexCel.XlsAdapter;

namespace DK.Application
{
    public class TaiSanService
    {
        const int Code = 2;
        const int Name = 3;
        const int ChungLoai = 4;
        const int DanhMuc = 5;
        const int NhanHieu = 6;
        protected readonly string TemplateFolder = HttpContext.Current.Server.MapPath("~/ReportTemplates\\");
        private readonly ITaiSanRepository _taiSanRepository;
        public TaiSanService(ITaiSanRepository taiSanRepository)
        {
            _taiSanRepository = taiSanRepository;
            //xlsx.Open(TemplateFolder + "TimeLine.xlsx");
        }

        public void Import()
        {
            XlsFile xls = new XlsFile(TemplateFolder + "TimeLine.xlsx");

            xls.ActiveSheetByName = "Sheet1";  //we'll read sheet1. We could loop over the existing sheets by using xls.SheetCount and xls.ActiveSheet 
            for (int row = 1; row <= xls.RowCount; row++)
            {
                for (int colIndex = 1; colIndex <= xls.ColCountInRow(row); colIndex++) //Don't use xls.ColCount as it is slow: https://doc.tmssoftware.com/flexcel/net/guides/performance-guide.html#avoid-calling-colcount
                {
                    int XF = -1;
                    object cell = xls.GetCellValueIndexed(row, colIndex, ref XF);
                    var index = TaiSan.GetCol(nameof(TaiSan.ChungLoai));
                    TCellAddress addr = new TCellAddress(row, xls.ColFromIndex(row, colIndex));
                    Console.Write("Cell " + addr.CellRef + " ");
                    if (cell == null) Console.WriteLine("is empty.");
                    else if (cell is TRichString) Console.WriteLine("has a rich string.");
                    else if (cell is string) Console.WriteLine("has a string.");
                    else if (cell is Double) Console.WriteLine("has a number.");
                    else if (cell is bool) Console.WriteLine("has a bool.");
                    else if (cell is TFlxFormulaErrorValue) Console.WriteLine("has an error.");
                    else if (cell is TFormula) Console.WriteLine("has a formula.");
                    else Console.WriteLine("Error: Unknown cell type");
                }
            }
        }

        private string GetCellString(object cell) => cell?.ToString();
        private decimal? GetCellDecimal(object cell)
        {
            if (decimal.TryParse(cell + "", out decimal result))
                return result;
            return null;
        }
        private int? GetCellInt(object cell)
        {
            if (int.TryParse(cell + "", out int result))
                return result;
            return null;
        }
    }
}
