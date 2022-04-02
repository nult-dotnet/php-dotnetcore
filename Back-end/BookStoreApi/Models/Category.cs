using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Models
{
    [Table("Categories")]
    [Index(nameof(Category.Name), IsUnique = true)]
    public class Category
    {
        [Key]
        [StringLength(5)]
        public string? Id { get; set; } = RandomID.RandomString(5);
        [MaxLength(255),Unicode(true)]
        public string Name { get; set; }
        public int Quantity { get; set; }= 0;
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
        public List<Book>? Book { get; set; }
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