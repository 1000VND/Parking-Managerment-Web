using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class ChangeColTypeCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeCustomer",
                table: "Cars");

            migrationBuilder.AlterColumn<int>(
                name: "TypeCard",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TypeCard",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "TypeCustomer",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
