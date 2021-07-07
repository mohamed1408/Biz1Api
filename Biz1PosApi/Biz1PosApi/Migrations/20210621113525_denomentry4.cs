using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class denomentry4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CashInJson",
                table: "DenomEntries",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CashOutJson",
                table: "DenomEntries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CashInJson",
                table: "DenomEntries");

            migrationBuilder.DropColumn(
                name: "CashOutJson",
                table: "DenomEntries");
        }
    }
}
