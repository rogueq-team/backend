using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Application
    {
        private int applicationId;
        private int userId;
        private string description = string.Empty;
        private decimal cost;
        private string status = "New";
        private DateTime createdAt;
        private DateTime updatedAt;
        private DateTime? deletedAt;


        [Key]
        public int ApplicationId
        { get { return applicationId; } set { applicationId = value; } }

        [Required]
        public int UserId
        { get { return userId; } set { userId = value; } }

        [Required]
        [MaxLength(1000)]
        public string Description
        { get { return description; } set { description = value; } }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Cost { get { return cost; } set { cost = value; } }

        [Required]
        [RegularExpression("New|InProgress|Completed|Cancelled",
            ErrorMessage = "Статус может быть только New, InProgress, Completed или Cancelled")]
        public string Status
        { get { return status; } set { status = value; } }

        [Required]
        public DateTime CreatedAt
        { get { return createdAt; } set { createdAt = value; } }

        public DateTime UpdatedAt
        { get { return updatedAt; } set { updatedAt = value; } }

        public DateTime? DeletedAt
        { get { return deletedAt; } set { deletedAt = value; } }

        public Application()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
        }

    }
}