using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class ApplicationCategoryEntity
    {
        [Required]
        public Guid ApplicationId { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public ApplicationEntity Application { get; set; } = null!;
        [Required]
        public CategoryEntity Category { get; set; } = null!;
    }
}
