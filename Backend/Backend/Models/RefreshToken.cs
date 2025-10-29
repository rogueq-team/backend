using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class RefreshToken
    {
        public int Id { get; set; } = 0;
        public Guid UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsActive => !IsExpired;
        
        
    }
}
