using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreApi.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime TimeCreate { get; set; } = DateTime.Now;
        [ForeignKey("RoleFK")]
        public string RoleId { get; set; }
        public RoleShow Role { get; set; }
        public string Address { get; set; }
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