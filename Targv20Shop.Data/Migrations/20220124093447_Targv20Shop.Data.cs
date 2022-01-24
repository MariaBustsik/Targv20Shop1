using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Targv20Shop.Data.Migrations
{
    public partial class Targv20ShopData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarId",
                table: "FileToDatabase");

            migrationBuilder.DropColumn(
                name: "Crew",
                table: "Car");

            migrationBuilder.AddColumn<Guid>(
                name: "CarId",
                table: "ExistingFilePath",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarId",
                table: "ExistingFilePath");

            migrationBuilder.AddColumn<Guid>(
                name: "CarId",
                table: "FileToDatabase",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Crew",
                table: "Car",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
