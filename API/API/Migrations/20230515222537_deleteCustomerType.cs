using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class deleteCustomerType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerType",
                table: "Promotions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerType",
                table: "Promotions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
