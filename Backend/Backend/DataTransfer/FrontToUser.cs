using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Backend;
using Backend.Entities;
using Backend.Enums;

namespace Backend.Models
{
    public class FrontToUser
    {

        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Login { get; set; } = string.Empty;
        [Required]
         [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MaxLength(255)]
        public string Password { get; set; } = string.Empty;
         [Required]
        public UserRole Role { get; set; } = UserRole.User; //user, admin
        [Required]
        public UserType Type { get; set; } = UserType.Platform; //platform, advertiser, both
        
    }

}