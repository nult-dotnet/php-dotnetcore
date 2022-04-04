using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreApi.Models
{
    [Table("Permission")]
    [Index(nameof(Permission.Name),IsUnique = true)]
    public class Permission
    {
        [Key]
        [StringLength(5)]
        public string Id { get; set; } = RandomID.RandomString(5);
        [Required]
        [Unicode(true)]
        public string Name { get; set; }
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
    }
}