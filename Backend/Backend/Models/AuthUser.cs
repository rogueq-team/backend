using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class AuthUser
    {
        [Required(ErrorMessage = " Email обязателен")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        public string password { get; set; } = string.Empty;
    }

}