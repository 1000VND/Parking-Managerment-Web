using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class addColCostTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "TicketMonthlys",
                type: "decimal(15,0)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "TicketMonthlys");
        }
    }
}
