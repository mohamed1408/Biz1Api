using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class customeraddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "CustomerAddresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                table: "CustomerAddresses",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NearByStoreId",
                table: "CustomerAddresses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "CustomerAddresses");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "CustomerAddresses");

            migrationBuilder.DropColumn(
                name: "NearByStoreId",
                table: "CustomerAddresses");
        }
    }
}
