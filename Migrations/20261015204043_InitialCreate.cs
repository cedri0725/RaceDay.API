using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RaceDay.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Category_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Distance_Km = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Category_ID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Role_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Role_ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role_ID = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password_Hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Full_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone_Number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Is_Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.User_ID);
                    table.ForeignKey(
                        name: "FK_Users_Roles_Role_ID",
                        column: x => x.Role_ID,
                        principalTable: "Roles",
                        principalColumn: "Role_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Event_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Organiser_ID = table.Column<int>(type: "int", nullable: false),
                    Event_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Event_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Max_Capacity = table.Column<int>(type: "int", nullable: false),
                    Is_Active = table.Column<bool>(type: "bit", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Event_ID);
                    table.ForeignKey(
                        name: "FK_Events_Users_Organiser_ID",
                        column: x => x.Organiser_ID,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventCategories",
                columns: table => new
                {
                    Event_Category_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Event_ID = table.Column<int>(type: "int", nullable: false),
                    Category_ID = table.Column<int>(type: "int", nullable: false),
                    Entry_Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Start_Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Is_Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCategories", x => x.Event_Category_ID);
                    table.ForeignKey(
                        name: "FK_EventCategories_Categories_Category_ID",
                        column: x => x.Category_ID,
                        principalTable: "Categories",
                        principalColumn: "Category_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventCategories_Events_Event_ID",
                        column: x => x.Event_ID,
                        principalTable: "Events",
                        principalColumn: "Event_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enrolments",
                columns: table => new
                {
                    Enrolment_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Event_Category_ID = table.Column<int>(type: "int", nullable: false),
                    Participant_ID = table.Column<int>(type: "int", nullable: false),
                    Enrolment_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrolments", x => x.Enrolment_ID);
                    table.ForeignKey(
                        name: "FK_Enrolments_EventCategories_Event_Category_ID",
                        column: x => x.Event_Category_ID,
                        principalTable: "EventCategories",
                        principalColumn: "Event_Category_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrolments_Users_Participant_ID",
                        column: x => x.Participant_ID,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Result_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Enrolment_ID = table.Column<int>(type: "int", nullable: false),
                    Finish_Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Recorded_At = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Result_ID);
                    table.ForeignKey(
                        name: "FK_Results_Enrolments_Enrolment_ID",
                        column: x => x.Enrolment_ID,
                        principalTable: "Enrolments",
                        principalColumn: "Enrolment_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Category_ID", "Category_Name", "Description", "Distance_Km" },
                values: new object[,]
                {
                    { 1, "5km Fun Run", "Family-friendly 5 kilometre run/walk", 5.00m },
                    { 2, "10km Road Race", "Challenging 10 kilometre road race", 10.00m },
                    { 3, "21km Half Marathon", "Competitive half marathon distance", 21.10m },
                    { 4, "42km Marathon", "Full marathon distance", 42.20m },
                    { 5, "50km Cycle", "Road cycling event - 50km", 50.00m },
                    { 6, "100km Cycle", "Long distance cycling event - 100km", 100.00m }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Role_ID", "Description", "Role_Name" },
                values: new object[,]
                {
                    { 1, "Event organisers who create and manage events", "Organiser" },
                    { 2, "Participants who browse events and enrol in races", "Participant" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Category_Name",
                table: "Categories",
                column: "Category_Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enrolments_Event_Category_ID_Participant_ID",
                table: "Enrolments",
                columns: new[] { "Event_Category_ID", "Participant_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enrolments_Participant_ID",
                table: "Enrolments",
                column: "Participant_ID");

            migrationBuilder.CreateIndex(
                name: "IX_EventCategories_Category_ID",
                table: "EventCategories",
                column: "Category_ID");

            migrationBuilder.CreateIndex(
                name: "IX_EventCategories_Event_ID_Category_ID",
                table: "EventCategories",
                columns: new[] { "Event_ID", "Category_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_Organiser_ID",
                table: "Events",
                column: "Organiser_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Results_Enrolment_ID",
                table: "Results",
                column: "Enrolment_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Role_Name",
                table: "Roles",
                column: "Role_Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Role_ID",
                table: "Users",
                column: "Role_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Enrolments");

            migrationBuilder.DropTable(
                name: "EventCategories");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
