using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class oc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderCharges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<int>(nullable: false),
                    AdditionalChargeId = table.Column<int>(nullable: false),
                    ChargePercentage = table.Column<double>(nullable: false),
                    ChargeAmount = table.Column<double>(nullable: false),
                    Tax1 = table.Column<double>(nullable: false),
                    Tax2 = table.Column<double>(nullable: false),
                    Tax3 = table.Column<double>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    StoreId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderCharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderCharges_AdditionalCharges_AdditionalChargeId",
                        column: x => x.AdditionalChargeId,
                        principalTable: "AdditionalCharges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_OrderCharges_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id");
                //onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_OrderCharges_Orders_OrderId",
                    column: x => x.OrderId,
                    principalTable: "Orders",
                    principalColumn: "Id");
                        //onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_OrderCharges_Stores_StoreId",
                    column: x => x.StoreId,
                    principalTable: "Stores",
                    principalColumn: "Id");
                        //onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderCharges_AdditionalChargeId",
                table: "OrderCharges",
                column: "AdditionalChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCharges_CompanyId",
                table: "OrderCharges",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCharges_OrderId",
                table: "OrderCharges",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCharges_StoreId",
                table: "OrderCharges",
                column: "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderCharges");
        }
    }
}
