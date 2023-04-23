using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class AddColIsCartblCar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsCarParking",
                table: "Cars",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCarParking",
                table: "Cars");
        }
    }
}
