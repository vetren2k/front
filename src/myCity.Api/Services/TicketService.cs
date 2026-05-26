using Microsoft.EntityFrameworkCore;
using myCity.Api.Data;
using myCity.Api.Entities;
using myCity.Api.Entities.Enums;
using myCity.Api.Dtos;
using System.Text.Json;

namespace myCity.Api.Services
{
    public class TicketService : ITicketService
    {

        private readonly MyCityDbContext _context;

        // DI - baza
        public TicketService(MyCityDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TicketDto>> GetTicketsAsync()
        {
            var tickets = await _context.Tickets
                .Include(t => t.PublicBodyDepartment)
                .Include(t => t.Creator)
                .Select(x => new TicketDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    FullAddress = $"{x.City}, ul. {x.Street} {x.BuildingNumber}",
                    Priority = x.Priority,
                    Status = x.CurrentStatus,
                    DepartmentName = x.PublicBodyDepartment != null ? x.PublicBodyDepartment.Name : "Brak przypisania",
                    CreatorName = $"{x.Creator.FirstName} {x.Creator.LastName}",
                    CreationTimestamp = x.CreationTimestamp
                })
                .ToListAsync();

            return tickets;
        }


        public async Task<TicketDto> CreateTicketAsync(CreateTicketResidentDto dto, int creatorId)
        {
            decimal lat = dto.Latitude ?? 0m;
            decimal lng = dto.Longitude ?? 0m;

            if (lat == 0m && lng == 0m)
            {
                var coords = await GetCoordinatesAsync(dto.City, dto.Street, dto.BuildingNumber);
                lat = coords.Latitude;
                lng = coords.Longitude;
            }

            var newTicket = new Ticket
            {
                CreatorId = creatorId,
                Title = dto.Title,
                Description = dto.Description,
                PhotoUrl = dto.PhotoUrl,
                City = dto.City,
                District = dto.District,
                Street = dto.Street,
                BuildingNumber = dto.BuildingNumber,
                FlatNumber = dto.FlatNumber,
                Postcode = dto.Postcode,
                Latitude = lat,
                Longitude = lng,
                CurrentStatus = TicketStatus.New,
                Priority = dto.Priority ?? TicketPriority.Normal,
                CreationTimestamp = DateTime.UtcNow,
                CurrentStatusTimestamp = DateTime.UtcNow
            };

            
            _context.Tickets.Add(newTicket);
            await _context.SaveChangesAsync();

            // od razu tworze pierwszy log
            var firstLog = new StatusLog
            {
                TicketId = newTicket.Id,
                Title = TicketStatus.New,
                Comment = "Zgłoszenie utworzone przez mieszkańca.",
                CreatorId = creatorId,
                Timestamp = DateTime.UtcNow
            };

            _context.StatusLogs.Add(firstLog);
            await _context.SaveChangesAsync();

            
            var creator = await _context.Users.FindAsync(creatorId);

            return new TicketDto
            {
                Id = newTicket.Id,
                Title = newTicket.Title,
                Description = newTicket.Description,
                Status = newTicket.CurrentStatus,
                CreatorName = creator != null ? $"{creator.FirstName} {creator.LastName}" : "Nieznany"
            };
        }


        public async Task<TicketDetailsDto?> GetTicketByIdAsync(int ticketId)
        {
            var ticket = await _context.Tickets
                .Include(t => t.PublicBodyDepartment)
                .Include(t => t.Creator)
                .Include(t => t.Official)
                .Include(t => t.Contractor)
                .Include(t => t.StatusLogs) 
                    .ThenInclude(log => log.User) 
                .FirstOrDefaultAsync(t => t.Id == ticketId); 

            if (ticket == null)
            {
                return null;
            }

            return new TicketDetailsDto
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                PhotoUrl = ticket.PhotoUrl,
                FullAddress = $"{ticket.City}, ul. {ticket.Street} {ticket.BuildingNumber}",
                Priority = ticket.Priority,
                Status = ticket.CurrentStatus,
                Latitude = ticket.Latitude,
                Longitude = ticket.Longitude,
                CreationTimestamp = ticket.CreationTimestamp,
                CurrentStatusTimestamp = ticket.CurrentStatusTimestamp,

                DepartmentName = ticket.PublicBodyDepartment != null ? ticket.PublicBodyDepartment.Name : "Brak",
                CreatorName = $"{ticket.Creator.FirstName} {ticket.Creator.LastName}",
                AssignedOfficialName = ticket.Official != null ? $"{ticket.Official.FirstName} {ticket.Official.LastName}" : "Brak",
                AssignedContractorName = ticket.Contractor != null ? $"{ticket.Contractor.FirstName} {ticket.Contractor.LastName}" : "Brak",

                
                History = ticket.StatusLogs
                    .OrderByDescending(log => log.Timestamp)
                    .Select(log => new StatusLogDto
                    {
                        Id = log.Id,
                        Title = log.Title,
                        Comment = log.Comment,
                        Timestamp = log.Timestamp,
                        CreatorName = log.User != null ? $"{log.User.FirstName} {log.User.LastName}" : "System"
                    }).ToList()
            };
        }



        public async Task<TicketDetailsDto?> UpdateTicketAsync(int ticketId, UpdateTicketDto dto, int officialId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);

            if (ticket == null)
            {
                return null;
            }

            //akt danych
            ticket.PublicBodyDepartmentId = dto.PublicBodyDepartmentId;
            ticket.Priority = dto.Priority;

            // log o zmianie
            var log = new StatusLog
            {
                TicketId = ticket.Id,
                Title = ticket.CurrentStatus, //status pokio co ten sam
                Comment = "Urzędnik zaktualizował dział i priorytet zgłoszenia.",
                CreatorId = officialId,
                Timestamp = DateTime.UtcNow
            };

            _context.StatusLogs.Add(log);

            await _context.SaveChangesAsync();

            return await GetTicketByIdAsync(ticket.Id);
        }


        public async Task<TicketDetailsDto?> AssignContractorAsync(int ticketId, AssignContractorDto dto, int officialId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);

            if (ticket == null)
            {
                return null;
            }

            
            ticket.AssignedContractorId = dto.ContractorId;
            ticket.AssignedOfficialId = officialId; 

            //w trakcie
            ticket.CurrentStatus = Entities.Enums.TicketStatus.InProgress;
            ticket.CurrentStatusTimestamp = DateTime.UtcNow;

            // log
            var log = new StatusLog
            {
                TicketId = ticket.Id,
                Title = Entities.Enums.TicketStatus.InProgress,
                Comment = "Zgłoszenie zostało przekazane do realizacji Wykonawcy.",
                CreatorId = officialId,
                Timestamp = DateTime.UtcNow
            };

            _context.StatusLogs.Add(log);

            await _context.SaveChangesAsync();

            return await GetTicketByIdAsync(ticket.Id);
        }

        public async Task<IEnumerable<TicketDto>> GetAssignedTicketsAsync(int contractorId)
        {
            return await _context.Tickets
                .Include(t => t.PublicBodyDepartment)
                .Include(t => t.Creator)
                // tylko gdzie id sie zhadza
                .Where(t => t.AssignedContractorId == contractorId)
                .Select(x => new TicketDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    FullAddress = $"{x.City}, ul. {x.Street} {x.BuildingNumber}",
                    Priority = x.Priority,
                    Status = x.CurrentStatus,
                    DepartmentName = x.PublicBodyDepartment != null ? x.PublicBodyDepartment.Name : "Brak przypisania",
                    CreatorName = $"{x.Creator.FirstName} {x.Creator.LastName}"
                })
                .ToListAsync();
        }

        public async Task<TicketDetailsDto?> ChangeTicketStatusAsync(int ticketId, ChangeTicketStatusDto dto, int contractorId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);

            if (ticket == null) return null;

            //czy ma prawo 
            if (ticket.AssignedContractorId != contractorId)
            {
                throw new UnauthorizedAccessException("Nie możesz zmieniać statusu cudzego zgłoszenia!");
            }

            
            ticket.CurrentStatus = dto.NewStatus;
            ticket.CurrentStatusTimestamp = DateTime.UtcNow;

            
            var log = new StatusLog
            {
                TicketId = ticket.Id,
                Title = dto.NewStatus,
                Comment = string.IsNullOrWhiteSpace(dto.Comment) ? "Zmieniono status zgłoszenia." : dto.Comment,
                CreatorId = contractorId,
                Timestamp = DateTime.UtcNow
            };

            _context.StatusLogs.Add(log);

            
            await _context.SaveChangesAsync();

            return await GetTicketByIdAsync(ticket.Id);
        }


        public async Task<IEnumerable<TicketDto>> GetMyTicketsAsync(int creatorId)
        {
            //po ID twórcy
            return await _context.Tickets
                .Include(t => t.PublicBodyDepartment)
                .Include(t => t.Creator)
                .Where(t => t.CreatorId == creatorId) 
                .Select(x => new TicketDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    FullAddress = $"{x.City}, ul. {x.Street} {x.BuildingNumber}",
                    Priority = x.Priority,
                    Status = x.CurrentStatus,
                    DepartmentName = x.PublicBodyDepartment != null ? x.PublicBodyDepartment.Name : "Brak przypisania",
                    CreatorName = $"{x.Creator.FirstName} {x.Creator.LastName}"
                })
                .ToListAsync();
        }

        public async Task<TicketDetailsDto?> ChangeTicketStatusByOfficialAsync(int ticketId, ChangeTicketStatusDto dto, int officialId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);

            if (ticket == null) return null;

            
            ticket.CurrentStatus = dto.NewStatus;
            ticket.CurrentStatusTimestamp = DateTime.UtcNow;

            //log
            var log = new StatusLog
            {
                TicketId = ticket.Id,
                Title = dto.NewStatus,
                Comment = string.IsNullOrWhiteSpace(dto.Comment) ? "Urzędnik zmienił status zgłoszenia." : dto.Comment,
                CreatorId = officialId,
                Timestamp = DateTime.UtcNow
            };

            _context.StatusLogs.Add(log);
            await _context.SaveChangesAsync();

            return await GetTicketByIdAsync(ticket.Id);
        }

        public async Task<TicketDetailsDto?> AddCommentByOfficialAsync(int ticketId, AddCommentDto dto, int officialId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null) return null;

            var log = new StatusLog
            {
                TicketId = ticket.Id,
                Title = ticket.CurrentStatus, // keep current status
                Comment = dto.Comment,
                CreatorId = officialId,
                Timestamp = DateTime.UtcNow
            };

            _context.StatusLogs.Add(log);
            await _context.SaveChangesAsync();

            return await GetTicketByIdAsync(ticket.Id);
        }

        public async Task<bool> DeleteCommentByOfficialAsync(int commentId)
        {
            var log = await _context.StatusLogs.FindAsync(commentId);
            if (log == null) return false;

            _context.StatusLogs.Remove(log);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> DeleteTicketAsync(int ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);

            if (ticket == null)
            {
                return false;
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return true;
        }


        // chat mi dal do mapy 
        private async Task<(decimal Latitude, decimal Longitude)> GetCoordinatesAsync(string city, string street, string buildingNumber)
        {
            try
            {
                using var client = new HttpClient();
                // OpenStreetMap wymaga, abyśmy się "przedstawili" (User-Agent)
                client.DefaultRequestHeaders.Add("User-Agent", "myCity_StudentProject_API/1.0");

                var searchQuery = Uri.EscapeDataString($"{street} {buildingNumber}, {city}, Poland");

                // Budujemy link do darmowego API (szukamy w Polsce, po mieście, ulicy i numerze)
                var url = $"https://nominatim.openstreetmap.org/search?street={buildingNumber} {street}&city={city}&country=Poland&format=json";

                // Wysyłamy zapytanie
                var response = await client.GetFromJsonAsync<JsonElement[]>(url);

                if (response != null && response.Length > 0)
                {
                    // API zwraca tablicę wyników. Bierzemy pierwszy (najlepszy) i wyciągamy lat i lon
                    var latStr = response[0].GetProperty("lat").GetString();
                    var lonStr = response[0].GetProperty("lon").GetString();

                    // Zamieniamy tekst na ułamki dziesiętne
                    var culture = System.Globalization.CultureInfo.InvariantCulture;
                    return (Convert.ToDecimal(latStr, culture), Convert.ToDecimal(lonStr, culture));
                }
            }
            catch (Exception)
            {
                // Jeśli mapa akurat nie działa, aplikacja nie może wywalić błędu 500!
                // Zwracamy po prostu 0, 0, żeby proces przeszedł dalej.
            }

            return (0m, 0m);
        }





    }
}
