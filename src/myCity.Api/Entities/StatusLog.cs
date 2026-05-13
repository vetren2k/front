using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using myCity.Api.Entities.Enums;

namespace myCity.Api.Entities
{
    public class StatusLog
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public TicketStatus Title { get; set; }
        public string? Comment { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public int? CreatorId { get; set; }

        // Relationships
        public Ticket Ticket { get; set; } = null!;
        [ForeignKey("CreatorId")] public User? User { get; set; }
    }
}