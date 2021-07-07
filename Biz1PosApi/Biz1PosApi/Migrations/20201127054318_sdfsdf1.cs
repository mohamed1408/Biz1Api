using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class sdfsdf1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEditable",
                table: "KOTGroups",
                nullable: false,
                defaultValue: false);

        //    migrationBuilder.AddColumn<string>(
        //        name: "bizid",
        //        table: "Accounts",
        //        nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEditable",
                table: "KOTGroups");

        //    migrationBuilder.DropColumn(
        //        name: "bizid",
        //        table: "Accounts");
        }
    }
}
