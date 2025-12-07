using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Backend;
using Backend.Entities;
using Backend.Enums;

namespace Backend.Models
{
    
    public class MassegeDto
    {
        [Required]
        public string? Text{get;set;}=string.Empty;
        [Required]
        public Guid DealId {get;set;}=Guid.Empty;

    }
}