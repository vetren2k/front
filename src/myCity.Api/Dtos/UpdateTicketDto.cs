using myCity.Api.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace myCity.Api.Dtos
{
    public class UpdateTicketDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ID działu musi być większe od 0.")]
        public int PublicBodyDepartmentId { get; set; }

        [Required]
        public TicketPriority Priority { get; set; }
    }
}
