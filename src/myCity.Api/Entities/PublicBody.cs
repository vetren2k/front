namespace myCity.Api.Entities
{
    public class PublicBody
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Locality { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }

        // Relationships
        public ICollection<PublicBodyDepartment> PublicBodyDepartments { get; set; } = new List<PublicBodyDepartment>();
    }
}