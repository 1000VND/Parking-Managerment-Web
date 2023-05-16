using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class AddPromotionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerPoint",
                table: "TicketMonthlys");

            migrationBuilder.AddColumn<int>(
                name: "PromotionId",
                table: "TicketMonthlys",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PromotionId",
                table: "TicketMonthlys");

            migrationBuilder.AddColumn<int>(
                name: "CustomerPoint",
                table: "TicketMonthlys",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
