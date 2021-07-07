using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class sdfsdf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KOTGroupPrinters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Printer = table.Column<string>(nullable: true),
                    KOTGroupId = table.Column<int>(nullable: false),
                    StoreId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KOTGroupPrinters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KOTGroupPrinters_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KOTGroupPrinters_KOTGroups_KOTGroupId",
                        column: x => x.KOTGroupId,
                        principalTable: "KOTGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KOTGroupPrinters_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KOTGroupPrinters_CompanyId",
                table: "KOTGroupPrinters",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_KOTGroupPrinters_KOTGroupId",
                table: "KOTGroupPrinters",
                column: "KOTGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_KOTGroupPrinters_StoreId",
                table: "KOTGroupPrinters",
                column: "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KOTGroupPrinters");
        }
    }
}
