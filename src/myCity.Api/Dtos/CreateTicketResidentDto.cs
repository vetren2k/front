using System.ComponentModel.DataAnnotations;
using myCity.Api.Entities.Enums;

namespace myCity.Api.Dtos
{
    public class CreateTicketResidentDto
    {
        [Required(ErrorMessage = "Tytuł jest wymagany")]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Opis jest wymagany")]
        public string Description { get; set; } = string.Empty;

        public string? PhotoUrl { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public TicketPriority? Priority { get; set; }

        [Required] public string City { get; set; } = string.Empty;
        [Required] public string District { get; set; } = string.Empty;
        [Required] public string Street { get; set; } = string.Empty;
        public string BuildingNumber { get; set; } = string.Empty;
        public string? FlatNumber { get; set; } 
        public string Postcode { get; set; } = string.Empty;
    }
}
