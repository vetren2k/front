using System.ComponentModel.DataAnnotations;

namespace myCity.Api.Dtos
{
    public class AssignContractorDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Wybierz poprawnego Wykonawcę.")]
        public int ContractorId { get; set; }
    }
}
