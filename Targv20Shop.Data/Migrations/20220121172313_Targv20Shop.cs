using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Targv20Shop.Data.Migrations
{
    public partial class Targv20Shop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CarId",
                table: "FileToDatabase",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mass = table.Column<double>(type: "float", nullable: false),
                    Prize = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Crew = table.Column<int>(type: "int", nullable: false),
                    ConstructedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "FileToDatabase");
        }
    }
}
