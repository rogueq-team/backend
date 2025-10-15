using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class AuthRequest
    {
        [Required(ErrorMessage = "Логин или Email обязателен")]
        public string LoginOrEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        public string password { get; set; } = string.Empty;
    }

}