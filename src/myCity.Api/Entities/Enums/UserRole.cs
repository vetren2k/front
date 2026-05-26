namespace myCity.Api.Entities.Enums
{
    public enum UserRole { Mieszkaniec = 1, Urzędnik = 2, Wykonawca = 3, Admin = 4 } //EF zmieni na liczbe do tabeli
    public enum TicketStatus { New = 1, InProgress = 2, Resolved = 3, Rejected = 4 }
    public enum TicketPriority { Low = 1, Normal = 2, High = 3, Critical = 4}
}