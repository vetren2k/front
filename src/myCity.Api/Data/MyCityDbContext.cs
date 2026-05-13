using Microsoft.EntityFrameworkCore;
using myCity.Api.Entities;
using myCity.Api.Entities.Enums; // Konieczne dla Enumów

namespace myCity.Api.Data
{
    public class MyCityDbContext : DbContext
    {
        public MyCityDbContext(DbContextOptions<MyCityDbContext> options) : base(options) { }

        // --- ZAKTUALIZOWANE DB-SETY ---
        public DbSet<PublicBody> PublicBodies { get; set; }
        public DbSet<PublicBodyDepartment> PublicBodyDepartments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<StatusLog> StatusLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- KONWERSJA ENUM NA STRING W BAZIE ---
            modelBuilder.Entity<StatusLog>()
                .Property(s => s.Title)
                .HasConversion<string>();

            modelBuilder.Entity<Ticket>()
                .Property(t => t.CurrentStatus)
                .HasConversion<string>();

            modelBuilder.Entity<Ticket>()
                .Property(t => t.Priority)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();


            // --- SEEDING ---

            // 1. URZĄD MIASTA
            modelBuilder.Entity<PublicBody>().HasData(
                new PublicBody { Id = 1, Name = "Miasto Gdańsk", Locality = "Gdańsk", PhoneNumber = "58 524 45 00" }
            );

            // 2. WSZYSCY UŻYTKOWNICY (MIESZKAŃCY, URZĘDNICY, WYKONAWCY)
            modelBuilder.Entity<User>().HasData(
                // Residents (Mieszkańcy)
                new User { Id = 1, Role = UserRole.Resident, Mail = "bartosz.kujawa@mail.com", Password = "zaq1@WSX", FirstName = "Bartosz", LastName = "Kujawa", PhoneNumber = "111222333", TrustScore = 100 },
                new User { Id = 2, Role = UserRole.Resident, Mail = "oskar.krenke@mail.com", Password = "zaq1@WSX", FirstName = "Oskar", LastName = "Krenke", PhoneNumber = "123123123", TrustScore = 50 },
                new User { Id = 3, Role = UserRole.Resident, Mail = "jakub.markuszewski@mail.com", Password = "zaq1@WSX", FirstName = "Jakub", LastName = "Markuszewski", PhoneNumber = "000000000", TrustScore = 0 },
                new User { Id = 4, Role = UserRole.Resident, Mail = "kacper.tubiak@mail.com", Password = "zaq1@WSX", FirstName = "Kacper", LastName = "Tubiak", PhoneNumber = "111111111", TrustScore = -10 },

                // Officials (Urzędnicy) - pole Position
                new User { Id = 5, Role = UserRole.Official, Position = "Dyrektor Wydziału", Mail = "joanna.pinska@gdansk.pl", Password = "zaq1@WSX", FirstName = "Joanna", LastName = "Pińska" },
                new User { Id = 6, Role = UserRole.Official, Position = "Dyrektor Zarządu Dróg", Mail = "anna.bobrowska@gzdiz.gda.pl", Password = "zaq1@WSX", FirstName = "Anna", LastName = "Bobrowska" },
                new User { Id = 7, Role = UserRole.Official, Position = "Dyrektor Zarządu Zieleni", Mail = "barbara.tusk-krajewska@gzz.gda.pl", Password = "zaq1@WSX", FirstName = "Barbara", LastName = "Tusk-Krajewska" },
                new User { Id = 8, Role = UserRole.Official, Position = "Dyrektor Wydziału", Mail = "anna.trzuskolas@gdansk.pl", Password = "zaq1@WSX", FirstName = "Anna", LastName = "Trzuskolas" },
                new User { Id = 9, Role = UserRole.Official, Position = "Dyrektor Wydziału", Mail = "anna.wolodzko@gdansk.pl", Password = "zaq1@WSX", FirstName = "Anna", LastName = "Wołodźko" },
                new User { Id = 10, Role = UserRole.Official, Position = "Dyrektor Wydziału", Mail = "wi@gdansk.gda.pl", Password = "zaq1@WSX", FirstName = "Piotr", LastName = "Spyra" },
                new User { Id = 11, Role = UserRole.Official, Position = "Komendant Straży Miejskiej", Mail = "leszek.walczak@strazmiejska.gda.pl", Password = "zaq1@WSX", FirstName = "Leszek", LastName = "Walczak" },
                new User { Id = 12, Role = UserRole.Official, Position = "p.o. Dyrektora ZTM", Mail = "lukasz.klos@ztm.gda.pl", Password = "zaq1@WSX", FirstName = "Łukasz", LastName = "Kłos" },
                new User { Id = 13, Role = UserRole.Official, Position = "Wiceprezes Zarządu", Mail = "danuta.jarzembowska@gdanskiewodociagi.pl", Password = "zaq1@WSX", FirstName = "Danuta", LastName = "Jarzembowska" },

                // Contractors (Wykonawcy) - pola Employer i PublicBodyDepartmentId
                new User { Id = 14, Role = UserRole.Contractor, Employer = "Miasto Gdańsk", PublicBodyDepartmentId = 1, Mail = "jan.kowalski@mycity.pl", Password = "zaq1@WSX", FirstName = "Jan", LastName = "Kowalski" },
                new User { Id = 15, Role = UserRole.Contractor, Employer = "DrogBud Sp. z o.o.", PublicBodyDepartmentId = 2, Mail = "p.nowak@drogbud.pl", Password = "zaq1@WSX", FirstName = "Piotr", LastName = "Nowak" },
                new User { Id = 16, Role = UserRole.Contractor, Employer = "Asfalt-Max", PublicBodyDepartmentId = 2, Mail = "m.wisniewski@asfalt-max.pl", Password = "zaq1@WSX", FirstName = "Michał", LastName = "Wiśniewski" },
                new User { Id = 17, Role = UserRole.Contractor, Employer = "Eko-Zieleń", PublicBodyDepartmentId = 3, Mail = "a.wojcik@eko-zielen.pl", Password = "zaq1@WSX", FirstName = "Adam", LastName = "Wójcik" },
                new User { Id = 18, Role = UserRole.Contractor, Employer = "Parki i Ogrody S.A.", PublicBodyDepartmentId = 3, Mail = "t.kowalczyk@parkiiogrody.pl", Password = "zaq1@WSX", FirstName = "Tomasz", LastName = "Kowalczyk" },
                new User { Id = 19, Role = UserRole.Contractor, Employer = "Drzew-Serwis", PublicBodyDepartmentId = 3, Mail = "k.kaminski@drzew-serwis.pl", Password = "zaq1@WSX", FirstName = "Krzysztof", LastName = "Kamiński" },
                new User { Id = 20, Role = UserRole.Contractor, Employer = "Eco-Tech", PublicBodyDepartmentId = 4, Mail = "m.lewandowski@eco-tech.pl", Password = "zaq1@WSX", FirstName = "Maciej", LastName = "Lewandowski" },
                new User { Id = 21, Role = UserRole.Contractor, Employer = "Błysk-Trans", PublicBodyDepartmentId = 5, Mail = "j.zielinski@blysk-trans.pl", Password = "zaq1@WSX", FirstName = "Jakub", LastName = "Zieliński" },
                new User { Id = 22, Role = UserRole.Contractor, Employer = "Czyste Miasto", PublicBodyDepartmentId = 5, Mail = "s.szymanski@czystemiasto.pl", Password = "zaq1@WSX", FirstName = "Szymon", LastName = "Szymański" },
                new User { Id = 23, Role = UserRole.Contractor, Employer = "InfraBud", PublicBodyDepartmentId = 6, Mail = "d.wozniak@infrabud.pl", Password = "zaq1@WSX", FirstName = "Dawid", LastName = "Woźniak" },
                new User { Id = 24, Role = UserRole.Contractor, Employer = "Miasto Gdańsk", PublicBodyDepartmentId = 7, Mail = "k.dabrowski@mycity.pl", Password = "zaq1@WSX", FirstName = "Kacper", LastName = "Dąbrowski" },
                new User { Id = 25, Role = UserRole.Contractor, Employer = "Przystanek-Serwis", PublicBodyDepartmentId = 8, Mail = "f.kozlowski@przystanek-serwis.pl", Password = "zaq1@WSX", FirstName = "Filip", LastName = "Kozłowski" },
                new User { Id = 26, Role = UserRole.Contractor, Employer = "Aqua-Bud", PublicBodyDepartmentId = 9, Mail = "m.jankowski@aqua-bud.pl", Password = "zaq1@WSX", FirstName = "Mateusz", LastName = "Jankowski" },
                new User { Id = 27, Role = UserRole.Contractor, Employer = "Hydro-Naprawa", PublicBodyDepartmentId = 9, Mail = "l.mazur@hydro-naprawa.pl", Password = "zaq1@WSX", FirstName = "Łukasz", LastName = "Mazur" }
            );

            // 3. DEPARTAMENTY (Oparte na nowej nazwie PublicBodyDepartment i relacji z ExecutiveId)
            modelBuilder.Entity<PublicBodyDepartment>().HasData(
                new PublicBodyDepartment { Id = 1, PublicBodyId = 1, Name = "Wydział Bezpieczeństwa i Zarządzania Kryzysowego", Description = "Zgłoszenia dotyczące lokalnych zagrożeń, niebezpiecznych znalezisk, dewastacji mienia publicznego oraz klęsk żywiołowych.", ExecutiveId = 5 },
                new PublicBodyDepartment { Id = 2, PublicBodyId = 1, Name = "Gdański Zarząd Dróg", Description = "Zgłoszenia dotyczące dziur w jezdni, uszkodzonych chodników, niedziałającej sygnalizacji świetlnej oraz brakujących lub zniszczonych znaków drogowych.", ExecutiveId = 6 },
                new PublicBodyDepartment { Id = 3, PublicBodyId = 1, Name = "Gdański Zarząd Zieleni", Description = "Zgłoszenia dotyczące powalonych drzew, połamanych gałęzi, niekoszonych trawników, uszkodzeń w parkach oraz zniszczonej małej architektury.", ExecutiveId = 7 },
                new PublicBodyDepartment { Id = 4, PublicBodyId = 1, Name = "Wydział Ekologii i Energetyki", Description = "Zgłoszenia dotyczące zanieczyszczenia powietrza (smog, spalanie śmieci), nielegalnych zrzutów ścieków do rzek oraz martwych zwierząt.", ExecutiveId = 8 },
                new PublicBodyDepartment { Id = 5, PublicBodyId = 1, Name = "Wydział Gospodarki Komunalnej", Description = "Zgłoszenia dotyczące dzikich wysypisk śmieci, przepełnionych śmietników, zanieczyszczenia ulic oraz problemów z wywozem odpadów.", ExecutiveId = 9 },
                new PublicBodyDepartment { Id = 6, PublicBodyId = 1, Name = "Wydział Infrastruktury", Description = "Zgłoszenia dotyczące awarii oświetlenia ulicznego, uszkodzonych barierek ochronnych oraz większych usterek infrastruktury miejskiej.", ExecutiveId = 10 },
                new PublicBodyDepartment { Id = 7, PublicBodyId = 1, Name = "Straż Miejska", Description = "Zgłoszenia dotyczące nieprawidłowego parkowania, wraków pojazdów, zakłócania porządku publicznego oraz spożywania alkoholu w miejscach niedozwolonych.", ExecutiveId = 11 },
                new PublicBodyDepartment { Id = 8, PublicBodyId = 1, Name = "Zarząd Transportu Miejskiego", Description = "Zgłoszenia dotyczące zniszczonych wiat przystankowych, rozbitych szyb, niedziałających biletomatów oraz uszkodzonych tablic informacji pasażerskiej.", ExecutiveId = 12 },
                new PublicBodyDepartment { Id = 9, PublicBodyId = 1, Name = "Gdańskie Wodociągi", Description = "Zgłoszenia dotyczące zapadniętych studzienek, brakujących włazów, awarii rur wodociągowych oraz podtopień infrastruktury drogowej.", ExecutiveId = 13 }
            );

            // 4. ZGŁOSZENIA (Zdjęcia przeniesione do PhotoUrl, Statusy zamienione na Enumy)
            modelBuilder.Entity<Ticket>().HasData(
                new Ticket
                {
                    Id = 1,
                    CreatorId = 1,
                    PublicBodyDepartmentId = 2,
                    Title = "Głęboka dziura w jezdni",
                    Description = "Na prawym pasie znajduje się głęboka wyrwa. Można uszkodzić zawieszenie.",
                    PhotoUrl = "https://example.com/images/ticket1_a.jpg",
                    Latitude = 54.378900m,
                    Longitude = 18.608700m,
                    City = "Gdańsk",
                    District = "Wrzeszcz",
                    Street = "Grunwaldzka",
                    BuildingNumber = "120",
                    Postcode = "80-244",
                    Priority = TicketPriority.High,
                    CurrentStatus = TicketStatus.New,
                    CreationTimestamp = new DateTime(2026, 4, 1, 10, 0, 0, DateTimeKind.Utc),
                    CurrentStatusTimestamp = new DateTime(2026, 4, 1, 10, 0, 0, DateTimeKind.Utc)
                },
                new Ticket
                {
                    Id = 2,
                    CreatorId = 2,
                    PublicBodyDepartmentId = 3,
                    AssignedOfficialId = 7,
                    AssignedContractorId = 17,
                    Title = "Powalone drzewo na ścieżce",
                    Description = "Po wczorajszej wichurze drzewo całkowicie blokuje ścieżkę rowerową.",
                    PhotoUrl = "https://example.com/images/ticket2.jpg",
                    Latitude = 54.409800m,
                    Longitude = 18.601200m,
                    City = "Gdańsk",
                    District = "Zaspa",
                    Street = "Jana Pawła II",
                    BuildingNumber = "3",
                    Postcode = "80-462",
                    Priority = TicketPriority.High,
                    CurrentStatus = TicketStatus.InProgress,
                    CreationTimestamp = new DateTime(2026, 4, 2, 8, 30, 0, DateTimeKind.Utc),
                    CurrentStatusTimestamp = new DateTime(2026, 4, 2, 10, 15, 0, DateTimeKind.Utc)
                },
                new Ticket
                {
                    Id = 3,
                    CreatorId = 3,
                    PublicBodyDepartmentId = 8,
                    AssignedOfficialId = 12,
                    AssignedContractorId = 25,
                    Title = "Zniszczony ekran biletomatu",
                    Description = "Ktoś rozbił ekran, nie da się kupić biletu.",
                    Note = "Wszędzie leży szkło, uważajcie",
                    PhotoUrl = "https://example.com/images/ticket3.jpg",
                    Latitude = 54.351200m,
                    Longitude = 18.646500m,
                    City = "Gdańsk",
                    District = "Śródmieście",
                    Street = "Podwale Grodzkie",
                    BuildingNumber = "1",
                    Postcode = "80-895",
                    Priority = TicketPriority.Normal,
                    CurrentStatus = TicketStatus.Resolved,
                    CreationTimestamp = new DateTime(2026, 3, 28, 14, 20, 0, DateTimeKind.Utc),
                    CurrentStatusTimestamp = new DateTime(2026, 3, 30, 16, 0, 0, DateTimeKind.Utc)
                },
                new Ticket
                {
                    Id = 4,
                    CreatorId = 4,
                    PublicBodyDepartmentId = 9,
                    AssignedOfficialId = 13,
                    AssignedContractorId = 26,
                    Title = "Wybiła studzienka",
                    Description = "Woda zalewa chodnik i śmierdzi.",
                    PhotoUrl = "https://example.com/images/ticket4.jpg",
                    Latitude = 54.346800m,
                    Longitude = 18.621400m,
                    City = "Gdańsk",
                    District = "Chełm",
                    Street = "Cienista",
                    BuildingNumber = "15",
                    Postcode = "80-805",
                    Priority = TicketPriority.Critical,
                    CurrentStatus = TicketStatus.InProgress,
                    CreationTimestamp = new DateTime(2026, 4, 5, 9, 10, 0, DateTimeKind.Utc),
                    CurrentStatusTimestamp = new DateTime(2026, 4, 5, 11, 20, 0, DateTimeKind.Utc)
                },
                new Ticket
                {
                    Id = 5,
                    CreatorId = 1,
                    PublicBodyDepartmentId = 7,
                    Title = "Porzucone auto bez tablic",
                    Description = "Stoi tu od pół roku, zajmuje miejsce, wyciekają z niego płyny.",
                    PhotoUrl = "https://example.com/images/ticket5_wrak.jpg",
                    Latitude = 54.398100m,
                    Longitude = 18.589100m,
                    City = "Gdańsk",
                    District = "Przymorze",
                    Street = "Obrońców Wybrzeża",
                    BuildingNumber = "10",
                    Postcode = "80-398",
                    Priority = TicketPriority.Low,
                    CurrentStatus = TicketStatus.New,
                    CreationTimestamp = new DateTime(2026, 4, 6, 17, 45, 0, DateTimeKind.Utc),
                    CurrentStatusTimestamp = new DateTime(2026, 4, 6, 17, 45, 0, DateTimeKind.Utc)
                },
                new Ticket
                {
                    Id = 6,
                    CreatorId = 2,
                    PublicBodyDepartmentId = 6,
                    AssignedOfficialId = 10,
                    AssignedContractorId = 23,
                    Title = "Ciemno na przejściu dla pieszych",
                    Description = "Nie pali się latarnia bezpośrednio nad pasami, jest niebezpiecznie.",
                    Latitude = 54.341500m,
                    Longitude = 18.660100m,
                    City = "Gdańsk",
                    District = "Stogi",
                    Street = "Nowotna",
                    BuildingNumber = "18",
                    Postcode = "80-620",
                    Priority = TicketPriority.Normal,
                    CurrentStatus = TicketStatus.Resolved,
                    CreationTimestamp = new DateTime(2026, 3, 20, 20, 10, 0, DateTimeKind.Utc),
                    CurrentStatusTimestamp = new DateTime(2026, 3, 22, 12, 30, 0, DateTimeKind.Utc)
                },
                new Ticket
                {
                    Id = 7,
                    CreatorId = 3,
                    PublicBodyDepartmentId = 5,
                    AssignedOfficialId = 9,
                    AssignedContractorId = 21,
                    Title = "Śmieci wysypują się na wiatr",
                    Description = "Pojemniki pełne od 3 dni, ptaki roznoszą śmieci po parku.",
                    PhotoUrl = "https://example.com/images/ticket7_smietnik.jpg",
                    Latitude = 54.415200m,
                    Longitude = 18.571400m,
                    City = "Gdańsk",
                    District = "Oliwa",
                    Street = "Opata Rybińskiego",
                    BuildingNumber = "24",
                    Postcode = "80-320",
                    Priority = TicketPriority.Low,
                    CurrentStatus = TicketStatus.InProgress,
                    CreationTimestamp = new DateTime(2026, 4, 4, 13, 0, 0, DateTimeKind.Utc),
                    CurrentStatusTimestamp = new DateTime(2026, 4, 5, 8, 0, 0, DateTimeKind.Utc)
                },
                new Ticket
                {
                    Id = 8,
                    CreatorId = 4,
                    PublicBodyDepartmentId = 5,
                    Title = "Ktoś wyrzucił gruz do lasu",
                    Description = "Na skraju lasu leży sterta worków z gruzem i stary sedes.",
                    PhotoUrl = "https://example.com/images/ticket8_gruz.jpg",
                    Latitude = 54.358100m,
                    Longitude = 18.530100m,
                    City = "Gdańsk",
                    District = "Jasień",
                    Street = "Kartuska",
                    BuildingNumber = "312",
                    Postcode = "80-125",
                    Priority = TicketPriority.Normal,
                    CurrentStatus = TicketStatus.New,
                    CreationTimestamp = new DateTime(2026, 4, 7, 11, 20, 0, DateTimeKind.Utc),
                    CurrentStatusTimestamp = new DateTime(2026, 4, 7, 11, 20, 0, DateTimeKind.Utc)
                },
                new Ticket
                {
                    Id = 9,
                    CreatorId = 1,
                    PublicBodyDepartmentId = 1,
                    AssignedOfficialId = 5,
                    AssignedContractorId = 14,
                    Title = "Gniazdo szerszeni przy placu zabaw",
                    Description = "Wielkie gniazdo na drzewie zaraz obok zjeżdżalni.",
                    Note = "Uwaga dla służb: Szerszenie są wyjątkowo agresywne, wygląda na to, że ktoś rzucał w nie kamieniami!!!",
                    PhotoUrl = "https://example.com/images/ticket9_osy.jpg",
                    Latitude = 54.332100m,
                    Longitude = 18.614500m,
                    City = "Gdańsk",
                    District = "Orunia",
                    Street = "Gościnna",
                    BuildingNumber = "5",
                    Postcode = "80-032",
                    Priority = TicketPriority.Critical,
                    CurrentStatus = TicketStatus.Resolved,
                    CreationTimestamp = new DateTime(2026, 3, 25, 15, 0, 0, DateTimeKind.Utc),
                    CurrentStatusTimestamp = new DateTime(2026, 3, 25, 18, 45, 0, DateTimeKind.Utc)
                },
                new Ticket
                {
                    Id = 10,
                    CreatorId = 2,
                    PublicBodyDepartmentId = 2,
                    AssignedOfficialId = 6,
                    AssignedContractorId = 16,
                    Title = "Znak STOP obrócony w złą stronę",
                    Description = "Znak obrócił się na wietrze, kierowcy go nie widzą.",
                    PhotoUrl = "https://example.com/images/ticket10_znak.jpg",
                    Latitude = 54.352800m,
                    Longitude = 18.640100m,
                    City = "Gdańsk",
                    District = "Śródmieście",
                    Street = "Targ Drzewny",
                    BuildingNumber = "1",
                    Postcode = "80-886",
                    Priority = TicketPriority.High,
                    CurrentStatus = TicketStatus.InProgress,
                    CreationTimestamp = new DateTime(2026, 4, 6, 9, 30, 0, DateTimeKind.Utc),
                    CurrentStatusTimestamp = new DateTime(2026, 4, 7, 10, 0, 0, DateTimeKind.Utc)
                }
            );

            // 5. STATUS LOGI (Z uwzględnieniem Enumów i nazwy Comment)
            modelBuilder.Entity<StatusLog>().HasData(
                new StatusLog { Id = 1, TicketId = 2, CreatorId = 7, Title = TicketStatus.InProgress, Comment = "Skierowano firmę Eko-Zieleń do usunięcia drzewa.", Timestamp = new DateTime(2026, 4, 2, 10, 15, 0, DateTimeKind.Utc) },
                new StatusLog { Id = 2, TicketId = 3, CreatorId = 12, Title = TicketStatus.InProgress, Comment = "Wysłano serwis.", Timestamp = new DateTime(2026, 3, 29, 9, 0, 0, DateTimeKind.Utc) },
                new StatusLog { Id = 3, TicketId = 3, CreatorId = 25, Title = TicketStatus.Resolved, Comment = "Matryca została wymieniona. Biletomat sprawny.", Timestamp = new DateTime(2026, 3, 30, 16, 0, 0, DateTimeKind.Utc) },
                new StatusLog { Id = 4, TicketId = 4, CreatorId = 13, Title = TicketStatus.InProgress, Comment = "Ekipa Wodociągów jest w drodze.", Timestamp = new DateTime(2026, 4, 5, 11, 20, 0, DateTimeKind.Utc) },
                new StatusLog { Id = 5, TicketId = 6, CreatorId = 10, Title = TicketStatus.InProgress, Comment = "Zlecono wymianę lampy.", Timestamp = new DateTime(2026, 3, 21, 8, 15, 0, DateTimeKind.Utc) },
                new StatusLog { Id = 6, TicketId = 6, CreatorId = 23, Title = TicketStatus.Resolved, Comment = "Latarnia znowu świeci.", Timestamp = new DateTime(2026, 3, 22, 12, 30, 0, DateTimeKind.Utc) },
                new StatusLog { Id = 7, TicketId = 7, CreatorId = 9, Title = TicketStatus.InProgress, Comment = "Zgłoszono do natychmiastowego wywozu pozaharmonogramowego.", Timestamp = new DateTime(2026, 4, 5, 8, 0, 0, DateTimeKind.Utc) },
                new StatusLog { Id = 8, TicketId = 9, CreatorId = 5, Title = TicketStatus.InProgress, Comment = "Skierowano straż pożarną.", Timestamp = new DateTime(2026, 3, 25, 15, 30, 0, DateTimeKind.Utc) },
                new StatusLog { Id = 9, TicketId = 9, CreatorId = 5, Title = TicketStatus.Resolved, Comment = "Teren zabezpieczony, zagrożenie zneutralizowane.", Timestamp = new DateTime(2026, 3, 25, 18, 45, 0, DateTimeKind.Utc) },
                new StatusLog { Id = 10, TicketId = 10, CreatorId = 6, Title = TicketStatus.InProgress, Comment = "Ekipa DrogBud została powiadomiona.", Timestamp = new DateTime(2026, 4, 7, 10, 0, 0, DateTimeKind.Utc) }
            );
        }
    }
}