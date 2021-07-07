using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class dfjhjilfdsg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AdminOnlyCancel",
                table: "Preferences",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AdvanceOrder",
                table: "Preferences",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Delivery",
                table: "Preferences",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DineIn",
                table: "Preferences",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OnlineOrder",
                table: "Preferences",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TakeAway",
                table: "Preferences",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminOnlyCancel",
                table: "Preferences");

            migrationBuilder.DropColumn(
                name: "AdvanceOrder",
                table: "Preferences");

            migrationBuilder.DropColumn(
                name: "Delivery",
                table: "Preferences");

            migrationBuilder.DropColumn(
                name: "DineIn",
                table: "Preferences");

            migrationBuilder.DropColumn(
                name: "OnlineOrder",
                table: "Preferences");

            migrationBuilder.DropColumn(
                name: "TakeAway",
                table: "Preferences");
        }
    }
}
