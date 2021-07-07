using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class aks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductAddonGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(nullable: false),
                    AddonGroupId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAddonGroups", x => x.Id);
                table.ForeignKey(
                    name: "FK_ProductAddonGroups_AddonGroups_AddonGroupId",
                    column: x => x.AddonGroupId,
                    principalTable: "AddonGroups",
                    principalColumn: "Id");
                //onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ProductAddonGroups_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id");
                //onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ProductAddonGroups_Products_ProductId",
                    column: x => x.ProductId,
                    principalTable: "Products",
                    principalColumn: "Id");
                        //onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAddonGroups_AddonGroupId",
                table: "ProductAddonGroups",
                column: "AddonGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAddonGroups_CompanyId",
                table: "ProductAddonGroups",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAddonGroups_ProductId",
                table: "ProductAddonGroups",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductAddonGroups");
        }
    }
}
