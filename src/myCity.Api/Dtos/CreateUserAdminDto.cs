namespace myCity.Api.Dtos
{
    public class CreateUserAdminDto
    {
        public string Mail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // "Resident", "Official", "Contractor", "Admin"
    }
}
