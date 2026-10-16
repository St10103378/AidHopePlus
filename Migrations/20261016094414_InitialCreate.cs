using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AidHopePlus.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AwarenessEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AwarenessEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Beneficiaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BeneficiaryCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateRegistered = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beneficiaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AwarenessEventId = table.Column<int>(type: "int", nullable: true),
                    BeneficiaryId = table.Column<int>(type: "int", nullable: true),
                    AttendeeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AttendanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_AwarenessEvents_AwarenessEventId",
                        column: x => x.AwarenessEventId,
                        principalTable: "AwarenessEvents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_Beneficiaries_BeneficiaryId",
                        column: x => x.BeneficiaryId,
                        principalTable: "Beneficiaries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SupportSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BeneficiaryId = table.Column<int>(type: "int", nullable: false),
                    SupportType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SessionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FollowUpNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportSessions_Beneficiaries_BeneficiaryId",
                        column: x => x.BeneficiaryId,
                        principalTable: "Beneficiaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AwarenessEvents",
                columns: new[] { "Id", "CreatedAt", "Description", "EventDate", "EventName", "Location" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8259), null, new DateTime(2026, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Support Group Meeting", "Clinic" },
                    { 2, new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8262), null, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Health Education Day", "Town Hall" },
                    { 3, new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8263), null, new DateTime(2026, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Youth Workshop", "2BC Centre" },
                    { 4, new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8264), null, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "HIV Awareness Talk", "Community Hall" }
                });

            migrationBuilder.InsertData(
                table: "Beneficiaries",
                columns: new[] { "Id", "BeneficiaryCode", "ContactNumber", "DateRegistered", "FullName", "Notes", "Status" },
                values: new object[,]
                {
                    { 1, "B001", "071 234 5678", new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8195), "Thabo Mokoena", null, "HIV Positive" },
                    { 2, "B002", "082 345 6789", new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8201), "Nomsa Dlamini", null, "Affected" },
                    { 3, "B003", "079 876 5432", new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8202), "Sipho Khumalo", null, "HIV Positive" },
                    { 4, "B004", "083 111 2222", new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8203), "Lerato Nkosi", null, "Affected" },
                    { 5, "B005", "076 555 4444", new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8204), "Anele Zulu", null, "HIV Positive" },
                    { 6, "B006", "0723456789", new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8205), "Thapelo Sehadi", null, "HIV Positive" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FullName", "PasswordHash", "Role", "Username" },
                values: new object[] { 1, "Staff User", "staff123", "Staff", "staff" });

            migrationBuilder.InsertData(
                table: "AttendanceRecords",
                columns: new[] { "Id", "AttendanceDate", "AttendeeName", "AwarenessEventId", "BeneficiaryId", "CreatedAt", "Notes", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thabo Mokoena", 2, 1, new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8296), null, "Present" },
                    { 2, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nomsa Dlamini", 2, 2, new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8301), null, "Present" },
                    { 3, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sipho Khumalo", 2, 3, new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8303), null, "Present" },
                    { 4, new DateTime(2026, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lerato Nkosi", 3, 4, new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8305), null, "Present" },
                    { 5, new DateTime(2026, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Anele Zulu", 3, 5, new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8306), null, "Late" },
                    { 6, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thabo Mokoena", 4, 1, new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8308), null, "Present" },
                    { 7, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thapelo Sehadi", 4, 6, new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8310), null, "Present" },
                    { 8, new DateTime(2026, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nomsa Dlamini", 1, 2, new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8311), "Registered in advance", "Present" }
                });

            migrationBuilder.InsertData(
                table: "SupportSessions",
                columns: new[] { "Id", "BeneficiaryId", "CreatedAt", "FollowUpNotes", "SessionDate", "SupportType" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8228), "Family counselling session held.", new DateTime(2026, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Psychosocial Support" },
                    { 2, 3, new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8233), "Follow up on treatment progress next week.", new DateTime(2026, 10, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Counselling" },
                    { 3, 5, new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8235), "Referred to Mamelodi clinic for check-up.", new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Medical Referral" },
                    { 4, 4, new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8236), "Prayer and encouragement session.", new DateTime(2026, 9, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spiritual Care" },
                    { 5, 1, new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8237), "Doing better. Referred to support group.", new DateTime(2026, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Home Visit" },
                    { 6, 3, new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8238), "Reminded about clinic dates.", new DateTime(2026, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Treatment Adherence Support" },
                    { 7, 2, new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8239), "Parcel delivered to the family.", new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Food Parcel" },
                    { 8, 1, new DateTime(2026, 10, 16, 9, 44, 13, 819, DateTimeKind.Utc).AddTicks(8240), "Discussed coping strategies. Check in again in two weeks.", new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Counselling" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_AwarenessEventId",
                table: "AttendanceRecords",
                column: "AwarenessEventId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_BeneficiaryId",
                table: "AttendanceRecords",
                column: "BeneficiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportSessions_BeneficiaryId",
                table: "SupportSessions",
                column: "BeneficiaryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceRecords");

            migrationBuilder.DropTable(
                name: "SupportSessions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "AwarenessEvents");

            migrationBuilder.DropTable(
                name: "Beneficiaries");
        }
    }
}
