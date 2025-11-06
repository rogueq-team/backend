using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class AdvertiserCategoryEntity
    {
        [Required]
        public Guid AdvertiserId { get; set; }
        [Required]
        public Guid CategotyId { get; set; }
        [Required]
        public UserEntity? Advertiser { get; set; }
        [Required]
        public CategoryEntity? Category { get; set; }

    }
}
