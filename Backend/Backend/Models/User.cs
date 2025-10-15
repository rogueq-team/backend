using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class User
    {
        private int id=0;
        private string login=string.Empty;
        private string email=string.Empty;
        private string role = "platform";
        private string password=string.Empty;


        [JsonIgnore]
        public int Id { get { return id; } set { id=value; } }

        [Required(ErrorMessage = "Логин обязателен")]
        public string Login { get { return login; } set { login = value; } }

        [Required]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        public string Email { get { return email; ; } set { email = value; } }
        
        [Required]
        public string Role { get { return role; } set { role = value; } }
        [Required(ErrorMessage ="Пароль обязателен")]
        public string Password { get{ return password; } set { password = value; } }
    }
}
