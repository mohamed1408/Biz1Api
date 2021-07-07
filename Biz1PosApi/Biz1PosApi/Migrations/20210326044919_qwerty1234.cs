using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class qwerty1234 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLogs_Companies_CompanyId",
                table: "OrderLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderLogs_Stores_StoreId",
                table: "OrderLogs");

            migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "OrderLogs",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "OrderLogs",
                nullable: true,
                oldClrType: typeof(int));

            //migrationBuilder.CreateTable(
            //    name: "Tags",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Description = table.Column<string>(nullable: true),
            //        CreatedDate = table.Column<DateTime>(nullable: false),
            //        ModifiedDate = table.Column<DateTime>(nullable: false),
            //        CompanyId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Tags", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Tags_Companies_CompanyId",
            //            column: x => x.CompanyId,
            //            principalTable: "Companies",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TagMappings",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        TagId = table.Column<int>(nullable: false),
            //        ProductId = table.Column<int>(nullable: false),
            //        CreatedDate = table.Column<DateTime>(nullable: false),
            //        ModifiedDate = table.Column<DateTime>(nullable: false),
            //        CompanyId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TagMappings", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_TagMappings_Companies_CompanyId",
            //            column: x => x.CompanyId,
            //            principalTable: "Companies",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_TagMappings_Products_ProductId",
            //            column: x => x.ProductId,
            //            principalTable: "Products",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_TagMappings_Tags_TagId",
            //            column: x => x.TagId,
            //            principalTable: "Tags",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_TagMappings_CompanyId",
            //    table: "TagMappings",
            //    column: "CompanyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TagMappings_ProductId",
            //    table: "TagMappings",
            //    column: "ProductId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TagMappings_TagId",
            //    table: "TagMappings",
            //    column: "TagId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Tags_CompanyId",
            //    table: "Tags",
            //    column: "CompanyId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_OrderLogs_Companies_CompanyId",
            //    table: "OrderLogs",
            //    column: "CompanyId",
            //    principalTable: "Companies",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_OrderLogs_Stores_StoreId",
            //    table: "OrderLogs",
            //    column: "StoreId",
            //    principalTable: "Stores",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLogs_Companies_CompanyId",
                table: "OrderLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderLogs_Stores_StoreId",
                table: "OrderLogs");

            //migrationBuilder.DropTable(
            //    name: "TagMappings");

            //migrationBuilder.DropTable(
            //    name: "Tags");

            migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "OrderLogs",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "OrderLogs",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLogs_Companies_CompanyId",
                table: "OrderLogs",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLogs_Stores_StoreId",
                table: "OrderLogs",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
