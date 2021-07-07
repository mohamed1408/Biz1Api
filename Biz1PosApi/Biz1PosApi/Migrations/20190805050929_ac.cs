using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class ac : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalCharges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    ChargeType = table.Column<int>(nullable: false),
                    ChargeValue = table.Column<double>(nullable: false),
                    TaxGroupId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    StoreId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalCharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalCharges_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AdditionalCharges_Stores_StoreId",
                    column: x => x.StoreId,
                    principalTable: "Stores",
                    principalColumn: "Id");
                //onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AdditionalCharges_TaxGroups_TaxGroupId",
                    column: x => x.TaxGroupId,
                    principalTable: "TaxGroups",
                    principalColumn: "Id");
                        //onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalCharges_CompanyId",
                table: "AdditionalCharges",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalCharges_StoreId",
                table: "AdditionalCharges",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalCharges_TaxGroupId",
                table: "AdditionalCharges",
                column: "TaxGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalCharges");
        }
    }
}
