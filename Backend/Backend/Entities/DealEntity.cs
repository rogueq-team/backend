using Backend.Abstraction;
using Backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend;
using Backend.Entities;

public class DealEntity:BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid DealId { get; set; }

    [Required]
    public ApplicationEntity? Application { get; set; }

    [Required]
    public Guid ApplicationId { get; set; }

    [Required]
    public Guid AdvertiserId { get; set; }
    [Required]
    public Guid PlatformId { get; set; }

    [MaxLength(600)]
    public string? Description { get; set; }

    [Required]
    public DealStatus Status { get; set; } //inProgress   isOver    cancel

    public List<MessageEntity>? Messages { get; set; }

    public TransactionEntity? Transaction { get; set; }

    public virtual Backend.Entities.UserEntity? Advertiser { get; set; }
    public virtual Backend.Entities.UserEntity? Platform { get; set; }
   
}

