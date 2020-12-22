using MongoDB.Driver;
using DK.Application.Models;

namespace DK.Application.Repositories
{
    public class TypeRepository : BaseRepository<Type>, ITypeRepository
    {
        public TypeRepository(IMongoDatabase db) : base(db)
        {
        }
    }
}
