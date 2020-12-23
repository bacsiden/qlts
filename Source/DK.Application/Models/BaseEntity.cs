using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;

namespace DK.Application.Models
{
    public class BaseEntity
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id { get; set; }

        public DateTime Created { get; protected set; } = DateTime.UtcNow.AddHours(7);

        public DateTime Modified { get; protected set; } = DateTime.UtcNow.AddHours(7);

        public string CreatedBy { get; set; }
    }
}
