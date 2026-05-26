using myCity.Api.Entities;
using myCity.Api.Dtos;

namespace myCity.Api.Services
{
    public interface ITicketService
    {
        Task<TicketDetailsDto?> GetTicketByIdAsync(int ticketId);
        Task<IEnumerable<TicketDto>> GetTicketsAsync();
        Task<TicketDto> CreateTicketAsync(CreateTicketResidentDto dto, int creatorId);
        Task<TicketDetailsDto?> UpdateTicketAsync(int ticketId, UpdateTicketDto dto, int officialId);
        Task<TicketDetailsDto?> AssignContractorAsync(int ticketId, AssignContractorDto dto, int officialId);
        Task<IEnumerable<TicketDto>> GetAssignedTicketsAsync(int contractorId);
        Task<TicketDetailsDto?> ChangeTicketStatusAsync(int ticketId, ChangeTicketStatusDto dto, int contractorId);
        Task<IEnumerable<TicketDto>> GetMyTicketsAsync(int creatorId);

        //zmiana przez urzednika - zamkniecie lub odrzucenie ticketu
        Task<TicketDetailsDto?> ChangeTicketStatusByOfficialAsync(int ticketId, ChangeTicketStatusDto dto, int officialId);
        Task<TicketDetailsDto?> AddCommentByOfficialAsync(int ticketId, AddCommentDto dto, int officialId);
        Task<bool> DeleteCommentByOfficialAsync(int commentId);
        Task<bool> DeleteTicketAsync(int ticketId);
        
    }
}
