using myCity.Api.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace myCity.Api.Dtos
{
    public class ChangeTicketStatusDto
    {
        [Required]
        public TicketStatus NewStatus { get; set; }
        public string? Comment { get; set; }
    }
}
