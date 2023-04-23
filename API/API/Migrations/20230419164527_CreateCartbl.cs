using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class CreateCartbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<int>(type: "int", nullable: false),
                    LicensePlateIn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicensePlateOut = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarInTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CarOutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TypeCustomer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cars");
        }
    }
}
