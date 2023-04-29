using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class AddColTiketVsCar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardId",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "CarOutTime",
                table: "Cars",
                newName: "CarTimeOut");

            migrationBuilder.RenameColumn(
                name: "CarInTime",
                table: "Cars",
                newName: "CarTimeIn");

            migrationBuilder.AddColumn<string>(
                name: "ImgCarIn",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgCarOut",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeCard",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TicketMonthlys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<int>(type: "int", nullable: false),
                    LicensePlate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerImgage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    CustomerPoint = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketMonthlys", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketMonthlys");

            migrationBuilder.DropColumn(
                name: "ImgCarIn",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "ImgCarOut",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "TypeCard",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "CarTimeOut",
                table: "Cars",
                newName: "CarOutTime");

            migrationBuilder.RenameColumn(
                name: "CarTimeIn",
                table: "Cars",
                newName: "CarInTime");

            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
