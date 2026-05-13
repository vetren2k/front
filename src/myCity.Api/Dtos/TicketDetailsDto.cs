using myCity.Api.Entities.Enums;

namespace myCity.Api.Dtos
{
    public class TicketDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? PhotoUrl { get; set; }

        public string FullAddress { get; set; } = string.Empty;

        public TicketPriority Priority { get; set; }
        public TicketStatus Status { get; set; }

        public string DepartmentName { get; set; } = string.Empty;
        public string CreatorName { get; set; } = string.Empty;
        public string AssignedOfficialName { get; set; } = string.Empty;
        public string AssignedContractorName { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public DateTime CreationTimestamp { get; set; }
        public DateTime CurrentStatusTimestamp { get; set; }

        //historia zmian
        public List<StatusLogDto> History { get; set; } = new List<StatusLogDto>();
    }
}
