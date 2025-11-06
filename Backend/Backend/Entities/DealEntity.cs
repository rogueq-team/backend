using Backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class DealEntity
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid ApplicationId { get; set; }

        [Required]
        public Guid AdvertizerId { get; set; }

        [Required]
        public Guid PlatformId { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public DealEntityStatus Status { get; set; } = DealEntityStatus.Wait;

        // Связи _________________________
        
        // _________________________

    }
}
