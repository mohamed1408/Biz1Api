using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class sdfsdf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cnditions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VariableTypeId = table.Column<int>(nullable: false),
                    ParentCnditionId = table.Column<int>(nullable: false),
                    ValueId = table.Column<int>(nullable: false),
                    OperatorId = table.Column<int>(nullable: false),
                    JoinOperatorId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cnditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cnditions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    //onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cnditions_Operators_JoinOperatorId",
                        column: x => x.JoinOperatorId,
                        principalTable: "Operators",
                        principalColumn: "Id");
                    //onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cnditions_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "Id");
                    //onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cnditions_Cnditions_ParentCnditionId",
                        column: x => x.ParentCnditionId,
                        principalTable: "Cnditions",
                        principalColumn: "Id");
                    //onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cnditions_VariableTypes_VariableTypeId",
                        column: x => x.VariableTypeId,
                        principalTable: "VariableTypes",
                        principalColumn: "Id");
                    //onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerOffers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: false),
                    OfferId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerOffers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    //onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerOffers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    //onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerOffers_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id");
                    //onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfferRules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OfferId = table.Column<int>(nullable: false),
                    RuleId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfferRules_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    //onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfferRules_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id");
                    //onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cnditions_CompanyId",
                table: "Cnditions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Cnditions_JoinOperatorId",
                table: "Cnditions",
                column: "JoinOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Cnditions_OperatorId",
                table: "Cnditions",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Cnditions_ParentCnditionId",
                table: "Cnditions",
                column: "ParentCnditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Cnditions_VariableTypeId",
                table: "Cnditions",
                column: "VariableTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOffers_CompanyId",
                table: "CustomerOffers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOffers_CustomerId",
                table: "CustomerOffers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOffers_OfferId",
                table: "CustomerOffers",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferRules_CompanyId",
                table: "OfferRules",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferRules_OfferId",
                table: "OfferRules",
                column: "OfferId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cnditions");

            migrationBuilder.DropTable(
                name: "CustomerOffers");

            migrationBuilder.DropTable(
                name: "OfferRules");
        }
    }
}
