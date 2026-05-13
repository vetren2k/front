using myCity.Api.Entities.Enums;

namespace myCity.Api.Dtos
{
    public class TicketDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

       
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string FullAddress { get; set; } = string.Empty;

      
        public TicketPriority Priority { get; set; }
        public TicketStatus Status { get; set; }

       
        public string DepartmentName { get; set; } = string.Empty;

        
        public string CreatorName { get; set; } = string.Empty;
    }
}
