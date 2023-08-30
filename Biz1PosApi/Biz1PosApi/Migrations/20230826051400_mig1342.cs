using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class mig1342 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "groupid",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "groupid",
                table: "Categories",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MenuMappings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    groupid = table.Column<int>(nullable: false),
                    companyid = table.Column<int>(nullable: false),
                    menutype = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuMappings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OldProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OldId = table.Column<int>(nullable: false),
                    Name = table.Column<int>(nullable: false),
                    Tax = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OldProducts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuMappings");

            migrationBuilder.DropTable(
                name: "OldProducts");

            migrationBuilder.DropColumn(
                name: "groupid",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "groupid",
                table: "Categories");
        }
    }
}
