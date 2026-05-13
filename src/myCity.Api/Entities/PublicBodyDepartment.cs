using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace myCity.Api.Entities
{
    public class PublicBodyDepartment
    {
        public int Id { get; set; }
        public int PublicBodyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ExecutiveId { get; set; }

        // Relationships
        public PublicBody PublicBody { get; set; } = null!;

        [ForeignKey("ExecutiveId")]
        [InverseProperty("ExecutiveDepartment")]
        public User Executive { get; set; } = null!;

        [InverseProperty("ContractorDepartment")]
        public ICollection<User> Contractors { get; set; } = new List<User>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
