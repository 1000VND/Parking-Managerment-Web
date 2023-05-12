using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class removeColTiket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PromotionId",
                table: "TicketMonthlys");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PromotionId",
                table: "TicketMonthlys",
                type: "int",
                nullable: true);
        }
    }
}
