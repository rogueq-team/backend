using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Backend;
using Backend.Entities;
using Backend.Enums;

namespace Backend.Models
{
    public class AppToFront
    {

        
    
        public Guid ApplicationId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.InProgress;
        public List<ApplicationCategoryEntity>? ApplicationCategories { get; set; }
        public List<CategoryEntity>? Categories { get; set; }

        public AppToFront(){}

        public AppToFront(ApplicationEntity app)
        {
            ApplicationId=app.ApplicationId;
            Description=app.Description;
            Cost=app.Cost;
            Status=app.Status;
            ApplicationCategories=app.ApplicationCategories;
            Categories=app.Categories;
        }

    }
    

}

