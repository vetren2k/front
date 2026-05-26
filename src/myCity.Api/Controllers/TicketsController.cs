using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myCity.Api.Dtos;
using myCity.Api.Entities;
using myCity.Api.Services;
using System.Security.Claims;

namespace myCity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

       
        private int GetCurrentUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
                throw new UnauthorizedAccessException("Brak ID użytkownika w tokenie.");

            return int.Parse(userIdString);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets()
        {
            var tickets = await _ticketService.GetTicketsAsync();
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<TicketDetailsDto>> GetTicketById(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }

        [HttpPost]
        public async Task<ActionResult<TicketDto>> CreateTicket([FromBody] CreateTicketResidentDto dto)
        {
            var userId = GetCurrentUserId(); 
            var createdTicket = await _ticketService.CreateTicketAsync(dto, userId);

            return CreatedAtAction(nameof(GetTickets), new { id = createdTicket.Id }, createdTicket);
        }

        [HttpGet("my-tickets")]
        
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetMyTickets()
        {
            var userId = GetCurrentUserId();
            var tickets = await _ticketService.GetMyTicketsAsync(userId);
            return Ok(tickets);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Urzędnik")] //tylko dla urzednika
        public async Task<ActionResult<TicketDetailsDto>> UpdateTicket(int id, [FromBody] UpdateTicketDto dto)
        {
            var officialId = GetCurrentUserId();
            var updatedTicket = await _ticketService.UpdateTicketAsync(id, dto, officialId);

            if (updatedTicket == null)
            {
                return NotFound("Nie znaleziono zgłoszenia o podanym ID.");
            }

            return Ok(updatedTicket);
        }

        [HttpPatch("{id}/assign")]
        [Authorize(Roles = "Urzędnik")]
        public async Task<ActionResult<TicketDetailsDto>> AssignContractor(int id, [FromBody] AssignContractorDto dto)
        {
            var officialId = GetCurrentUserId();
            var updatedTicket = await _ticketService.AssignContractorAsync(id, dto, officialId);

            if (updatedTicket == null)
            {
                return NotFound("Nie znaleziono zgłoszenia o podanym ID.");
            }

            return Ok(updatedTicket);
        }

        [HttpPatch("{id}/official-status")]
        [Authorize(Roles = "Urzędnik")]
        public async Task<ActionResult<TicketDetailsDto>> ChangeTicketStatusByOfficial(int id, [FromBody] ChangeTicketStatusDto dto)
        {
            var officialId = GetCurrentUserId();
            var updatedTicket = await _ticketService.ChangeTicketStatusByOfficialAsync(id, dto, officialId);

            if (updatedTicket == null)
            {
                return NotFound("Nie znaleziono zgłoszenia.");
            }

            return Ok(updatedTicket);
        }

        [HttpGet("assigned")]
        [Authorize(Roles = "Wykonawca")] // tylko dla wykonawcy
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetAssignedTickets()
        {
            var contractorId = GetCurrentUserId();
            var tickets = await _ticketService.GetAssignedTicketsAsync(contractorId);
            return Ok(tickets);
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Wykonawca")]
        public async Task<ActionResult<TicketDetailsDto>> ChangeTicketStatus(int id, [FromBody] ChangeTicketStatusDto dto)
        {
            var contractorId = GetCurrentUserId();

            try
            {
                var updatedTicket = await _ticketService.ChangeTicketStatusAsync(id, dto, contractorId);

                if (updatedTicket == null)
                {
                    return NotFound("Nie znaleziono zgłoszenia o podanym ID.");
                }

                return Ok(updatedTicket);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, ex.Message);
            }
        }

        [HttpPost("{id}/comments")]
        [Authorize(Roles = "Urzędnik")]
        public async Task<ActionResult<TicketDetailsDto>> AddCommentByOfficial(int id, [FromBody] AddCommentDto dto)
        {
            var officialId = GetCurrentUserId();
            var updatedTicket = await _ticketService.AddCommentByOfficialAsync(id, dto, officialId);
            
            if (updatedTicket == null)
                return NotFound("Nie znaleziono zgłoszenia.");

            return Ok(updatedTicket);
        }

        [HttpDelete("comments/{commentId}")]
        [Authorize(Roles = "Urzędnik")]
        public async Task<IActionResult> DeleteCommentByOfficial(int commentId)
        {
            var deleted = await _ticketService.DeleteCommentByOfficialAsync(commentId);
            if (!deleted) return NotFound("Nie znaleziono komentarza.");
            
            return NoContent();
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Urzędnik")] // z konta urzednika
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var deleted = await _ticketService.DeleteTicketAsync(id);

            if (!deleted)
            {
                return NotFound("Nie znaleziono zgłoszenia o podanym ID.");
            }

            return NoContent(); 
        }


    }
}