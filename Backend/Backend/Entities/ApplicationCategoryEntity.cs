namespace Backend.Entities
{
    public class ApplicationCategoryEntity
    {
        public Guid ApplicationId { get; set; }
        public Guid CategoryId { get; set; }
        public ApplicationEntity Application { get; set; } = null!;
        public CategoryEntity Category { get; set; } = null!;
    }
}
