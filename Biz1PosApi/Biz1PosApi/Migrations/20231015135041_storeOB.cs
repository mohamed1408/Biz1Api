using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class storeOB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_UPProducts",
            //    table: "UPProducts");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_UPLogs",
            //    table: "UPLogs");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "UPProducts",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddColumn<int>(
            //    name: "UPProductId",
            //    table: "UPProducts",
            //    nullable: false,
            //    defaultValue: 0)
            //    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "UPLogs",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddColumn<int>(
            //    name: "UPLogId",
            //    table: "UPLogs",
            //    nullable: false,
            //    defaultValue: 0)
            //    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddColumn<double>(
            //    name: "OpeningBalance",
            //    table: "Stores",
            //    nullable: true);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_UPProducts",
            //    table: "UPProducts",
            //    column: "UPProductId");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_UPLogs",
            //    table: "UPLogs",
            //    column: "UPLogId");

            //migrationBuilder.CreateTable(
            //    name: "StoreOB",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        StoreId = table.Column<int>(nullable: false),
            //        OrderedDateTime = table.Column<DateTime>(nullable: false)
            //    }
            //    //constraints: table =>
            //    //{
            //    //    table.PrimaryKey("PK_StoreOB", x => x.Id);
            //    //    table.ForeignKey(
            //    //        name: "FK_StoreOB_Stores_StoreId",
            //    //        column: x => x.StoreId,
            //    //        principalTable: "Stores",
            //    //        principalColumn: "Id",
            //    //        onDelete: ReferentialAction.Cascade);
            //    //}
            //    );

            //migrationBuilder.CreateTable(
            //    name: "UserAccounts",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Name = table.Column<string>(nullable: true),
            //        Email = table.Column<string>(nullable: true),
            //        Password = table.Column<string>(nullable: true),
            //        PhoneNo = table.Column<string>(nullable: true),
            //        UPUsername = table.Column<string>(nullable: true),
            //        UPAPIKey = table.Column<string>(nullable: true),
            //        SatUname = table.Column<string>(nullable: true),
            //        SatPass = table.Column<string>(nullable: true),
            //        FCM_Token = table.Column<string>(nullable: true),
            //        bizid = table.Column<string>(nullable: true),
            //        IsConfirmed = table.Column<bool>(nullable: false),
            //        CompanyId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_UserAccounts", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_UserAccounts_Companies_CompanyId",
            //            column: x => x.CompanyId,
            //            principalTable: "Companies",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_StoreOB_StoreId",
            //    table: "StoreOB",
            //    column: "StoreId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_UserAccounts_CompanyId",
            //    table: "UserAccounts",
            //    column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreOB");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UPProducts",
                table: "UPProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UPLogs",
                table: "UPLogs");

            migrationBuilder.DropColumn(
                name: "UPProductId",
                table: "UPProducts");

            migrationBuilder.DropColumn(
                name: "UPLogId",
                table: "UPLogs");

            migrationBuilder.DropColumn(
                name: "OpeningBalance",
                table: "Stores");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UPProducts",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UPLogs",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UPProducts",
                table: "UPProducts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UPLogs",
                table: "UPLogs",
                column: "Id");
        }
    }
}
