using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class User
    {
        private int _id = 0;
        private string _login = string.Empty;
        private string _email = string.Empty;
        private string _role = string.Empty;
        private string _userType = string.Empty;
        private string _passwordHash = string.Empty;
        private DateTime _deletedAt = new DateTime();


        [JsonIgnore]
        public int Id { get { return _id; } set { _id = value; } }

        [Required(ErrorMessage = "Логин обязателен")]
        public string Login { get { return _login; } set { _login = value; } }

        [Required]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        public string Email { get { return _email; ; } set { _email = value; } }

        [Required]
        public string Role { get { return _role; } set { _role = value; } }

        [Required]
        public string UserType { get { return _userType; } set { _userType = value; } }


        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get { return _passwordHash; } set { _passwordHash = value; } }

        [JsonIgnore]
        public DateTime DeletedAt { get { return _deletedAt; } set { _deletedAt = value; } }
    }
}
