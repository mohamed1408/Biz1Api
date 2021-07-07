using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class strprdvrnt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreProductVariants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StoreId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    VariantId = table.Column<int>(nullable: false),
                    Price = table.Column<float>(nullable: true),
                    TakeawayPrice = table.Column<float>(nullable: true),
                    DeliveryPrice = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreProductVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreProductVariants_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_StoreProductVariants_Stores_StoreId",
                    column: x => x.StoreId,
                    principalTable: "Stores",
                    principalColumn: "Id");
                        //onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreProductVariants_Variants_VariantId",
                        column: x => x.VariantId,
                        principalTable: "Variants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreProductVariants_ProductId",
                table: "StoreProductVariants",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProductVariants_StoreId",
                table: "StoreProductVariants",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProductVariants_VariantId",
                table: "StoreProductVariants",
                column: "VariantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreProductVariants");
        }
    }
}
