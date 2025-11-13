using Backend.Abstraction;
using Backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    public class ApplicationEntity : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ApplicationId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Сумма должна быть положительной")]
        public decimal Cost { get; set; }

        [Required]
        public ApplicationStatus Status { get; set; } = ApplicationStatus.InProgress;

        // Связи _________________________
        public List<DealEntity>? Deals { get; set; }
        public List<ApplicationCategoryEntity>? ApplicationCategories { get; set; }
        [NotMapped]
        public List<CategoryEntity>? Categories { get; set; }
        // _________________________
    }
}
