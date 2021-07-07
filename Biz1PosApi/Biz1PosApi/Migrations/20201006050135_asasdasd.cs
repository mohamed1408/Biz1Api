using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class asasdasd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerAddressId",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "iscurrentaddress",
                table: "CustomerAddresses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerAddressId",
                table: "Orders",
                column: "CustomerAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CustomerAddresses_CustomerAddressId",
                table: "Orders",
                column: "CustomerAddressId",
                principalTable: "CustomerAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CustomerAddresses_CustomerAddressId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerAddressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomerAddressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "iscurrentaddress",
                table: "CustomerAddresses");
        }
    }
}
