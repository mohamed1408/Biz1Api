using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class odrs_icdr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "dr",
                table: "Odrs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "icr",
                table: "Odrs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dr",
                table: "Odrs");

            migrationBuilder.DropColumn(
                name: "icr",
                table: "Odrs");
        }
    }
}
