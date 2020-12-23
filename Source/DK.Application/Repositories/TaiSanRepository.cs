using MongoDB.Driver;
using System.Threading.Tasks;
using System;
using DK.Application.Models;

namespace DK.Application.Repositories
{
    public class TaiSanRepository : BaseRepository<TaiSan>, ITaiSanRepository
    {
        public TaiSanRepository(IMongoDatabase db) : base(db)
        {
        }
    }
}
