using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class satunamepass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SatPass",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SatUname",
                table: "Accounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SatPass",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "SatUname",
                table: "Accounts");
        }
    }
}
