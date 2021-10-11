using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class dummy2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "Factor",
            //    table: "SaleProductGroups",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<int>(
            //    name: "AkountzCompanyId",
            //    table: "Companies",
            //    nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Factor",
            //    table: "SaleProductGroups");

            //migrationBuilder.DropColumn(
            //    name: "AkountzCompanyId",
            //    table: "Companies");
        }
    }
}
