using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreApi.Models
{
    [Table("Books")]
    [Index(nameof(Book.BookName),IsUnique = true)]
    public class Book
    {
        [Key]
        [StringLength(8)]
        public string Id { get; set; } = RandomID.RandomString(8);
        [BsonElement("Name")]
        [StringLength(255),Unicode(true)]
        public string BookName { get; set; }
        public string ImagePath { get; set; }
        public int Price { get; set; }
        [DataType(DataType.Currency)]
        public string Currency { get; set; } = "VND";
        public int Quantity { get; set; }
        public int Sold { get; set; }
        [StringLength(255),Unicode(true)] 
        public string Author { get; set; }
        private DateTime timeCreate = DateTime.UtcNow;
        [Column("Create_at")]
        public DateTime TimeCreate
        {
            get
            {
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                return TimeZoneInfo.ConvertTimeFromUtc(timeCreate, cstZone);
            }
            set { this.timeCreate = DateTime.UtcNow; }
        }
        public string CategoryId { get; set; }
        IList<BillDetail>? BillDetails { get; set; }
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
    public class ChunkFile
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string FileName { get; set; }
    }
}