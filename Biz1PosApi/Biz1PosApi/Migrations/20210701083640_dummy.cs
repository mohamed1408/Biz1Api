using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class dummy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<bool>(
            //    name: "IsSaleProdGroup",
            //    table: "Products",
            //    nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "SaleProductGroups",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        SaleProductId = table.Column<int>(nullable: false),
            //        StockProductId = table.Column<int>(nullable: false),
            //        CreatedDate = table.Column<DateTime>(nullable: false),
            //        ModifiedDate = table.Column<DateTime>(nullable: false),
            //        CompanyId = table.Column<int>(nullable: false),
            //        OptionId = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SaleProductGroups", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_SaleProductGroups_Companies_CompanyId",
            //            column: x => x.CompanyId,
            //            principalTable: "Companies",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_SaleProductGroups_Options_OptionId",
            //            column: x => x.OptionId,
            //            principalTable: "Options",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_SaleProductGroups_Products_SaleProductId",
            //            column: x => x.SaleProductId,
            //            principalTable: "Products",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_SaleProductGroups_Products_StockProductId",
            //            column: x => x.StockProductId,
            //            principalTable: "Products",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_SaleProductGroups_CompanyId",
            //    table: "SaleProductGroups",
            //    column: "CompanyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SaleProductGroups_OptionId",
            //    table: "SaleProductGroups",
            //    column: "OptionId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SaleProductGroups_SaleProductId",
            //    table: "SaleProductGroups",
            //    column: "SaleProductId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SaleProductGroups_StockProductId",
            //    table: "SaleProductGroups",
            //    column: "StockProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "SaleProductGroups");

            //migrationBuilder.DropColumn(
            //    name: "IsSaleProdGroup",
            //    table: "Products");
        }
    }
}
