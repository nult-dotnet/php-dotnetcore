using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Models
{
    [Table("Roles")]
    [Index(nameof(Role.Name),IsUnique = true)]
    public class Role
    {
        [Key]
        [StringLength(5)]
        public string Id { get; set; } = RandomID.RandomString(5);
        [StringLength(255), Unicode(true)]
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
        public int Quantity { get; set; }
        public List<User>? User { get; set; }
    }
    public class RoleShow
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
    public class RoleDTO
    {
        [Required]
        public string Name { get; set; }
    }
}