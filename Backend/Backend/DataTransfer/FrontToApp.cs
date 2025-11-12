using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Backend;
using Backend.Entities;
using Backend.Enums;

namespace Backend.Models
{
    public class FrontToApp
    {

        [JsonIgnore]
        public Guid ApplicationId { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Сумма должна быть положительной")]
        public decimal Cost { get; set; }
         [Required]
        public ApplicationStatus Status { get; set; }

    }

}

