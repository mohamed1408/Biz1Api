﻿// <auto-generated />
using System;
using Biz1BookPOS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Biz1PosApi.Migrations
{
    [DbContext(typeof(POSDbContext))]
    [Migration("20190706065045_prdad")]
    partial class prdad
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Biz1BookPOS.Models.AddonGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("AddonGroups");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId");

                    b.Property<string>("Description");

                    b.Property<int?>("ParentCategoryId");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("ParentCategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DisplayName");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<int>("CompanyId");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("PhoneNo");

                    b.Property<int>("PostalCode");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.DiningArea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("DiningAreas");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.DiningTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<int>("DiningAreaId");

                    b.Property<int>("TableStatusId");

                    b.HasKey("Id");

                    b.HasIndex("DiningAreaId");

                    b.HasIndex("TableStatusId");

                    b.ToTable("DiningTables");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.DropDown", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DropDownGroupId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("DropDownGroupId");

                    b.ToTable("DropDowns");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.DropDownGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("DropDownGroups");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.KOT", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Instruction");

                    b.Property<int>("KOTStatusId");

                    b.Property<int>("OrderId");

                    b.HasKey("Id");

                    b.HasIndex("KOTStatusId");

                    b.HasIndex("OrderId");

                    b.ToTable("KOTs");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.OrdItemAddon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("OrderItemId");

                    b.Property<int>("ProductAddonId");

                    b.HasKey("Id");

                    b.HasIndex("OrderItemId");

                    b.HasIndex("ProductAddonId");

                    b.ToTable("OrdItemAddons");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.OrdItemVariant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("OrderItemId");

                    b.Property<int>("ProductVariantId");

                    b.HasKey("Id");

                    b.HasIndex("OrderItemId");

                    b.HasIndex("ProductVariantId");

                    b.ToTable("OrdItemVariants");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AggregatorOrderId");

                    b.Property<float>("BillAmount");

                    b.Property<DateTime>("BillDate")
                        .HasColumnType("Date");

                    b.Property<DateTime>("BillDateTime");

                    b.Property<int>("BillStatusId");

                    b.Property<int>("CashierId");

                    b.Property<int>("CustomerId");

                    b.Property<float>("DiscAmount");

                    b.Property<float>("DiscPercent");

                    b.Property<string>("Note");

                    b.Property<string>("OrderNo");

                    b.Property<int>("OrderStatusId");

                    b.Property<string>("OrderType");

                    b.Property<DateTime>("OrderedDate")
                        .HasColumnType("Date");

                    b.Property<DateTime>("OrderedDateTime");

                    b.Property<float>("PaidAmount");

                    b.Property<int?>("SplitTableId");

                    b.Property<int>("StoreId");

                    b.Property<int>("WaiterId");

                    b.HasKey("Id");

                    b.HasIndex("BillStatusId");

                    b.HasIndex("CashierId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("OrderStatusId");

                    b.HasIndex("StoreId");

                    b.HasIndex("WaiterId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("DiscAmount");

                    b.Property<float>("DiscPercent");

                    b.Property<int>("KOTId");

                    b.Property<int?>("KitchenUserId");

                    b.Property<string>("Note");

                    b.Property<int>("OrderId");

                    b.Property<float>("Price");

                    b.Property<int>("ProductId");

                    b.Property<float>("Quantity");

                    b.Property<int>("StatusId");

                    b.Property<float?>("Tax1");

                    b.Property<float?>("Tax2");

                    b.Property<float?>("Tax3");

                    b.HasKey("Id");

                    b.HasIndex("KitchenUserId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.HasIndex("StatusId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.PaymentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("PaymentTypes");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId");

                    b.Property<float>("DeliveryPrice");

                    b.Property<string>("Description");

                    b.Property<byte[]>("Image");

                    b.Property<float>("Price");

                    b.Property<float>("TakeawayPrice");

                    b.Property<int>("TaxGroupId");

                    b.Property<int?>("TypeId");

                    b.Property<int>("UnitId");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("TaxGroupId");

                    b.HasIndex("TypeId");

                    b.HasIndex("UnitId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.ProductAddOn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AddOnId");

                    b.Property<float>("DeliveryPrice");

                    b.Property<float>("Price");

                    b.Property<int>("ProductId");

                    b.Property<float>("TakeawayPrice");

                    b.HasKey("Id");

                    b.HasIndex("AddOnId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductAddOns");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.ProductVariant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float?>("DeliveryPrice");

                    b.Property<float>("Price");

                    b.Property<int>("ProductId");

                    b.Property<float?>("TakeawayPrice");

                    b.Property<int>("VariantId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("VariantId");

                    b.ToTable("ProductVariants");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<int>("CompanyId");

                    b.Property<string>("ContactNo");

                    b.Property<string>("Country");

                    b.Property<string>("Email");

                    b.Property<bool>("IsMainStore");

                    b.Property<bool>("IsSalesStore");

                    b.Property<string>("Name");

                    b.Property<int?>("ParentStoreId");

                    b.Property<string>("PostalCode");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("ParentStoreId");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.StoreProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("DeliveryPrice");

                    b.Property<bool?>("IsAvailable");

                    b.Property<bool?>("IsDeliveryService");

                    b.Property<bool?>("IsDineInService");

                    b.Property<bool?>("IsTakeAwayService");

                    b.Property<float>("Price");

                    b.Property<int>("ProductId");

                    b.Property<int>("StoreId");

                    b.Property<float>("TakeawayPrice");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("StoreId");

                    b.ToTable("StoreProducts");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.TaxGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId");

                    b.Property<string>("Description");

                    b.Property<float?>("Tax1");

                    b.Property<float?>("Tax2");

                    b.Property<float?>("Tax3");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("TaxGroups");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Amount");

                    b.Property<int>("CustomerId");

                    b.Property<int>("OrderId");

                    b.Property<int>("PaymentTypeId");

                    b.Property<DateTime>("TransDate")
                        .HasColumnType("Date");

                    b.Property<DateTime>("TransDateTime");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("OrderId");

                    b.HasIndex("PaymentTypeId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.Unit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId");

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<int>("CompanyId");

                    b.Property<string>("Country");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("PhoneNo");

                    b.Property<string>("PostalCode");

                    b.Property<int>("StoreId");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("StoreId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId");

                    b.Property<int>("Isdefault");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.Variant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<int>("VariantGroupId");

                    b.HasKey("Id");

                    b.HasIndex("VariantGroupId");

                    b.ToTable("Variants");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.VariantGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("VariantGroups");
                });

            modelBuilder.Entity("Biz1PosApi.Models.Addon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AddonGroupId");

                    b.Property<int>("ProductId");

                    b.HasKey("Id");

                    b.HasIndex("AddonGroupId");

                    b.HasIndex("ProductId");

                    b.ToTable("Addons");
                });

            modelBuilder.Entity("Biz1PosApi.Models.StoreProductVariant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float?>("DeliveryPrice");

                    b.Property<float?>("Price");

                    b.Property<int>("ProductId");

                    b.Property<int>("StoreId");

                    b.Property<float?>("TakeawayPrice");

                    b.Property<int>("VariantId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("StoreId");

                    b.HasIndex("VariantId");

                    b.ToTable("StoreProductVariants");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.Category", b =>
                {
                    b.HasOne("Biz1BookPOS.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.Category", "ParentCategory")
                        .WithMany()
                        .HasForeignKey("ParentCategoryId");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.Customer", b =>
                {
                    b.HasOne("Biz1BookPOS.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Biz1BookPOS.Models.DiningTable", b =>
                {
                    b.HasOne("Biz1BookPOS.Models.DiningArea", "DiningArea")
                        .WithMany()
                        .HasForeignKey("DiningAreaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.DropDown", "DropDown")
                        .WithMany()
                        .HasForeignKey("TableStatusId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Biz1BookPOS.Models.DropDown", b =>
                {
                    b.HasOne("Biz1BookPOS.Models.DropDownGroup", "DropDownGroup")
                        .WithMany()
                        .HasForeignKey("DropDownGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Biz1BookPOS.Models.KOT", b =>
                {
                    b.HasOne("Biz1BookPOS.Models.DropDown", "Status")
                        .WithMany()
                        .HasForeignKey("KOTStatusId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Biz1BookPOS.Models.OrdItemAddon", b =>
                {
                    b.HasOne("Biz1BookPOS.Models.OrderItem", "OrderItem")
                        .WithMany()
                        .HasForeignKey("OrderItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.ProductAddOn", "ProductAddon")
                        .WithMany()
                        .HasForeignKey("ProductAddonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Biz1BookPOS.Models.OrdItemVariant", b =>
                {
                    b.HasOne("Biz1BookPOS.Models.OrderItem", "OrderItem")
                        .WithMany()
                        .HasForeignKey("OrderItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.ProductVariant", "ProductVariant")
                        .WithMany()
                        .HasForeignKey("ProductVariantId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Biz1BookPOS.Models.Order", b =>
                {
                    b.HasOne("Biz1BookPOS.Models.DropDown", "BillStatus")
                        .WithMany()
                        .HasForeignKey("BillStatusId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.User", "Cashier")
                        .WithMany()
                        .HasForeignKey("CashierId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.DropDown", "OrderStatus")
                        .WithMany()
                        .HasForeignKey("OrderStatusId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.User", "POSUser")
                        .WithMany()
                        .HasForeignKey("WaiterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Biz1BookPOS.Models.OrderItem", b =>
                {
                    b.HasOne("Biz1BookPOS.Models.User", "KitchenUser")
                        .WithMany()
                        .HasForeignKey("KitchenUserId");

                    b.HasOne("Biz1BookPOS.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.DropDown", "DropDown")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Biz1BookPOS.Models.Product", b =>
                {
                    b.HasOne("Biz1BookPOS.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.TaxGroup", "TaxGroup")
                        .WithMany()
                        .HasForeignKey("TaxGroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.DropDown", "DropDown")
                        .WithMany()
                        .HasForeignKey("TypeId");

                    b.HasOne("Biz1BookPOS.Models.Unit", "Unit")
                        .WithMany()
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Biz1BookPOS.Models.ProductAddOn", b =>
                {
                    b.HasOne("Biz1PosApi.Models.Addon", "AddOn")
                        .WithMany()
                        .HasForeignKey("AddOnId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Biz1BookPOS.Models.ProductVariant", b =>
                {
                    b.HasOne("Biz1BookPOS.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.Variant", "Variant")
                        .WithMany()
                        .HasForeignKey("VariantId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Biz1BookPOS.Models.Store", b =>
                {
                    b.HasOne("Biz1BookPOS.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.Store", "ParentStore")
                        .WithMany()
                        .HasForeignKey("ParentStoreId");
                });

            modelBuilder.Entity("Biz1BookPOS.Models.StoreProduct", b =>
                {
                    b.HasOne("Biz1BookPOS.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Biz1BookPOS.Models.TaxGroup", b =>
                {
                    b.HasOne("Biz1BookPOS.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Biz1BookPOS.Models.Transaction", b =>
                {
                    b.HasOne("Biz1BookPOS.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.PaymentType", "PaymentType")
                        .WithMany()
                        .HasForeignKey("PaymentTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Biz1BookPOS.Models.Unit", b =>
                {
                    b.HasOne("Biz1BookPOS.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Biz1BookPOS.Models.User", b =>
                {
                    b.HasOne("Biz1BookPOS.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Biz1BookPOS.Models.UserRole", b =>
                {
                    b.HasOne("Biz1BookPOS.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Biz1BookPOS.Models.Variant", b =>
                {
                    b.HasOne("Biz1BookPOS.Models.VariantGroup", "VariantGroup")
                        .WithMany()
                        .HasForeignKey("VariantGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Biz1PosApi.Models.Addon", b =>
                {
                    b.HasOne("Biz1BookPOS.Models.AddonGroup", "AddonGroup")
                        .WithMany()
                        .HasForeignKey("AddonGroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Biz1PosApi.Models.StoreProductVariant", b =>
                {
                    b.HasOne("Biz1BookPOS.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biz1BookPOS.Models.Variant", "Variant")
                        .WithMany()
                        .HasForeignKey("VariantId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
