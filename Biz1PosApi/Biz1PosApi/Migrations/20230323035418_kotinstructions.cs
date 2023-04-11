using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class kotinstructions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KOTInstructions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InstructionType = table.Column<int>(nullable: false),
                    url = table.Column<string>(nullable: true),
                    InstructionDateTime = table.Column<DateTime>(nullable: false),
                    KOTId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KOTInstructions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KOTInstructions_KOTs_KOTId",
                        column: x => x.KOTId,
                        principalTable: "KOTs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KOTInstructions_KOTId",
                table: "KOTInstructions",
                column: "KOTId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KOTInstructions");
        }
    }
}