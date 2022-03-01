using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }= 0;
        public DateTime TimeCreate { get; set; } = DateTime.UtcNow;
        public List<String>? Book { get; set; }
    }
    public class CategoryShow
    {
        public string Id { get; set; }
        public string CategoryName { get; set; }
    }
    public class CategoryDTO
    {
        [Required]
        public string Name { get; set; }
    }
}