using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class DeletePoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Point",
                table: "Promotions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Point",
                table: "Promotions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
