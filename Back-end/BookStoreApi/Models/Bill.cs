using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using LibraryAbstractDBProvider;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreApi.Models
{
    [Table("Bills")]
    public class Bill
    {
        [Key]
        [StringLength(8)]
        public string Id { get; set; } = RandomID.RandomString(8);
        public int Value { get; set; }
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
        [StringLength(3)]
        public string Currency { get; set; } = "VND";
        IList<BillDetail>? BillDetails { get; set; }
    }
    [Table("BillDetail")]
    public class BillDetail
    {
        [Key]
        public string BookId { get; set; }
        public Book? Book { get; set; }
        [Key]
        public string BillId { get; set; }
        public Bill? Bill { get; set; }
        public int Quantity { get; set; }
    }
    public class BillDTO
    {
        [Required]
        public List<string> BookId { get; set; }
        [Required]
        public List<int> Quantity { get; set; }
    }
}