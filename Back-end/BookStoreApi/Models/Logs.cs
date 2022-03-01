using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.Models
{
    public class Logs
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public int logLevel { get; set; }
        public string Method { get; set; }
        public string URL { get; set; }
        public string? Input { get; set; }
        public string? Output { get; set; }
        public string? Message { get; set; }
        public DateTime Time { get; set; } = DateTime.UtcNow;
    }
    public static class Method
    {
        public const string GET = "GET";
        public const string POST = "POST";
        public const string PUT = "PUT";
        public const string DELETE = "DELETE";
        public const string PATCH = "PATCH";
    }
}