using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class RegUser
    {
        private int id = 0;
        private string login=string.Empty;
        private string email=string.Empty;
        private string role = string.Empty;
        private string userType = string.Empty;
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

        [Required]
        public string UserType { get { return userType; } set { userType = value; } }

        public RegUser(string login, string email, string role, string userType)
        {
            Login = login;
            Email = email;
            Role = role;
            UserType = userType;
        }
        public RegUser(User user)
        {
            Login = user.Login;
            Email = user.Email;
            Role = user.Email;
            UserType = user.UserType;
        }

    }

}