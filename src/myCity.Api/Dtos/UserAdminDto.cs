namespace myCity.Api.Dtos
{
    public class UserAdminDto
    {
        public int Id { get; set; }
        public string Mail { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
