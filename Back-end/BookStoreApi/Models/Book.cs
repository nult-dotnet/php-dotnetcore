using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreApi.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        [BsonElement("Name")]
        public string BookName { get; set; }
        public string ImagePath { get; set; }
        public int Price { get; set; }
        public string Currency { get; set; } = "VND";
        public int Quantity { get; set; }
        public int Sold { get; set; }
        [ForeignKey("CategoryFK")]
        public string CategoryId { get; set; }
        public CategoryShow Category { get; set; }
        public string Author { get; set; }
        public DateTime TimeCreate { get; set; } = DateTime.UtcNow;
    }
    public class BookInBill
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
    }
    public class BookDTO
    {
        private int quantity;
        [Required]
        public string Name { get; set; }
        [Range(1, 10000000000000, ErrorMessage = "The Price field is required.")]
        public int Price { get; set; }
        [Range(1,1000000,ErrorMessage = "The Quantity field is required.")]
        public int Quantity
        {
            get { return quantity; }
            set { if (value > 0) quantity = value; }
        }
        [Required]
        public string CategoryId { get; set; }
        [Required]
        public string Author { get; set; }
    }
}