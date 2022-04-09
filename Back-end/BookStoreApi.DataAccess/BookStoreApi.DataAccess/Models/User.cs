using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreApi.Models
{
    [Table("Users")]
    [Index(nameof(User.Email),IsUnique = true)]
    [Index(nameof(User.Phone),IsUnique = true)]
    public class User
    {
        [Key]
        [StringLength(8)]
        public string Id { get; set; } = RandomID.RandomString(8);
        [StringLength(255),Unicode(true)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        [StringLength(10)]
        public string Phone { get; set; }
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
        [Unicode(true)]
        public string Address { get; set; }
        public string RoleId { get; set; }
    }
    public class UpdateUser
    {
        [Required]
        public string Name { get; set; }
        [Required,Phone,StringLength(10,ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }
        [Required]
        public string RoleId { get; set; }
        [Required]
        public string Address { get; set; }
    }
    public class CreateUser
    {
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required,Phone,StringLength(10,ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }
        [Required]
        public string RoleId { get; set; }
        [Required]
        public string Address { get; set; }
    }
}