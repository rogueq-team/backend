using Backend.Abstraction;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class MessageEntity : BaseEntity
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid DealId { get; set; }

        [Required]
        public virtual DealEntity? Deal { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public virtual UserEntity? User { get; set; }

        [Required]
        [MaxLength(600)]
        public string? Text { get; set; } = String.Empty;
        public MessageEntity(Guid dealId, DealEntity? deal, Guid userId, UserEntity? user, string? text)
        {
      
            DealId=dealId;
            Deal=deal;
            UserId=userId;
            User=user;
            Text=text;
        }
        public MessageEntity()
        {
            
        }
    }
}
