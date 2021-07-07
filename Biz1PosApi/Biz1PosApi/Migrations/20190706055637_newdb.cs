//using System;
//using Microsoft.EntityFrameworkCore.Metadata;
//using Microsoft.EntityFrameworkCore.Migrations;

//namespace Biz1PosApi.Migrations
//{
//    public partial class newdb : Migration
//    {
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.CreateTable(
//                name: "AddonGroups",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    Description = table.Column<string>(nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AddonGroups", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Companies",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    Name = table.Column<string>(nullable: true),
//                    DisplayName = table.Column<string>(nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Companies", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "DiningAreas",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    Description = table.Column<string>(nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_DiningAreas", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "DropDownGroups",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    Description = table.Column<string>(nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_DropDownGroups", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "PaymentTypes",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    Description = table.Column<string>(nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_PaymentTypes", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Roles",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    Name = table.Column<string>(nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Roles", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "VariantGroups",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    Description = table.Column<string>(nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_VariantGroups", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Categories",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    Description = table.Column<string>(nullable: true),
//                    ParentCategoryId = table.Column<int>(nullable: true),
//                    CompanyId = table.Column<int>(nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Categories", x => x.Id);
//                table.ForeignKey(
//                    name: "FK_Categories_Companies_CompanyId",
//                    column: x => x.CompanyId,
//                    principalTable: "Companies",
//                    principalColumn: "Id");
//                        //onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_Categories_Categories_ParentCategoryId",
//                        column: x => x.ParentCategoryId,
//                        principalTable: "Categories",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Restrict);
//                });

//            migrationBuilder.CreateTable(
//                name: "Customers",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    Name = table.Column<string>(nullable: true),
//                    Email = table.Column<string>(nullable: true),
//                    PhoneNo = table.Column<string>(nullable: true),
//                    Address = table.Column<string>(nullable: true),
//                    City = table.Column<string>(nullable: true),
//                    PostalCode = table.Column<int>(nullable: false),
//                    CompanyId = table.Column<int>(nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Customers", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_Customers_Companies_CompanyId",
//                        column: x => x.CompanyId,
//                        principalTable: "Companies",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "Stores",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    Name = table.Column<string>(nullable: true),
//                    ParentStoreId = table.Column<int>(nullable: true),
//                    IsMainStore = table.Column<bool>(nullable: false),
//                    IsSalesStore = table.Column<bool>(nullable: false),
//                    Address = table.Column<string>(nullable: true),
//                    City = table.Column<string>(nullable: true),
//                    PostalCode = table.Column<string>(nullable: true),
//                    Country = table.Column<string>(nullable: true),
//                    ContactNo = table.Column<string>(nullable: true),
//                    Email = table.Column<string>(nullable: true),
//                    CompanyId = table.Column<int>(nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Stores", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_Stores_Companies_CompanyId",
//                        column: x => x.CompanyId,
//                        principalTable: "Companies",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_Stores_Stores_ParentStoreId",
//                        column: x => x.ParentStoreId,
//                        principalTable: "Stores",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Restrict);
//                });

//            migrationBuilder.CreateTable(
//                name: "TaxGroups",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    Description = table.Column<string>(nullable: true),
//                    Tax1 = table.Column<float>(nullable: true),
//                    Tax2 = table.Column<float>(nullable: true),
//                    Tax3 = table.Column<float>(nullable: true),
//                    CompanyId = table.Column<int>(nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_TaxGroups", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_TaxGroups_Companies_CompanyId",
//                        column: x => x.CompanyId,
//                        principalTable: "Companies",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "Units",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    Description = table.Column<string>(nullable: true),
//                    CompanyId = table.Column<int>(nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Units", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_Units_Companies_CompanyId",
//                        column: x => x.CompanyId,
//                        principalTable: "Companies",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "DropDowns",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    Name = table.Column<string>(nullable: true),
//                    DropDownGroupId = table.Column<int>(nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_DropDowns", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_DropDowns_DropDownGroups_DropDownGroupId",
//                        column: x => x.DropDownGroupId,
//                        principalTable: "DropDownGroups",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "UserRoles",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    Isdefault = table.Column<int>(nullable: false),
//                    CompanyId = table.Column<int>(nullable: false),
//                    RoleId = table.Column<int>(nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_UserRoles", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_UserRoles_Companies_CompanyId",
//                        column: x => x.CompanyId,
//                        principalTable: "Companies",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_UserRoles_Roles_RoleId",
//                        column: x => x.RoleId,
//                        principalTable: "Roles",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "Variants",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    Description = table.Column<string>(nullable: true),
//                    VariantGroupId = table.Column<int>(nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Variants", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_Variants_VariantGroups_VariantGroupId",
//                        column: x => x.VariantGroupId,
//                        principalTable: "VariantGroups",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "Users",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    Email = table.Column<string>(nullable: true),
//                    Name = table.Column<string>(nullable: true),
//                    PhoneNo = table.Column<string>(nullable: true),
//                    Password = table.Column<string>(nullable: true),
//                    City = table.Column<string>(nullable: true),
//                    Address = table.Column<string>(nullable: true),
//                    Country = table.Column<string>(nullable: true),
//                    PostalCode = table.Column<string>(nullable: true),
//                    CompanyId = table.Column<int>(nullable: false),
//                    StoreId = table.Column<int>(nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Users", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_Users_Companies_CompanyId",
//                        column: x => x.CompanyId,
//                        principalTable: "Companies",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                table.ForeignKey(
//                    name: "FK_Users_Stores_StoreId",
//                    column: x => x.StoreId,
//                    principalTable: "Stores",
//                    principalColumn: "Id");
//                        //onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "DiningTables",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    Description = table.Column<string>(nullable: true),
//                    DiningAreaId = table.Column<int>(nullable: false),
//                    TableStatusId = table.Column<int>(nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_DiningTables", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_DiningTables_DiningAreas_DiningAreaId",
//                        column: x => x.DiningAreaId,
//                        principalTable: "DiningAreas",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_DiningTables_DropDowns_TableStatusId",
//                        column: x => x.TableStatusId,
//                        principalTable: "DropDowns",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "Products",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    Description = table.Column<string>(nullable: true),
//                    UnitId = table.Column<int>(nullable: false),
//                    CategoryId = table.Column<int>(nullable: false),
//                    Price = table.Column<float>(nullable: false),
//                    TakeawayPrice = table.Column<float>(nullable: false),
//                    DeliveryPrice = table.Column<float>(nullable: false),
//                    TaxGroupId = table.Column<int>(nullable: false),
//                    Image = table.Column<byte[]>(nullable: true),
//                    TypeId = table.Column<int>(nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Products", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_Products_Categories_CategoryId",
//                        column: x => x.CategoryId,
//                        principalTable: "Categories",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_Products_TaxGroups_TaxGroupId",
//                        column: x => x.TaxGroupId,
//                        principalTable: "TaxGroups",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_Products_DropDowns_TypeId",
//                        column: x => x.TypeId,
//                        principalTable: "DropDowns",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Restrict);
//                table.ForeignKey(
//                    name: "FK_Products_Units_UnitId",
//                    column: x => x.UnitId,
//                    principalTable: "Units",
//                    principalColumn: "Id");
//                        //onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "Orders",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    OrderNo = table.Column<string>(nullable: true),
//                    OrderType = table.Column<string>(nullable: true),
//                    AggregatorOrderId = table.Column<int>(nullable: true),
//                    StoreId = table.Column<int>(nullable: false),
//                    CustomerId = table.Column<int>(nullable: false),
//                    OrderStatusId = table.Column<int>(nullable: false),
//                    BillAmount = table.Column<float>(nullable: false),
//                    PaidAmount = table.Column<float>(nullable: false),
//                    BillStatusId = table.Column<int>(nullable: false),
//                    SplitTableId = table.Column<int>(nullable: true),
//                    DiscPercent = table.Column<float>(nullable: false),
//                    DiscAmount = table.Column<float>(nullable: false),
//                    CashierId = table.Column<int>(nullable: false),
//                    WaiterId = table.Column<int>(nullable: false),
//                    OrderedDateTime = table.Column<DateTime>(nullable: false),
//                    OrderedDate = table.Column<DateTime>(type: "Date", nullable: false),
//                    BillDateTime = table.Column<DateTime>(nullable: false),
//                    BillDate = table.Column<DateTime>(type: "Date", nullable: false),
//                    Note = table.Column<string>(nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Orders", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_Orders_DropDowns_BillStatusId",
//                        column: x => x.BillStatusId,
//                        principalTable: "DropDowns",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_Orders_Users_CashierId",
//                        column: x => x.CashierId,
//                        principalTable: "Users",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                table.ForeignKey(
//                    name: "FK_Orders_Customers_CustomerId",
//                    column: x => x.CustomerId,
//                    principalTable: "Customers",
//                    principalColumn: "Id");
//                //onDelete: ReferentialAction.Cascade);
//                table.ForeignKey(
//                    name: "FK_Orders_DropDowns_OrderStatusId",
//                    column: x => x.OrderStatusId,
//                    principalTable: "DropDowns",
//                    principalColumn: "Id");
//                //onDelete: ReferentialAction.Cascade);
//                table.ForeignKey(
//                    name: "FK_Orders_Stores_StoreId",
//                    column: x => x.StoreId,
//                    principalTable: "Stores",
//                    principalColumn: "Id");
//                //onDelete: ReferentialAction.Cascade);
//                table.ForeignKey(
//                    name: "FK_Orders_Users_WaiterId",
//                    column: x => x.WaiterId,
//                    principalTable: "Users",
//                    principalColumn: "Id");
//                        //onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "ProductAddOns",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    ProductId = table.Column<int>(nullable: false),
//                    AddOnId = table.Column<int>(nullable: false),
//                    Price = table.Column<float>(nullable: false),
//                    TakeawayPrice = table.Column<float>(nullable: false),
//                    DeliveryPrice = table.Column<float>(nullable: false),
//                    AddOnGrpId = table.Column<int>(nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_ProductAddOns", x => x.Id);
//                table.ForeignKey(
//                    name: "FK_ProductAddOns_AddonGroups_AddOnGrpId",
//                    column: x => x.AddOnGrpId,
//                    principalTable: "AddonGroups",
//                    principalColumn: "Id");
//                //onDelete: ReferentialAction.Cascade);
//                table.ForeignKey(
//                    name: "FK_ProductAddOns_Products_AddOnId",
//                    column: x => x.AddOnId,
//                    principalTable: "Products",
//                    principalColumn: "Id");
//                //onDelete: ReferentialAction.Cascade);
//                table.ForeignKey(
//                    name: "FK_ProductAddOns_Products_ProductId",
//                    column: x => x.ProductId,
//                    principalTable: "Products",
//                    principalColumn: "Id");
//                        //onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "ProductVariants",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    ProductId = table.Column<int>(nullable: false),
//                    VariantId = table.Column<int>(nullable: false),
//                    Price = table.Column<float>(nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_ProductVariants", x => x.Id);
//                table.ForeignKey(
//                    name: "FK_ProductVariants_Products_ProductId",
//                    column: x => x.ProductId,
//                    principalTable: "Products",
//                    principalColumn: "Id");
//                //onDelete: ReferentialAction.Cascade);
//                table.ForeignKey(
//                    name: "FK_ProductVariants_Variants_VariantId",
//                    column: x => x.VariantId,
//                    principalTable: "Variants",
//                    principalColumn: "Id");
//                        //onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "StoreProducts",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    StoreId = table.Column<int>(nullable: false),
//                    ProductId = table.Column<int>(nullable: false),
//                    Price = table.Column<float>(nullable: false),
//                    TakeawayPrice = table.Column<float>(nullable: false),
//                    DeliveryPrice = table.Column<float>(nullable: false),
//                    IsDineInService = table.Column<bool>(nullable: true),
//                    IsTakeAwayService = table.Column<bool>(nullable: true),
//                    IsDeliveryService = table.Column<bool>(nullable: true),
//                    IsAvailable = table.Column<bool>(nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_StoreProducts", x => x.Id);
//                table.ForeignKey(
//                    name: "FK_StoreProducts_Products_ProductId",
//                    column: x => x.ProductId,
//                    principalTable: "Products",
//                    principalColumn: "Id");
//                //onDelete: ReferentialAction.Cascade);
//                table.ForeignKey(
//                    name: "FK_StoreProducts_Stores_StoreId",
//                    column: x => x.StoreId,
//                    principalTable: "Stores",
//                    principalColumn: "Id");
//                        //onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "KOTs",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    KOTStatusId = table.Column<int>(nullable: false),
//                    Instruction = table.Column<string>(nullable: true),
//                    OrderId = table.Column<int>(nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_KOTs", x => x.Id);
//                table.ForeignKey(
//                    name: "FK_KOTs_DropDowns_KOTStatusId",
//                    column: x => x.KOTStatusId,
//                    principalTable: "DropDowns",
//                    principalColumn: "Id");
//                //onDelete: ReferentialAction.Cascade);
//                table.ForeignKey(
//                    name: "FK_KOTs_Orders_OrderId",
//                    column: x => x.OrderId,
//                    principalTable: "Orders",
//                    principalColumn: "Id");
//                        //onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "OrderItems",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    Quantity = table.Column<float>(nullable: false),
//                    Price = table.Column<float>(nullable: false),
//                    OrderId = table.Column<int>(nullable: false),
//                    ProductId = table.Column<int>(nullable: false),
//                    Tax1 = table.Column<float>(nullable: true),
//                    Tax2 = table.Column<float>(nullable: true),
//                    Tax3 = table.Column<float>(nullable: true),
//                    DiscPercent = table.Column<float>(nullable: false),
//                    DiscAmount = table.Column<float>(nullable: false),
//                    StatusId = table.Column<int>(nullable: false),
//                    KitchenUserId = table.Column<int>(nullable: true),
//                    KOTId = table.Column<int>(nullable: false),
//                    Note = table.Column<string>(nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_OrderItems", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_OrderItems_Users_KitchenUserId",
//                        column: x => x.KitchenUserId,
//                        principalTable: "Users",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Restrict);
//                table.ForeignKey(
//                    name: "FK_OrderItems_Orders_OrderId",
//                    column: x => x.OrderId,
//                    principalTable: "Orders",
//                    principalColumn: "Id");
//                // onDelete: ReferentialAction.Cascade);
//                table.ForeignKey(
//                    name: "FK_OrderItems_Products_ProductId",
//                    column: x => x.ProductId,
//                    principalTable: "Products",
//                    principalColumn: "Id");
//                //onDelete: ReferentialAction.Cascade);
//                table.ForeignKey(
//                    name: "FK_OrderItems_DropDowns_StatusId",
//                    column: x => x.StatusId,
//                    principalTable: "DropDowns",
//                    principalColumn: "Id");
//                        //onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "Transactions",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    Amount = table.Column<float>(nullable: false),
//                    OrderId = table.Column<int>(nullable: false),
//                    CustomerId = table.Column<int>(nullable: false),
//                    PaymentTypeId = table.Column<int>(nullable: false),
//                    TransDateTime = table.Column<DateTime>(nullable: false),
//                    TransDate = table.Column<DateTime>(type: "Date", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Transactions", x => x.Id);
//                table.ForeignKey(
//                    name: "FK_Transactions_Customers_CustomerId",
//                    column: x => x.CustomerId,
//                    principalTable: "Customers",
//                    principalColumn: "Id");
//                // onDelete: ReferentialAction.Cascade);
//                table.ForeignKey(
//                    name: "FK_Transactions_Orders_OrderId",
//                    column: x => x.OrderId,
//                    principalTable: "Orders",
//                    principalColumn: "Id");
//                //onDelete: ReferentialAction.Cascade);
//                table.ForeignKey(
//                    name: "FK_Transactions_PaymentTypes_PaymentTypeId",
//                    column: x => x.PaymentTypeId,
//                    principalTable: "PaymentTypes",
//                    principalColumn: "Id");
//                        //onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "OrdItemAddons",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    OrderItemId = table.Column<int>(nullable: false),
//                    ProductAddonId = table.Column<int>(nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_OrdItemAddons", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_OrdItemAddons_OrderItems_OrderItemId",
//                        column: x => x.OrderItemId,
//                        principalTable: "OrderItems",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_OrdItemAddons_ProductAddOns_ProductAddonId",
//                        column: x => x.ProductAddonId,
//                        principalTable: "ProductAddOns",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "OrdItemVariants",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
//                    ProductVariantId = table.Column<int>(nullable: false),
//                    OrderItemId = table.Column<int>(nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_OrdItemVariants", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_OrdItemVariants_OrderItems_OrderItemId",
//                        column: x => x.OrderItemId,
//                        principalTable: "OrderItems",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_OrdItemVariants_ProductVariants_ProductVariantId",
//                        column: x => x.ProductVariantId,
//                        principalTable: "ProductVariants",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateIndex(
//                name: "IX_Categories_CompanyId",
//                table: "Categories",
//                column: "CompanyId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Categories_ParentCategoryId",
//                table: "Categories",
//                column: "ParentCategoryId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Customers_CompanyId",
//                table: "Customers",
//                column: "CompanyId");

//            migrationBuilder.CreateIndex(
//                name: "IX_DiningTables_DiningAreaId",
//                table: "DiningTables",
//                column: "DiningAreaId");

//            migrationBuilder.CreateIndex(
//                name: "IX_DiningTables_TableStatusId",
//                table: "DiningTables",
//                column: "TableStatusId");

//            migrationBuilder.CreateIndex(
//                name: "IX_DropDowns_DropDownGroupId",
//                table: "DropDowns",
//                column: "DropDownGroupId");

//            migrationBuilder.CreateIndex(
//                name: "IX_KOTs_KOTStatusId",
//                table: "KOTs",
//                column: "KOTStatusId");

//            migrationBuilder.CreateIndex(
//                name: "IX_KOTs_OrderId",
//                table: "KOTs",
//                column: "OrderId");

//            migrationBuilder.CreateIndex(
//                name: "IX_OrderItems_KitchenUserId",
//                table: "OrderItems",
//                column: "KitchenUserId");

//            migrationBuilder.CreateIndex(
//                name: "IX_OrderItems_OrderId",
//                table: "OrderItems",
//                column: "OrderId");

//            migrationBuilder.CreateIndex(
//                name: "IX_OrderItems_ProductId",
//                table: "OrderItems",
//                column: "ProductId");

//            migrationBuilder.CreateIndex(
//                name: "IX_OrderItems_StatusId",
//                table: "OrderItems",
//                column: "StatusId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Orders_BillStatusId",
//                table: "Orders",
//                column: "BillStatusId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Orders_CashierId",
//                table: "Orders",
//                column: "CashierId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Orders_CustomerId",
//                table: "Orders",
//                column: "CustomerId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Orders_OrderStatusId",
//                table: "Orders",
//                column: "OrderStatusId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Orders_StoreId",
//                table: "Orders",
//                column: "StoreId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Orders_WaiterId",
//                table: "Orders",
//                column: "WaiterId");

//            migrationBuilder.CreateIndex(
//                name: "IX_OrdItemAddons_OrderItemId",
//                table: "OrdItemAddons",
//                column: "OrderItemId");

//            migrationBuilder.CreateIndex(
//                name: "IX_OrdItemAddons_ProductAddonId",
//                table: "OrdItemAddons",
//                column: "ProductAddonId");

//            migrationBuilder.CreateIndex(
//                name: "IX_OrdItemVariants_OrderItemId",
//                table: "OrdItemVariants",
//                column: "OrderItemId");

//            migrationBuilder.CreateIndex(
//                name: "IX_OrdItemVariants_ProductVariantId",
//                table: "OrdItemVariants",
//                column: "ProductVariantId");

//            migrationBuilder.CreateIndex(
//                name: "IX_ProductAddOns_AddOnGrpId",
//                table: "ProductAddOns",
//                column: "AddOnGrpId");

//            migrationBuilder.CreateIndex(
//                name: "IX_ProductAddOns_AddOnId",
//                table: "ProductAddOns",
//                column: "AddOnId");

//            migrationBuilder.CreateIndex(
//                name: "IX_ProductAddOns_ProductId",
//                table: "ProductAddOns",
//                column: "ProductId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Products_CategoryId",
//                table: "Products",
//                column: "CategoryId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Products_TaxGroupId",
//                table: "Products",
//                column: "TaxGroupId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Products_TypeId",
//                table: "Products",
//                column: "TypeId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Products_UnitId",
//                table: "Products",
//                column: "UnitId");

//            migrationBuilder.CreateIndex(
//                name: "IX_ProductVariants_ProductId",
//                table: "ProductVariants",
//                column: "ProductId");

//            migrationBuilder.CreateIndex(
//                name: "IX_ProductVariants_VariantId",
//                table: "ProductVariants",
//                column: "VariantId");

//            migrationBuilder.CreateIndex(
//                name: "IX_StoreProducts_ProductId",
//                table: "StoreProducts",
//                column: "ProductId");

//            migrationBuilder.CreateIndex(
//                name: "IX_StoreProducts_StoreId",
//                table: "StoreProducts",
//                column: "StoreId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Stores_CompanyId",
//                table: "Stores",
//                column: "CompanyId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Stores_ParentStoreId",
//                table: "Stores",
//                column: "ParentStoreId");

//            migrationBuilder.CreateIndex(
//                name: "IX_TaxGroups_CompanyId",
//                table: "TaxGroups",
//                column: "CompanyId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Transactions_CustomerId",
//                table: "Transactions",
//                column: "CustomerId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Transactions_OrderId",
//                table: "Transactions",
//                column: "OrderId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Transactions_PaymentTypeId",
//                table: "Transactions",
//                column: "PaymentTypeId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Units_CompanyId",
//                table: "Units",
//                column: "CompanyId");

//            migrationBuilder.CreateIndex(
//                name: "IX_UserRoles_CompanyId",
//                table: "UserRoles",
//                column: "CompanyId");

//            migrationBuilder.CreateIndex(
//                name: "IX_UserRoles_RoleId",
//                table: "UserRoles",
//                column: "RoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Users_CompanyId",
//                table: "Users",
//                column: "CompanyId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Users_StoreId",
//                table: "Users",
//                column: "StoreId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Variants_VariantGroupId",
//                table: "Variants",
//                column: "VariantGroupId");
//        }

//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "DiningTables");

//            migrationBuilder.DropTable(
//                name: "KOTs");

//            migrationBuilder.DropTable(
//                name: "OrdItemAddons");

//            migrationBuilder.DropTable(
//                name: "OrdItemVariants");

//            migrationBuilder.DropTable(
//                name: "StoreProducts");

//            migrationBuilder.DropTable(
//                name: "Transactions");

//            migrationBuilder.DropTable(
//                name: "UserRoles");

//            migrationBuilder.DropTable(
//                name: "DiningAreas");

//            migrationBuilder.DropTable(
//                name: "ProductAddOns");

//            migrationBuilder.DropTable(
//                name: "OrderItems");

//            migrationBuilder.DropTable(
//                name: "ProductVariants");

//            migrationBuilder.DropTable(
//                name: "PaymentTypes");

//            migrationBuilder.DropTable(
//                name: "Roles");

//            migrationBuilder.DropTable(
//                name: "AddonGroups");

//            migrationBuilder.DropTable(
//                name: "Orders");

//            migrationBuilder.DropTable(
//                name: "Products");

//            migrationBuilder.DropTable(
//                name: "Variants");

//            migrationBuilder.DropTable(
//                name: "Users");

//            migrationBuilder.DropTable(
//                name: "Customers");

//            migrationBuilder.DropTable(
//                name: "Categories");

//            migrationBuilder.DropTable(
//                name: "TaxGroups");

//            migrationBuilder.DropTable(
//                name: "DropDowns");

//            migrationBuilder.DropTable(
//                name: "Units");

//            migrationBuilder.DropTable(
//                name: "VariantGroups");

//            migrationBuilder.DropTable(
//                name: "Stores");

//            migrationBuilder.DropTable(
//                name: "DropDownGroups");

//            migrationBuilder.DropTable(
//                name: "Companies");
//        }
//    }
//}
