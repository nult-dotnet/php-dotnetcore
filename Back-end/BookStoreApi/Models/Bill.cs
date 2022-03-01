using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.Models
{
    public class Bill
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int Value { get; set; }
        public DateTime TimeCreate { get; set; } = DateTime.UtcNow;
        public List<BookInBill> Books { get; set; }
        public string Currency { get; set; } = "VND";
    }
    public class BillDTO
    {
        [Required]
        public List<string> BookId { get; set; }
        [Required]
        public List<int> Quantity { get; set; }
    }
}