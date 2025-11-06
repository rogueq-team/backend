using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class PlatformCategoryEntity
    {
        [Required]
        public Guid PlatformId { get; set; }
        [Required]
        public Guid CategotyId { get; set; }
        [Required]
        public UserEntity? Platform { get; set; }
        [Required]
        public CategoryEntity? Category { get; set; }
    }
}
