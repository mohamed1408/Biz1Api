using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class mig1346 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<string>(
            //    name: "Name",
            //    table: "OldProducts",
            //    nullable: true,
            //    oldClrType: typeof(int));

            //migrationBuilder.AddColumn<int>(
            //    name: "CategoryId",
            //    table: "OldProducts",
            //    nullable: false,
            //    defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "OldProducts");

            migrationBuilder.AlterColumn<int>(
                name: "Name",
                table: "OldProducts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
