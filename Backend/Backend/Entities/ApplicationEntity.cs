using Backend.Abstraction;
using Backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    public class ApplicationEntity : BaseEntity
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }

        [Required]
        public ApplicationEntityStatus Status { get; set; } = ApplicationEntityStatus.Wait;

        // Связи _________________________
        public List<DealEntity>? Deals { get; set; }

        public List<ApplicationCategoryEntity>? ApplicationCategories { get; set; }
        public List<CategoryEntity>? Categories { get; set; }
        // _________________________
    }
}
