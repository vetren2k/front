using myCity.Api.Entities.Enums;

namespace myCity.Api.Dtos
{
    public class StatusLogDto
    {
        public int Id { get; set; }
        public TicketStatus Title { get; set; }
        public string? Comment { get; set; }
        public DateTime Timestamp { get; set; }
        public string CreatorName { get; set; } = string.Empty;
    }
}
