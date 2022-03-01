using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.Models
{
    public class Role
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Role")]
        public string Name { get; set; }
        public DateTime TimeCreate { get; set; } = DateTime.UtcNow;
        public int Quantity { get; set; }
        public List<string> User { get; set; }
    }
    public class RoleShow
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class RoleDTO
    {
        [Required]
        public string Name { get; set; }
    }
}