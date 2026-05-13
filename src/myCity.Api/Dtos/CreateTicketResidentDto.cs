using System.ComponentModel.DataAnnotations;

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

        // resident daje adres tylko
        [Required] public string City { get; set; } = string.Empty;
        [Required] public string District { get; set; } = string.Empty;
        [Required] public string Street { get; set; } = string.Empty;
        [Required] public string BuildingNumber { get; set; } = string.Empty;
        public string? FlatNumber { get; set; } //opcjonalen
        [Required] public string Postcode { get; set; } = string.Empty;
    }
}
