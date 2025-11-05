using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Backend;
using Backend.Entities;
using Backend.Enums;

namespace Backend.Models
{
    public class UserToFront
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole? Role { get; set; } = UserRole.User; //user, admin
        
        public UserType? Type { get; set; } = UserType.Platform; //platform, advertiser, both
        public decimal Balance { get; set; }

        public string? AvatarPath { get; set; }

        public string? Bio { get; set; }

        public List<string>? SocialLinks { get; set; }
  

         public bool IsVerified { get; set; } = false;

        public DateTime? CreatedAt;

        public DateTime? UpdatedAt;

        public DateTime? DeletedAt; 


        public UserToFront(string name, string login, string email, string role, string userType, bool isVerified, decimal balance=0,string?  avatarPath=null, string? bio=null,List<string>? socialLinks=null, DateTime? createdAt=null, DateTime? updatedAt=null, DateTime? deletedAt=null)
        {
            Name = name;
            Login = login;
            Email = email;
            Role = (UserRole)(role=="Admin"?1:0);
            Type = (UserType)(userType == "Platform" ? 0 : (userType == "Advertiser") ? 1 : 2);
            Balance = balance;
            AvatarPath = avatarPath;
            Bio = bio;
            SocialLinks = socialLinks;
            IsVerified = isVerified;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            DeletedAt = deletedAt;
        }
        public UserToFront(UserEntity user)
        {
            Name = user.Name;
            Login = user.Login;
            Email = user.Email;
            Role = user.Role;
            Type = user.Type;
            Balance = user.Balance;
            AvatarPath = user.AvatarPath;
            Bio = user.Bio;
            SocialLinks = user.SocialLinks;
            IsVerified = user.IsVerified;
            CreatedAt = user.CreatedAt;
            UpdatedAt = user.UpdatedAt;
            DeletedAt = user.DeletedAt;

        }

    }

}