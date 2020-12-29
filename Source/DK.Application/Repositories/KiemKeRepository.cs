using MongoDB.Driver;
using System.Threading.Tasks;
using System;
using DK.Application.Models;
using System.Collections.Generic;
using MongoDB.Bson;
using System.Text.RegularExpressions;

namespace DK.Application.Repositories
{
    public class KiemKeRepository : BaseRepository<KiemKe>, IKiemKeRepository
    {
        public KiemKeRepository(IMongoDatabase db) : base(db)
        {
        }
    }
}
