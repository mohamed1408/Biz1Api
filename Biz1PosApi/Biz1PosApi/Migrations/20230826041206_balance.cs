using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class balance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "OrderLogId",
            //    table: "OrderLogs",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateTable(
            //    name: "Odrs",
            //    columns: table => new
            //    {
            //        OdrsId = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Id = table.Column<int>(nullable: false),
            //        on = table.Column<int>(nullable: true),
            //        ino = table.Column<string>(nullable: true),
            //        cr = table.Column<string>(nullable: true),
            //        soi = table.Column<int>(nullable: true),
            //        aoi = table.Column<string>(nullable: true),
            //        upoi = table.Column<long>(nullable: true),
            //        si = table.Column<int>(nullable: true),
            //        dsi = table.Column<int>(nullable: true),
            //        cui = table.Column<int>(nullable: true),
            //        cai = table.Column<int>(nullable: true),
            //        dri = table.Column<int>(nullable: true),
            //        osi = table.Column<int>(nullable: true),
            //        psi = table.Column<int>(nullable: true),
            //        ba = table.Column<double>(nullable: true),
            //        ta = table.Column<double>(nullable: true),
            //        pa = table.Column<double>(nullable: true),
            //        ra = table.Column<double>(nullable: true),
            //        s = table.Column<string>(nullable: true),
            //        to = table.Column<double>(nullable: true),
            //        tt = table.Column<double>(nullable: true),
            //        tth = table.Column<double>(nullable: true),
            //        bsi = table.Column<int>(nullable: true),
            //        sti = table.Column<int>(nullable: true),
            //        dp = table.Column<double>(nullable: true),
            //        da = table.Column<double>(nullable: true),
            //        isao = table.Column<bool>(nullable: true),
            //        cud = table.Column<string>(nullable: true),
            //        dti = table.Column<int>(nullable: true),
            //        wi = table.Column<int>(nullable: true),
            //        oddt = table.Column<DateTime>(nullable: false),
            //        od = table.Column<DateTime>(type: "Date", nullable: false),
            //        did = table.Column<DateTime>(type: "Date", nullable: true),
            //        ddd = table.Column<DateTime>(type: "Date", nullable: true),
            //        didt = table.Column<DateTime>(nullable: true),
            //        dddt = table.Column<DateTime>(nullable: true),
            //        n = table.Column<string>(nullable: true),
            //        osd = table.Column<string>(nullable: true),
            //        rsd = table.Column<string>(nullable: true),
            //        fr = table.Column<bool>(nullable: true),
            //        c = table.Column<bool>(nullable: true),
            //        isp = table.Column<bool>(nullable: true),
            //        oj = table.Column<string>(nullable: true),
            //        ij = table.Column<string>(nullable: true),
            //        cj = table.Column<string>(nullable: true),
            //        cgesb = table.Column<double>(nullable: true),
            //        odb = table.Column<double>(nullable: true),
            //        otadb = table.Column<double>(nullable: true),
            //        otodb = table.Column<double>(nullable: true),
            //        aidb = table.Column<double>(nullable: true),
            //        aitadb = table.Column<double>(nullable: true),
            //        aitodb = table.Column<double>(nullable: true),
            //        cts = table.Column<long>(nullable: true),
            //        md = table.Column<DateTime>(nullable: false),
            //        ui = table.Column<int>(nullable: true),
            //        ci = table.Column<int>(nullable: true),
            //        oti = table.Column<int>(nullable: true),
            //        inoj = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Odrs", x => x.OdrsId);
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Odrs");

            //migrationBuilder.DropColumn(
            //    name: "OrderLogId",
            //    table: "OrderLogs");
        }
    }
}
