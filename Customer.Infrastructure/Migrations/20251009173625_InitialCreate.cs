using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Customer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RetailCustomers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    PersonalId = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    ImageKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetailCustomers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RetailCustomers_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhoneNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IndividualCustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhoneNumbers_RetailCustomers_IndividualCustomerId",
                        column: x => x.IndividualCustomerId,
                        principalTable: "RetailCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Relations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndividualCustomerId = table.Column<int>(type: "int", nullable: false),
                    RelatedCustomerId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Relations_RetailCustomers_IndividualCustomerId",
                        column: x => x.IndividualCustomerId,
                        principalTable: "RetailCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Relations_RetailCustomers_RelatedCustomerId",
                        column: x => x.RelatedCustomerId,
                        principalTable: "RetailCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Country", "Name" },
                values: new object[,]
                {
                    { 1, "Georgia", "Tbilisi" },
                    { 2, "Georgia", "Batumi" },
                    { 3, "Georgia", "Kutaisi" },
                    { 4, "USA", "New York" },
                    { 5, "USA", "San Francisco" },
                    { 6, "Germany", "Munich" },
                    { 7, "Germany", "Berlin" },
                    { 8, "UK", "London" },
                    { 9, "UK", "Manchester" },
                    { 10, "Japan", "Tokyo" },
                    { 11, "Japan", "Osaka" },
                    { 12, "France", "Paris" },
                    { 13, "France", "Lyon" },
                    { 14, "Australia", "Sydney" },
                    { 15, "Australia", "Melbourne" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumbers_IndividualCustomerId",
                table: "PhoneNumbers",
                column: "IndividualCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Relations_IndividualCustomerId",
                table: "Relations",
                column: "IndividualCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Relations_RelatedCustomerId",
                table: "Relations",
                column: "RelatedCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_RetailCustomers_CityId",
                table: "RetailCustomers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_RetailCustomers_PersonalId",
                table: "RetailCustomers",
                column: "PersonalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhoneNumbers");

            migrationBuilder.DropTable(
                name: "Relations");

            migrationBuilder.DropTable(
                name: "RetailCustomers");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
