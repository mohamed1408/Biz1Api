using Biz1PosApi.Models;
using Biz1Retail_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite.Internal.ApacheModRewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Biz1BookPOS.Models
{
    public static class Config
    {
        public static Dictionary<string, string> Application = new Dictionary<string, string>();
    }
    public class POSDbContext : DbContext
    {
        public POSDbContext(DbContextOptions<POSDbContext> options) : base(options)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (optionsBuilder.IsConfigured)
        //    {
        //        Config.Application["conn_name"] = "logout";
        //        QueryString qs = _httpContext.Request.QueryString;
        //        var ci = qs.ToUriComponent();
        //        var clientClaim = _httpContext?.User.Claims.Where(c => c.Type == ClaimTypes.GroupSid).Select(c => c.Value).SingleOrDefault();
        //        if (clientClaim == null) clientClaim = "DEBUG"; // Let's say there is no http context, like when you update-database from PMC
        //        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("Biz1Retail_CS"));
        //    }
        //}

        public DbSet<Company> Companies { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<TaxGroup> TaxGroups { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<VariantGroup> VariantGroups { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAddOn> ProductAddOns { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<KOT> KOTs { get; set; }
        public DbSet<OrdItemVariant> OrdItemVariants { get; set; }
        public DbSet<OrdItemAddon> OrdItemAddons { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<DiningArea> DiningAreas { get; set; }
        public DbSet<DiningTable> DiningTables { get; set; }
        public DbSet<DropDownGroup> DropDownGroups { get; set; }
        public DbSet<DropDown> DropDowns { get; set; }
        public DbSet<StoreProduct> StoreProducts { get; set; }
        public DbSet<AddonGroup> AddonGroups { get; set; }
        public DbSet<StoreProductVariant> StoreProductVariants { get; set; }
        public DbSet<Addon> Addons { get; set; }
        public DbSet<StoreProductAddon> StoreProductAddons { get; set; }
        public DbSet<OrderType> OrderTypes { get; set; }
        public DbSet<ProductVariantGroup> ProductVariantGroups { get; set; }
        public DbSet<ProductAddonGroup> ProductAddonGroups { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<DiscountRule> DiscountRules { get; set; }
        public DbSet<AdditionalCharges> AdditionalCharges { get; set; }
        public DbSet<OrderCharges> OrderCharges { get; set; }
        public DbSet<KOTGroup> KOTGroups { get; set; }
        public DbSet<TransType> TransTypes { get; set; }
        public DbSet<Aggregator> Aggregators { get; set; }
        public DbSet<OptionGroup> OptionGroups { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<ProductOptionGroup> ProductOptionGroups { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }
        public DbSet<StoreProductOption> StoreProductOptions { get; set; }
        public DbSet<CategoryAddon> CategoryAddons { get; set; }
        public DbSet<CategoryVariant> CategoryVariants { get; set; }
        public DbSet<CategoryOption> CategoryOptions { get; set; }
        public DbSet<UserStores> UserStores { get; set; }
        public DbSet<Printer> Printers { get; set; }
        public DbSet<OrdItemOptions> ordItemOptions { get; set; }
        public DbSet<Preference> Preferences { get; set; }
        public DbSet<OrdItemOptions> OrdItemOptions { get; set; }
        public DbSet<ShiftSummary> ShiftSummaries { get; set; }
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<UserAccounts> UserAccounts { get; set; }
        public DbSet<UrbanPiperKey> UrbanPiperKeys { get; set; }            
        public DbSet<UrbanPiperStore> UrbanPiperStores { get; set; }
        public DbSet<CategoryOptionGroup> CategoryOptionGroups { get; set; }
        public DbSet<StoreOption> StoreOptions { get; set; }
        public DbSet<WebhookResponse> WebhookResponses { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<UPOrder> UPOrders { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<UPProduct> UPProducts { get; set; }
        public DbSet<UPLog> UPLogs { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<VariableType> VariableTypes { get; set; }
        public DbSet<OfferType> OfferTypes { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<OfferRule> OfferRules { get; set; }
        public DbSet<CustomerOffer> CustomerOffers { get; set; }
        public DbSet<Cndition> Cnditions { get; set; }
        public DbSet<StorePreference> StorePreferences { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<ScreenRule> ScreenRules { get; set; }
        public DbSet<OrderLog> OrderLogs { get; set; }
        public DbSet<KOTGroupPrinter> KOTGroupPrinters { get; set; }
        public DbSet<KOTGroupUser> KOTGroupUsers { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<StorePaymentType> StorePaymentTypes { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagMapping> TagMappings { get; set; }
        public DbSet<UserCompany> UserCompanies { get; set; }
        public DbSet<OrderTypePreference> OrderTypePreferences { get; set; }
        public DbSet<OrderStatusButton> OrderStatusButtons { get; set; }
        public DbSet<testorder> testorders { get; set; }
        public DbSet<DenominationType> DenominationTypes { get; set; }
        public DbSet<DenomEntry> DenomEntries { get; set; }
        public DbSet<Denomination> Denominations { get; set; }
        public DbSet<SaleProductGroup> SaleProductGroups { get; set; }
        public DbSet<PredefinedQuantity> PredefinedQuantities { get; set; }
        public DbSet<CakeQuantity> CakeQuantities { get; set; }
        public DbSet<ReportPreset> ReportPresets { get; set; }
        public DbSet<QuantityGroup> QuantityGroups{ get; set; }
        public DbSet<OptionQtyGroup> OptionQtyGroups{ get; set; }
        public DbSet<UPTag> UPTags{ get; set; }
        public DbSet<UrbanPiperOrder> UrbanPiperOrders{ get; set; }
        public DbSet<ContactForm> ContactForms{ get; set; }
        public DbSet<Review> Reviews{ get; set; }
        public DbSet<StoreConfig> StoreConfigs { get; set; }
        public DbSet<Career> Careers { get; set; }
        public DbSet<DashBoard> DashBoards { get; set; }
        public DbSet<StoreFIlter> StoreFIlters { get; set; }
        public DbSet<StoreLog> StoreLogs { get; set; }
        public DbSet<StoreDetail> StoreDetails { get; set; }
        public DbSet<CustomerRequire> CustomerRequires { get; set; }
        public DbSet<KOTInstruction> KOTInstructions { get; set; }
        public DbSet<EComProduct> EComProducts { get; set; }
        public DbSet<UPCoupon> UPCoupons { get; set; }
        public DbSet<DiscountVerification> DiscountVerifications { get; set; }
        public DbSet<RestaurantOrder> RestaurantOrders { get; set; }
        public DbSet <ProdGrp> ProdGrps { get; set; }
        public DbSet <ProdGrpAmt> ProdGrpAmts{ get; set; }
        public DbSet<ProdGrpUpload> ProdGrpUploads { get; set; }
        public DbSet<chumon> Chumons { get; set; }
        public DbSet<chumonBranch> ChumonBranches { get; set; }
        public DbSet<Odrs> Odrs { get; set; }
        public DbSet<Otms> Otms { get; set; }
        public DbSet<MenuMappings> MenuMappings { get; set; }
        public DbSet<OldProducts> OldProducts { get; set; }
        public DbSet<StoreOB> StoreOB { get; set; }
        public DbSet<Q> Q { get; set; }
        public DbSet<W> W { get; set; }
        public DbSet<E> E { get; set; }
        public DbSet<R> R { get; set; }
        public DbSet<T> T { get; set; }
        public DbSet<Y> Y { get; set; }
        public DbSet<GuestAddon> GuestAddons { get; set; }
    }

    public class TempDbContext : DbContext
    {
        public TempDbContext(DbContextOptions<TempDbContext> options) : base(options)
        {
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<TaxGroup> TaxGroups { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<VariantGroup> VariantGroups { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAddOn> ProductAddOns { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<KOT> KOTs { get; set; }
        public DbSet<OrdItemVariant> OrdItemVariants { get; set; }
        public DbSet<OrdItemAddon> OrdItemAddons { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<DiningArea> DiningAreas { get; set; }
        public DbSet<DiningTable> DiningTables { get; set; }
        public DbSet<DropDownGroup> DropDownGroups { get; set; }
        public DbSet<DropDown> DropDowns { get; set; }
        public DbSet<StoreProduct> StoreProducts { get; set; }
        public DbSet<AddonGroup> AddonGroups { get; set; }
        public DbSet<StoreProductVariant> StoreProductVariants { get; set; }
        public DbSet<Addon> Addons { get; set; }
        public DbSet<StoreProductAddon> StoreProductAddons { get; set; }
        public DbSet<OrderType> OrderTypes { get; set; }
        public DbSet<ProductVariantGroup> ProductVariantGroups { get; set; }
        public DbSet<ProductAddonGroup> ProductAddonGroups { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<DiscountRule> DiscountRules { get; set; }
        public DbSet<AdditionalCharges> AdditionalCharges { get; set; }
        public DbSet<OrderCharges> OrderCharges { get; set; }
        public DbSet<KOTGroup> KOTGroups { get; set; }
        public DbSet<TransType> TransTypes { get; set; }
        public DbSet<Aggregator> Aggregators { get; set; }
        public DbSet<OptionGroup> OptionGroups { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<ProductOptionGroup> ProductOptionGroups { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }
        public DbSet<StoreProductOption> StoreProductOptions { get; set; }
        public DbSet<CategoryAddon> CategoryAddons { get; set; }
        public DbSet<CategoryVariant> CategoryVariants { get; set; }
        public DbSet<CategoryOption> CategoryOptions { get; set; }
        public DbSet<UserStores> UserStores { get; set; }
        public DbSet<Printer> Printers { get; set; }
        public DbSet<OrdItemOptions> ordItemOptions { get; set; }
        public DbSet<Preference> Preferences { get; set; }
        public DbSet<OrdItemOptions> OrdItemOptions { get; set; }
        public DbSet<ShiftSummary> ShiftSummaries { get; set; }
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<UserAccounts> UserAccounts { get; set; }
        public DbSet<UrbanPiperKey> UrbanPiperKeys { get; set; }
        public DbSet<UrbanPiperStore> UrbanPiperStores { get; set; }
        public DbSet<CategoryOptionGroup> CategoryOptionGroups { get; set; }
        public DbSet<StoreOption> StoreOptions { get; set; }
        public DbSet<WebhookResponse> WebhookResponses { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<UPOrder> UPOrders { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<UPProduct> UPProducts { get; set; }
        public DbSet<UPLog> UPLogs { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<VariableType> VariableTypes { get; set; }
        public DbSet<OfferType> OfferTypes { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<OfferRule> OfferRules { get; set; }
        public DbSet<CustomerOffer> CustomerOffers { get; set; }
        public DbSet<Cndition> Cnditions { get; set; }
        public DbSet<StorePreference> StorePreferences { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<ScreenRule> ScreenRules { get; set; }
        public DbSet<OrderLog> OrderLogs { get; set; }
        public DbSet<KOTGroupPrinter> KOTGroupPrinters { get; set; }
        public DbSet<KOTGroupUser> KOTGroupUsers { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<StorePaymentType> StorePaymentTypes { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagMapping> TagMappings { get; set; }
        public DbSet<UserCompany> UserCompanies { get; set; }
        public DbSet<OrderTypePreference> OrderTypePreferences { get; set; }
        public DbSet<OrderStatusButton> OrderStatusButtons { get; set; }
        public DbSet<testorder> testorders { get; set; }
        public DbSet<DenominationType> DenominationTypes { get; set; }
        public DbSet<DenomEntry> DenomEntries { get; set; }
        public DbSet<Denomination> Denominations { get; set; }
        public DbSet<SaleProductGroup> SaleProductGroups { get; set; }
        public DbSet<PredefinedQuantity> PredefinedQuantities { get; set; }
        public DbSet<CakeQuantity> CakeQuantities { get; set; }
        public DbSet<ReportPreset> ReportPresets { get; set; }
        public DbSet<QuantityGroup> QuantityGroups { get; set; }
        public DbSet<OptionQtyGroup> OptionQtyGroups { get; set; }
        public DbSet<UPTag> UPTags { get; set; }
        public DbSet<UrbanPiperOrder> UrbanPiperOrders { get; set; }
        public DbSet<ContactForm> ContactForms { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<StoreConfig> StoreConfigs { get; set; }
        public DbSet<Career> Careers { get; set; }
        public DbSet<DashBoard> DashBoards { get; set; }
        public DbSet<StoreFIlter> StoreFIlters { get; set; }
        public DbSet<StoreLog> StoreLogs { get; set; }
        public DbSet<StoreDetail> StoreDetails { get; set; }
        public DbSet<CustomerRequire> CustomerRequires { get; set; }
        public DbSet<KOTInstruction> KOTInstructions { get; set; }
        public DbSet<EComProduct> EComProducts { get; set; }
        public DbSet<UPCoupon> UPCoupons { get; set; }
        public DbSet<DiscountVerification> DiscountVerifications { get; set; }
        public DbSet<RestaurantOrder> RestaurantOrders { get; set; }
        public DbSet<ProdGrp> ProdGrps { get; set; }
        public DbSet<ProdGrpAmt> ProdGrpAmts { get; set; }
        public DbSet<ProdGrpUpload> ProdGrpUploads { get; set; }
        public DbSet<chumon> Chumons { get; set; }
        public DbSet<chumonBranch> ChumonBranches { get; set; }
        public DbSet<Odrs> Odrs { get; set; }
        public DbSet<Otms> Otms { get; set; }
        public DbSet<MenuMappings> MenuMappings { get; set; }
        public DbSet<OldProducts> OldProducts { get; set; }
        public DbSet<StoreOB> StoreOB { get; set; }
        public DbSet<Q> Q { get; set; }
        public DbSet<W> W { get; set; }
        public DbSet<E> E { get; set; }
        public DbSet<R> R { get; set; }
        public DbSet<T> T { get; set; }
        public DbSet<Y> Y { get; set; }
    }
}