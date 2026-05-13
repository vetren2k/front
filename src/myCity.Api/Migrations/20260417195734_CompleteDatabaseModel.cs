using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace myCity.Api.Migrations
{
    /// <inheritdoc />
    public partial class CompleteDatabaseModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "public_bodies",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    locality = table.Column<string>(type: "text", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_public_bodies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "public_body_departments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    public_body_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    executive_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_public_body_departments", x => x.id);
                    table.ForeignKey(
                        name: "fk_public_body_departments_public_bodies_public_body_id",
                        column: x => x.public_body_id,
                        principalTable: "public_bodies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mail = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    creation_timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    trust_score = table.Column<int>(type: "integer", nullable: true),
                    position = table.Column<string>(type: "text", nullable: true),
                    employer = table.Column<string>(type: "text", nullable: true),
                    public_body_department_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_public_body_departments_public_body_department_id",
                        column: x => x.public_body_department_id,
                        principalTable: "public_body_departments",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    creator_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    photo_url = table.Column<string>(type: "text", nullable: true),
                    latitude = table.Column<decimal>(type: "numeric", nullable: false),
                    longitude = table.Column<decimal>(type: "numeric", nullable: false),
                    city = table.Column<string>(type: "text", nullable: false),
                    district = table.Column<string>(type: "text", nullable: false),
                    street = table.Column<string>(type: "text", nullable: false),
                    building_number = table.Column<string>(type: "text", nullable: false),
                    flat_number = table.Column<string>(type: "text", nullable: true),
                    postcode = table.Column<string>(type: "text", nullable: false),
                    assigned_official_id = table.Column<int>(type: "integer", nullable: true),
                    assigned_contractor_id = table.Column<int>(type: "integer", nullable: true),
                    public_body_department_id = table.Column<int>(type: "integer", nullable: false),
                    priority = table.Column<string>(type: "text", nullable: false),
                    current_status = table.Column<string>(type: "text", nullable: false),
                    current_status_timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    creation_timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tickets", x => x.id);
                    table.ForeignKey(
                        name: "fk_tickets_public_body_departments_public_body_department_id",
                        column: x => x.public_body_department_id,
                        principalTable: "public_body_departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_tickets_users_assigned_contractor_id",
                        column: x => x.assigned_contractor_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tickets_users_assigned_official_id",
                        column: x => x.assigned_official_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tickets_users_creator_id",
                        column: x => x.creator_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "status_logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ticket_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    creator_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_status_logs", x => x.id);
                    table.ForeignKey(
                        name: "fk_status_logs_tickets_ticket_id",
                        column: x => x.ticket_id,
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_status_logs_users_creator_id",
                        column: x => x.creator_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                table: "public_bodies",
                columns: new[] { "id", "locality", "name", "phone_number" },
                values: new object[] { 1, "Gdańsk", "Miasto Gdańsk", "58 524 45 00" });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "creation_timestamp", "employer", "first_name", "is_active", "last_name", "mail", "password", "phone_number", "position", "public_body_department_id", "role", "trust_score" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2446), null, "Bartosz", true, "Kujawa", "bartosz.kujawa@mail.com", "zaq1@WSX", "111222333", "", null, "Resident", 100 },
                    { 2, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2459), null, "Oskar", true, "Krenke", "oskar.krenke@mail.com", "zaq1@WSX", "123123123", "", null, "Resident", 50 },
                    { 3, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2461), null, "Jakub", true, "Markuszewski", "jakub.markuszewski@mail.com", "zaq1@WSX", "000000000", "", null, "Resident", 0 },
                    { 4, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2464), null, "Kacper", true, "Tubiak", "kacper.tubiak@mail.com", "zaq1@WSX", "111111111", "", null, "Resident", -10 },
                    { 5, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2466), null, "Joanna", true, "Pińska", "joanna.pinska@gdansk.pl", "zaq1@WSX", null, "Dyrektor Wydziału", null, "Official", 0 },
                    { 6, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2468), null, "Anna", true, "Bobrowska", "anna.bobrowska@gzdiz.gda.pl", "zaq1@WSX", null, "Dyrektor Zarządu Dróg", null, "Official", 0 },
                    { 7, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2470), null, "Barbara", true, "Tusk-Krajewska", "barbara.tusk-krajewska@gzz.gda.pl", "zaq1@WSX", null, "Dyrektor Zarządu Zieleni", null, "Official", 0 },
                    { 8, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2471), null, "Anna", true, "Trzuskolas", "anna.trzuskolas@gdansk.pl", "zaq1@WSX", null, "Dyrektor Wydziału", null, "Official", 0 },
                    { 9, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2473), null, "Anna", true, "Wołodźko", "anna.wolodzko@gdansk.pl", "zaq1@WSX", null, "Dyrektor Wydziału", null, "Official", 0 },
                    { 10, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2475), null, "Piotr", true, "Spyra", "wi@gdansk.gda.pl", "zaq1@WSX", null, "Dyrektor Wydziału", null, "Official", 0 },
                    { 11, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2477), null, "Leszek", true, "Walczak", "leszek.walczak@strazmiejska.gda.pl", "zaq1@WSX", null, "Komendant Straży Miejskiej", null, "Official", 0 },
                    { 12, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2478), null, "Łukasz", true, "Kłos", "lukasz.klos@ztm.gda.pl", "zaq1@WSX", null, "p.o. Dyrektora ZTM", null, "Official", 0 },
                    { 13, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2480), null, "Danuta", true, "Jarzembowska", "danuta.jarzembowska@gdanskiewodociagi.pl", "zaq1@WSX", null, "Wiceprezes Zarządu", null, "Official", 0 }
                });

            migrationBuilder.InsertData(
                table: "public_body_departments",
                columns: new[] { "id", "description", "executive_id", "name", "public_body_id" },
                values: new object[,]
                {
                    { 1, "Zgłoszenia dotyczące lokalnych zagrożeń, niebezpiecznych znalezisk, dewastacji mienia publicznego oraz klęsk żywiołowych.", 5, "Wydział Bezpieczeństwa i Zarządzania Kryzysowego", 1 },
                    { 2, "Zgłoszenia dotyczące dziur w jezdni, uszkodzonych chodników, niedziałającej sygnalizacji świetlnej oraz brakujących lub zniszczonych znaków drogowych.", 6, "Gdański Zarząd Dróg", 1 },
                    { 3, "Zgłoszenia dotyczące powalonych drzew, połamanych gałęzi, niekoszonych trawników, uszkodzeń w parkach oraz zniszczonej małej architektury.", 7, "Gdański Zarząd Zieleni", 1 },
                    { 4, "Zgłoszenia dotyczące zanieczyszczenia powietrza (smog, spalanie śmieci), nielegalnych zrzutów ścieków do rzek oraz martwych zwierząt.", 8, "Wydział Ekologii i Energetyki", 1 },
                    { 5, "Zgłoszenia dotyczące dzikich wysypisk śmieci, przepełnionych śmietników, zanieczyszczenia ulic oraz problemów z wywozem odpadów.", 9, "Wydział Gospodarki Komunalnej", 1 },
                    { 6, "Zgłoszenia dotyczące awarii oświetlenia ulicznego, uszkodzonych barierek ochronnych oraz większych usterek infrastruktury miejskiej.", 10, "Wydział Infrastruktury", 1 },
                    { 7, "Zgłoszenia dotyczące nieprawidłowego parkowania, wraków pojazdów, zakłócania porządku publicznego oraz spożywania alkoholu w miejscach niedozwolonych.", 11, "Straż Miejska", 1 },
                    { 8, "Zgłoszenia dotyczące zniszczonych wiat przystankowych, rozbitych szyb, niedziałających biletomatów oraz uszkodzonych tablic informacji pasażerskiej.", 12, "Zarząd Transportu Miejskiego", 1 },
                    { 9, "Zgłoszenia dotyczące zapadniętych studzienek, brakujących włazów, awarii rur wodociągowych oraz podtopień infrastruktury drogowej.", 13, "Gdańskie Wodociągi", 1 }
                });

            migrationBuilder.InsertData(
                table: "tickets",
                columns: new[] { "id", "assigned_contractor_id", "assigned_official_id", "building_number", "city", "creation_timestamp", "creator_id", "current_status", "current_status_timestamp", "description", "district", "flat_number", "latitude", "longitude", "note", "photo_url", "postcode", "priority", "public_body_department_id", "street", "title" },
                values: new object[,]
                {
                    { 1, null, null, "120", "Gdańsk", new DateTime(2026, 4, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, "New", new DateTime(2026, 4, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Na prawym pasie znajduje się głęboka wyrwa. Można uszkodzić zawieszenie.", "Wrzeszcz", null, 54.378900m, 18.608700m, null, "https://example.com/images/ticket1_a.jpg", "80-244", "High", 2, "Grunwaldzka", "Głęboka dziura w jezdni" },
                    { 5, null, null, "10", "Gdańsk", new DateTime(2026, 4, 6, 17, 45, 0, 0, DateTimeKind.Utc), 1, "New", new DateTime(2026, 4, 6, 17, 45, 0, 0, DateTimeKind.Utc), "Stoi tu od pół roku, zajmuje miejsce, wyciekają z niego płyny.", "Przymorze", null, 54.398100m, 18.589100m, null, "https://example.com/images/ticket5_wrak.jpg", "80-398", "Low", 7, "Obrońców Wybrzeża", "Porzucone auto bez tablic" },
                    { 8, null, null, "312", "Gdańsk", new DateTime(2026, 4, 7, 11, 20, 0, 0, DateTimeKind.Utc), 4, "New", new DateTime(2026, 4, 7, 11, 20, 0, 0, DateTimeKind.Utc), "Na skraju lasu leży sterta worków z gruzem i stary sedes.", "Jasień", null, 54.358100m, 18.530100m, null, "https://example.com/images/ticket8_gruz.jpg", "80-125", "Normal", 5, "Kartuska", "Ktoś wyrzucił gruz do lasu" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "creation_timestamp", "employer", "first_name", "is_active", "last_name", "mail", "password", "phone_number", "position", "public_body_department_id", "role", "trust_score" },
                values: new object[,]
                {
                    { 14, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2482), "Miasto Gdańsk", "Jan", true, "Kowalski", "jan.kowalski@mycity.pl", "zaq1@WSX", null, "", 1, "Contractor", 0 },
                    { 15, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2484), "DrogBud Sp. z o.o.", "Piotr", true, "Nowak", "p.nowak@drogbud.pl", "zaq1@WSX", null, "", 2, "Contractor", 0 },
                    { 16, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2486), "Asfalt-Max", "Michał", true, "Wiśniewski", "m.wisniewski@asfalt-max.pl", "zaq1@WSX", null, "", 2, "Contractor", 0 },
                    { 17, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2488), "Eko-Zieleń", "Adam", true, "Wójcik", "a.wojcik@eko-zielen.pl", "zaq1@WSX", null, "", 3, "Contractor", 0 },
                    { 18, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2490), "Parki i Ogrody S.A.", "Tomasz", true, "Kowalczyk", "t.kowalczyk@parkiiogrody.pl", "zaq1@WSX", null, "", 3, "Contractor", 0 },
                    { 19, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2492), "Drzew-Serwis", "Krzysztof", true, "Kamiński", "k.kaminski@drzew-serwis.pl", "zaq1@WSX", null, "", 3, "Contractor", 0 },
                    { 20, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2494), "Eco-Tech", "Maciej", true, "Lewandowski", "m.lewandowski@eco-tech.pl", "zaq1@WSX", null, "", 4, "Contractor", 0 },
                    { 21, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2496), "Błysk-Trans", "Jakub", true, "Zieliński", "j.zielinski@blysk-trans.pl", "zaq1@WSX", null, "", 5, "Contractor", 0 },
                    { 22, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2549), "Czyste Miasto", "Szymon", true, "Szymański", "s.szymanski@czystemiasto.pl", "zaq1@WSX", null, "", 5, "Contractor", 0 },
                    { 23, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2551), "InfraBud", "Dawid", true, "Woźniak", "d.wozniak@infrabud.pl", "zaq1@WSX", null, "", 6, "Contractor", 0 },
                    { 24, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2553), "Miasto Gdańsk", "Kacper", true, "Dąbrowski", "k.dabrowski@mycity.pl", "zaq1@WSX", null, "", 7, "Contractor", 0 },
                    { 25, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2555), "Przystanek-Serwis", "Filip", true, "Kozłowski", "f.kozlowski@przystanek-serwis.pl", "zaq1@WSX", null, "", 8, "Contractor", 0 },
                    { 26, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2557), "Aqua-Bud", "Mateusz", true, "Jankowski", "m.jankowski@aqua-bud.pl", "zaq1@WSX", null, "", 9, "Contractor", 0 },
                    { 27, new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2559), "Hydro-Naprawa", "Łukasz", true, "Mazur", "l.mazur@hydro-naprawa.pl", "zaq1@WSX", null, "", 9, "Contractor", 0 }
                });

            migrationBuilder.InsertData(
                table: "tickets",
                columns: new[] { "id", "assigned_contractor_id", "assigned_official_id", "building_number", "city", "creation_timestamp", "creator_id", "current_status", "current_status_timestamp", "description", "district", "flat_number", "latitude", "longitude", "note", "photo_url", "postcode", "priority", "public_body_department_id", "street", "title" },
                values: new object[,]
                {
                    { 2, 17, 7, "3", "Gdańsk", new DateTime(2026, 4, 2, 8, 30, 0, 0, DateTimeKind.Utc), 2, "InProgress", new DateTime(2026, 4, 2, 10, 15, 0, 0, DateTimeKind.Utc), "Po wczorajszej wichurze drzewo całkowicie blokuje ścieżkę rowerową.", "Zaspa", null, 54.409800m, 18.601200m, null, "https://example.com/images/ticket2.jpg", "80-462", "High", 3, "Jana Pawła II", "Powalone drzewo na ścieżce" },
                    { 3, 25, 12, "1", "Gdańsk", new DateTime(2026, 3, 28, 14, 20, 0, 0, DateTimeKind.Utc), 3, "Resolved", new DateTime(2026, 3, 30, 16, 0, 0, 0, DateTimeKind.Utc), "Ktoś rozbił ekran, nie da się kupić biletu.", "Śródmieście", null, 54.351200m, 18.646500m, "Wszędzie leży szkło, uważajcie", "https://example.com/images/ticket3.jpg", "80-895", "Normal", 8, "Podwale Grodzkie", "Zniszczony ekran biletomatu" },
                    { 4, 26, 13, "15", "Gdańsk", new DateTime(2026, 4, 5, 9, 10, 0, 0, DateTimeKind.Utc), 4, "InProgress", new DateTime(2026, 4, 5, 11, 20, 0, 0, DateTimeKind.Utc), "Woda zalewa chodnik i śmierdzi.", "Chełm", null, 54.346800m, 18.621400m, null, "https://example.com/images/ticket4.jpg", "80-805", "Critical", 9, "Cienista", "Wybiła studzienka" },
                    { 6, 23, 10, "18", "Gdańsk", new DateTime(2026, 3, 20, 20, 10, 0, 0, DateTimeKind.Utc), 2, "Resolved", new DateTime(2026, 3, 22, 12, 30, 0, 0, DateTimeKind.Utc), "Nie pali się latarnia bezpośrednio nad pasami, jest niebezpiecznie.", "Stogi", null, 54.341500m, 18.660100m, null, null, "80-620", "Normal", 6, "Nowotna", "Ciemno na przejściu dla pieszych" },
                    { 7, 21, 9, "24", "Gdańsk", new DateTime(2026, 4, 4, 13, 0, 0, 0, DateTimeKind.Utc), 3, "InProgress", new DateTime(2026, 4, 5, 8, 0, 0, 0, DateTimeKind.Utc), "Pojemniki pełne od 3 dni, ptaki roznoszą śmieci po parku.", "Oliwa", null, 54.415200m, 18.571400m, null, "https://example.com/images/ticket7_smietnik.jpg", "80-320", "Low", 5, "Opata Rybińskiego", "Śmieci wysypują się na wiatr" },
                    { 9, 14, 5, "5", "Gdańsk", new DateTime(2026, 3, 25, 15, 0, 0, 0, DateTimeKind.Utc), 1, "Resolved", new DateTime(2026, 3, 25, 18, 45, 0, 0, DateTimeKind.Utc), "Wielkie gniazdo na drzewie zaraz obok zjeżdżalni.", "Orunia", null, 54.332100m, 18.614500m, "Uwaga dla służb: Szerszenie są wyjątkowo agresywne, wygląda na to, że ktoś rzucał w nie kamieniami!!!", "https://example.com/images/ticket9_osy.jpg", "80-032", "Critical", 1, "Gościnna", "Gniazdo szerszeni przy placu zabaw" },
                    { 10, 16, 6, "1", "Gdańsk", new DateTime(2026, 4, 6, 9, 30, 0, 0, DateTimeKind.Utc), 2, "InProgress", new DateTime(2026, 4, 7, 10, 0, 0, 0, DateTimeKind.Utc), "Znak obrócił się na wietrze, kierowcy go nie widzą.", "Śródmieście", null, 54.352800m, 18.640100m, null, "https://example.com/images/ticket10_znak.jpg", "80-886", "High", 2, "Targ Drzewny", "Znak STOP obrócony w złą stronę" }
                });

            migrationBuilder.InsertData(
                table: "status_logs",
                columns: new[] { "id", "comment", "creator_id", "ticket_id", "timestamp", "title" },
                values: new object[,]
                {
                    { 1, "Skierowano firmę Eko-Zieleń do usunięcia drzewa.", 7, 2, new DateTime(2026, 4, 2, 10, 15, 0, 0, DateTimeKind.Utc), "InProgress" },
                    { 2, "Wysłano serwis.", 12, 3, new DateTime(2026, 3, 29, 9, 0, 0, 0, DateTimeKind.Utc), "InProgress" },
                    { 3, "Matryca została wymieniona. Biletomat sprawny.", 25, 3, new DateTime(2026, 3, 30, 16, 0, 0, 0, DateTimeKind.Utc), "Resolved" },
                    { 4, "Ekipa Wodociągów jest w drodze.", 13, 4, new DateTime(2026, 4, 5, 11, 20, 0, 0, DateTimeKind.Utc), "InProgress" },
                    { 5, "Zlecono wymianę lampy.", 10, 6, new DateTime(2026, 3, 21, 8, 15, 0, 0, DateTimeKind.Utc), "InProgress" },
                    { 6, "Latarnia znowu świeci.", 23, 6, new DateTime(2026, 3, 22, 12, 30, 0, 0, DateTimeKind.Utc), "Resolved" },
                    { 7, "Zgłoszono do natychmiastowego wywozu pozaharmonogramowego.", 9, 7, new DateTime(2026, 4, 5, 8, 0, 0, 0, DateTimeKind.Utc), "InProgress" },
                    { 8, "Skierowano straż pożarną.", 5, 9, new DateTime(2026, 3, 25, 15, 30, 0, 0, DateTimeKind.Utc), "InProgress" },
                    { 9, "Teren zabezpieczony, zagrożenie zneutralizowane.", 5, 9, new DateTime(2026, 3, 25, 18, 45, 0, 0, DateTimeKind.Utc), "Resolved" },
                    { 10, "Ekipa DrogBud została powiadomiona.", 6, 10, new DateTime(2026, 4, 7, 10, 0, 0, 0, DateTimeKind.Utc), "InProgress" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_public_body_departments_executive_id",
                table: "public_body_departments",
                column: "executive_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_public_body_departments_public_body_id",
                table: "public_body_departments",
                column: "public_body_id");

            migrationBuilder.CreateIndex(
                name: "ix_status_logs_creator_id",
                table: "status_logs",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_status_logs_ticket_id",
                table: "status_logs",
                column: "ticket_id");

            migrationBuilder.CreateIndex(
                name: "ix_tickets_assigned_contractor_id",
                table: "tickets",
                column: "assigned_contractor_id");

            migrationBuilder.CreateIndex(
                name: "ix_tickets_assigned_official_id",
                table: "tickets",
                column: "assigned_official_id");

            migrationBuilder.CreateIndex(
                name: "ix_tickets_creator_id",
                table: "tickets",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_tickets_public_body_department_id",
                table: "tickets",
                column: "public_body_department_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_public_body_department_id",
                table: "users",
                column: "public_body_department_id");

            migrationBuilder.AddForeignKey(
                name: "fk_public_body_departments_users_executive_id",
                table: "public_body_departments",
                column: "executive_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_public_body_departments_public_bodies_public_body_id",
                table: "public_body_departments");

            migrationBuilder.DropForeignKey(
                name: "fk_public_body_departments_users_executive_id",
                table: "public_body_departments");

            migrationBuilder.DropTable(
                name: "status_logs");

            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "public_bodies");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "public_body_departments");
        }
    }
}
