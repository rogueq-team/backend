using Backend.Abstraction;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class CategoryEntity : BaseEntity
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Slug { get; set; }

        public Guid? ParentCategoryId { get; set; } = null;
        // Связи _______________________
        public List<ApplicationEntity>? ApplicationEntities { get; set; }
        public List<ApplicationCategoryEntity>? ApplicationCategories { get; set; }
        public List<UserEntity>? UserEntities { get; set; }
        public List<PlatformCategoryEntity>? PlatformsCategories { get; set; }
        public List<AdvertiserCategoryEntity>? AdvertiserCategories { get; set; }
        //  _______________________

    }
}
