using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class storeOB2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_DenomEntries_DenomEntries_EditedForId",
            //    table: "DenomEntries");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Denominations_DenomEntries_DenomEntryId",
            //    table: "Denominations");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Denominations",
            //    table: "Denominations");

            //migrationBuilder.DropIndex(
            //    name: "IX_Denominations_DenomEntryId",
            //    table: "Denominations");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_DenomEntries",
            //    table: "DenomEntries");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "Denominations",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddColumn<int>(
            //    name: "DenominationId",
            //    table: "Denominations",
            //    nullable: false,
            //    defaultValue: 0)
            //    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "DenomEntries",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddColumn<int>(
            //    name: "DenomEntryId",
            //    table: "DenomEntries",
            //    nullable: false,
            //    defaultValue: 0)
            //    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Denominations",
            //    table: "Denominations",
            //    column: "DenominationId");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_DenomEntries",
            //    table: "DenomEntries",
            //    column: "DenomEntryId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_DenomEntries_DenomEntries_EditedForId",
            //    table: "DenomEntries",
            //    column: "EditedForId",
            //    principalTable: "DenomEntries",
            //    principalColumn: "DenomEntryId",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_DenomEntries_DenomEntries_EditedForId",
            //    table: "DenomEntries");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Denominations",
            //    table: "Denominations");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_DenomEntries",
            //    table: "DenomEntries");

            //migrationBuilder.DropColumn(
            //    name: "DenominationId",
            //    table: "Denominations");

            //migrationBuilder.DropColumn(
            //    name: "DenomEntryId",
            //    table: "DenomEntries");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "Denominations",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "DenomEntries",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Denominations",
            //    table: "Denominations",
            //    column: "Id");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_DenomEntries",
            //    table: "DenomEntries",
            //    column: "Id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Denominations_DenomEntryId",
            //    table: "Denominations",
            //    column: "DenomEntryId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_DenomEntries_DenomEntries_EditedForId",
            //    table: "DenomEntries",
            //    column: "EditedForId",
            //    principalTable: "DenomEntries",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Denominations_DenomEntries_DenomEntryId",
            //    table: "Denominations",
            //    column: "DenomEntryId",
            //    principalTable: "DenomEntries",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
