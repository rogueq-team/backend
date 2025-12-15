namespace Backend.DataTransfer
{
    public class CreateFeedbackDto
    {
        public Guid DealId { get; set; }
        public int Stars { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
