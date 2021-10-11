using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class optionqtygroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OptionQtyGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Factor = table.Column<double>(nullable: true),
                    SortOrder = table.Column<int>(nullable: true),
                    IsOnline = table.Column<bool>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    OptionId = table.Column<int>(nullable: false),
                    QuantityGroupId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionQtyGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionQtyGroups_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OptionQtyGroups_Options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Options",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OptionQtyGroups_QuantityGroups_QuantityGroupId",
                        column: x => x.QuantityGroupId,
                        principalTable: "QuantityGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OptionQtyGroups_CompanyId",
                table: "OptionQtyGroups",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionQtyGroups_OptionId",
                table: "OptionQtyGroups",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionQtyGroups_QuantityGroupId",
                table: "OptionQtyGroups",
                column: "QuantityGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OptionQtyGroups");
        }
    }
}
