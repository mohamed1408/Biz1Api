using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class stores_saexcash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ExpenseCash",
                table: "Stores",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SalesCash",
                table: "Stores",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpenseCash",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "SalesCash",
                table: "Stores");
        }
    }
}
