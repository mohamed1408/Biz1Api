using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class order_date_indexing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "del_Day",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "del_Month",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "del_Quarter",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "del_Week",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "del_Year",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ord_Day",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ord_Month",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ord_Quarter",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ord_Week",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ord_Year",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "del_Day",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "del_Month",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "del_Quarter",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "del_Week",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "del_Year",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ord_Day",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ord_Month",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ord_Quarter",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ord_Week",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ord_Year",
                table: "Orders");
        }
    }
}
