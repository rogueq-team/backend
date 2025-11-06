using Backend.Abstraction;
using Backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{

    public class UserEntity : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //само генерируется
        public Guid UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [EmailAddress] //валидация
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Password { get; set; } = string.Empty;
        [Required]
        public UserRole Role { get; set; } = UserRole.User; //user, admin

        [Required]
        public UserType Type { get; set; } = UserType.Platform; //platform, advertiser, both
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        // Связи _________________________
        public List<ApplicationEntity>? Applications { get; set; }

        //public List<DealEntity>? Deals { get; set; }

        //public List<AdvertiserCategoryEntity>? AdvertiserCategories { get; set; }

        //public List<PlatformCategoryEntity>? PlatformCategories { get; set; }

        //public List<MessageEntity>? Messages { get; set; }

        //public List<FeedbackEntity>? Feedbacks { get; set; }
        // _________________________

        public string? AvatarPath { get; set; }

        [MaxLength(600)]
        public string? Bio { get; set; }

        public List<string>? SocialLinks { get; set; }
        //позже можно добавить отдельный класс
        //для классификации по названию платформы

        [Required]
        public bool IsVerified { get; set; } = false;

    }
}
