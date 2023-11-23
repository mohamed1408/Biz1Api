using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class mmstoreid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_OrderItems_KOTs_KOTId",
            //    table: "OrderItems");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_StoreLogs",
            //    table: "StoreLogs");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_OrderLogs",
            //    table: "OrderLogs");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_KOTs",
            //    table: "KOTs");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "StoreLogs",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddColumn<int>(
            //    name: "StoreLogId",
            //    table: "StoreLogs",
            //    nullable: false,
            //    defaultValue: 0)
            //    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "OrderLogId",
            //    table: "OrderLogs",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "OrderLogs",
            //    nullable: true,
            //    oldClrType: typeof(int))
            //    .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "bd",
            //    table: "Odrs",
            //    type: "Date",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "bdt",
            //    table: "Odrs",
            //    nullable: true);

            //migrationBuilder.AddColumn<TimeSpan>(
            //    name: "odt",
            //    table: "Odrs",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "storeid",
            //    table: "MenuMappings",
            //    nullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "KOTs",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddColumn<int>(
            //    name: "KOTId",
            //    table: "KOTs",
            //    nullable: false,
            //    defaultValue: 0)
            //    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_StoreLogs",
            //    table: "StoreLogs",
            //    column: "StoreLogId");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_OrderLogs",
            //    table: "OrderLogs",
            //    column: "OrderLogId");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_KOTs",
            //    table: "KOTs",
            //    column: "KOTId");

            //migrationBuilder.CreateTable(
            //    name: "Otms",
            //    columns: table => new
            //    {
            //        OtmsId = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Id = table.Column<int>(nullable: false),
            //        qy = table.Column<float>(nullable: true),
            //        pr = table.Column<float>(nullable: true),
            //        ob = table.Column<int>(nullable: true),
            //        pi = table.Column<int>(nullable: true),
            //        to = table.Column<double>(nullable: true),
            //        tt = table.Column<double>(nullable: true),
            //        tth = table.Column<double>(nullable: true),
            //        dp = table.Column<double>(nullable: true),
            //        da = table.Column<double>(nullable: true),
            //        imd = table.Column<double>(nullable: true),
            //        tid = table.Column<double>(nullable: true),
            //        od = table.Column<double>(nullable: true),
            //        tod = table.Column<double>(nullable: true),
            //        sti = table.Column<int>(nullable: true),
            //        kui = table.Column<int>(nullable: true),
            //        ki = table.Column<int>(nullable: true),
            //        n = table.Column<string>(nullable: true),
            //        msg = table.Column<string>(nullable: true),
            //        ta = table.Column<double>(nullable: true),
            //        ext = table.Column<double>(nullable: true),
            //        kri = table.Column<string>(nullable: true),
            //        ri = table.Column<string>(nullable: true),
            //        issu = table.Column<bool>(nullable: true),
            //        cati = table.Column<int>(nullable: true),
            //        oj = table.Column<string>(nullable: true),
            //        cqy = table.Column<float>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Otms", x => x.OtmsId);
            //    });

            //migrationBuilder.AddForeignKey(
            //    name: "FK_OrderItems_KOTs_KOTId",
            //    table: "OrderItems",
            //    column: "KOTId",
            //    principalTable: "KOTs",
            //    principalColumn: "KOTId",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_OrderItems_KOTs_KOTId",
            //    table: "OrderItems");

            //migrationBuilder.DropTable(
            //    name: "Otms");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_StoreLogs",
            //    table: "StoreLogs");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_OrderLogs",
            //    table: "OrderLogs");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_KOTs",
            //    table: "KOTs");

            //migrationBuilder.DropColumn(
            //    name: "StoreLogId",
            //    table: "StoreLogs");

            //migrationBuilder.DropColumn(
            //    name: "bd",
            //    table: "Odrs");

            //migrationBuilder.DropColumn(
            //    name: "bdt",
            //    table: "Odrs");

            //migrationBuilder.DropColumn(
            //    name: "odt",
            //    table: "Odrs");

            //migrationBuilder.DropColumn(
            //    name: "storeid",
            //    table: "MenuMappings");

            //migrationBuilder.DropColumn(
            //    name: "KOTId",
            //    table: "KOTs");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "StoreLogs",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "OrderLogs",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldNullable: true)
            //    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "OrderLogId",
            //    table: "OrderLogs",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "KOTs",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_StoreLogs",
            //    table: "StoreLogs",
            //    column: "Id");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_OrderLogs",
            //    table: "OrderLogs",
            //    column: "Id");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_KOTs",
            //    table: "KOTs",
            //    column: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_OrderItems_KOTs_KOTId",
            //    table: "OrderItems",
            //    column: "KOTId",
            //    principalTable: "KOTs",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}
