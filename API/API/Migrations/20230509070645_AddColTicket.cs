using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class AddColTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PromotionId",
                table: "TicketMonthlys",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastRegisterDate",
                table: "TicketMonthlys",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PromotionId",
                table: "TicketMonthlys");

            migrationBuilder.DropColumn(
                name: "LastRegisterDate",
                table: "TicketMonthlys");
        }
    }
}
