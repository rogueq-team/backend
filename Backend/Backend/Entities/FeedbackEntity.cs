using Backend.Abstraction;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class FeedbackEntity : BaseEntity
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid SenderId { get; set; }
        [Required]
        public Guid RecipientId { get; set; }
        public UserEntity? Sender { get; set; }
        public UserEntity? Recipient { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Text { get; set; }
        [Required]
        [Range(0, 5, ErrorMessage = "Количество звёзд должно быть от 1 до 5")]
        public int? Stars { get; set; }
    }
}
