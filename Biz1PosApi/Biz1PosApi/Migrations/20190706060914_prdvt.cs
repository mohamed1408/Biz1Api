using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class prdvt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "DeliveryPrice",
                table: "ProductVariants",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "TakeawayPrice",
                table: "ProductVariants",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryPrice",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "TakeawayPrice",
                table: "ProductVariants");
        }
    }
}
