using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class customeraddress_hidden : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "hidden",
                table: "CustomerAddresses",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hidden",
                table: "CustomerAddresses");
        }
    }
}
