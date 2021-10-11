using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class predefinedqty_product_bool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsQtyPredefined",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PredefinedQuantities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    QuantityText = table.Column<string>(nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    FreeQuantity = table.Column<double>(nullable: false),
                    TotalQuantity = table.Column<double>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredefinedQuantities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PredefinedQuantities_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PredefinedQuantities_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PredefinedQuantities_CompanyId",
                table: "PredefinedQuantities",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PredefinedQuantities_ProductId",
                table: "PredefinedQuantities",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PredefinedQuantities");

            migrationBuilder.DropColumn(
                name: "IsQtyPredefined",
                table: "Products");
        }
    }
}
