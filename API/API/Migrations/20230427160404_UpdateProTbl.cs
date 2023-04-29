using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class UpdateProTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PromotionId",
                table: "Promotions");

            migrationBuilder.AddColumn<int>(
                name: "DisCount",
                table: "Promotions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "Promotions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Point",
                table: "Promotions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "Promotions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisCount",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "Point",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "Promotions");

            migrationBuilder.AddColumn<string>(
                name: "PromotionId",
                table: "Promotions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
