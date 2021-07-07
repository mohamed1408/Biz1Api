using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class ppqq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Amazon",
                table: "UrbanPiperStores",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAmazon",
                table: "UrbanPiperStores",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amazon",
                table: "UrbanPiperStores");

            migrationBuilder.DropColumn(
                name: "IsAmazon",
                table: "UrbanPiperStores");
        }
    }
}
