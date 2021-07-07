using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class denomentry3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "Denominations",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "TotalAmount",
                table: "DenomEntries",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<double>(
                name: "CashIn",
                table: "DenomEntries",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CashOut",
                table: "DenomEntries",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ExpectedBalance",
                table: "DenomEntries",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OpeningBalance",
                table: "DenomEntries",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CashIn",
                table: "DenomEntries");

            migrationBuilder.DropColumn(
                name: "CashOut",
                table: "DenomEntries");

            migrationBuilder.DropColumn(
                name: "ExpectedBalance",
                table: "DenomEntries");

            migrationBuilder.DropColumn(
                name: "OpeningBalance",
                table: "DenomEntries");

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "Denominations",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "TotalAmount",
                table: "DenomEntries",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
