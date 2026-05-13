using myCity.Api.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace myCity.Api.Entities
{
    public class User
    {   
        public int Id { get; set; }
        public string Mail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreationTimestamp { get; set; } = DateTime.UtcNow;
        public UserRole Role { get; set; }

        // resident
        public string? PhoneNumber { get; set; }
        public int? TrustScore { get; set; } = 0;

        //official
        public string? Position { get; set; } = string.Empty;

        // contractor
        public string? Employer { get; set; }
        public int? PublicBodyDepartmentId { get; set; }


        // Relationships
        [InverseProperty("Executive")]
        public PublicBodyDepartment? ExecutiveDepartment { get; set; }

        [ForeignKey("PublicBodyDepartmentId")]
        [InverseProperty("Contractors")]
        public PublicBodyDepartment? ContractorDepartment { get; set; }

        [InverseProperty("Creator")]
        public ICollection<Ticket> CreatedTickets { get; set; } = new List<Ticket>();

        [InverseProperty("Official")]
        public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();

        [InverseProperty("Contractor")]
        public ICollection<Ticket> AssignedTicketsToSolve { get; set; } = new List<Ticket>();

        [InverseProperty("User")]
        public ICollection<StatusLog> StatusLogs { get; set; } = new List<StatusLog>();
    }
}