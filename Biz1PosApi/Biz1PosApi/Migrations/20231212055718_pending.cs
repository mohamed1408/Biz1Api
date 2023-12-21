using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class pending : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_StoreProducts",
            //    table: "StoreProducts");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "StoreProducts",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddColumn<int>(
            //    name: "StoreProductId",
            //    table: "StoreProducts",
            //    nullable: false,
            //    defaultValue: 0)
            //    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_StoreProducts",
            //    table: "StoreProducts",
            //    column: "StoreProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StoreProducts",
                table: "StoreProducts");

            migrationBuilder.DropColumn(
                name: "StoreProductId",
                table: "StoreProducts");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "StoreProducts",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoreProducts",
                table: "StoreProducts",
                column: "Id");
        }
    }
}
