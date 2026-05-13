using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using myCity.Api.Entities.Enums;

namespace myCity.Api.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Note { get; set; } 

        public string? PhotoUrl { get; set; }//zamiast osobnej tabeli

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string BuildingNumber { get; set; } = string.Empty;
        public string? FlatNumber { get; set; }
        public string Postcode { get; set; } = string.Empty;
        public int? AssignedOfficialId { get; set; }
        public int? AssignedContractorId { get; set; }
        public int? PublicBodyDepartmentId { get; set; }

        public TicketPriority Priority { get; set; } = TicketPriority.Normal;
        public TicketStatus CurrentStatus { get; set; } = TicketStatus.New;

        public DateTime CurrentStatusTimestamp { get; set; } = DateTime.UtcNow;
        public DateTime CreationTimestamp { get; set; } = DateTime.UtcNow;



        // Relationships
        [ForeignKey("CreatorId")] public User Creator { get; set; } = null!;
        [ForeignKey("PublicBodyDepartmentId")] public PublicBodyDepartment? PublicBodyDepartment { get; set; }
        [ForeignKey("AssignedOfficialId")] public User? Official { get; set; }
        [ForeignKey("AssignedContractorId")] public User? Contractor { get; set; }
        public ICollection<StatusLog> StatusLogs { get; set; } = new List<StatusLog>();
    }
}