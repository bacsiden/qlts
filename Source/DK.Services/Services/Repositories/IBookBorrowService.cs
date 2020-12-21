using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DK.Services.Models;

namespace DK.Services.Services
{
    public interface IBookBorrowService : Framework.Services.IBaseService<BookBorrow>
    {
        Task ExportPhieuMuonAsync(int id);
    }
}
