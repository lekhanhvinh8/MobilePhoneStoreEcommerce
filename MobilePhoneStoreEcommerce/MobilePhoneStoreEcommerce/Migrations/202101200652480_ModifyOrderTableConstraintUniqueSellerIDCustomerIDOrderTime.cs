namespace MobilePhoneStoreEcommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyOrderTableConstraintUniqueSellerIDCustomerIDOrderTime : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Orders", new[] { "CustomerID", "OrderTime" });
            DropIndex("dbo.Orders", new[] { "SellerID" });
            CreateIndex("dbo.Orders", new[] { "CustomerID", "OrderTime", "SellerID" }, unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Orders", new[] { "CustomerID", "OrderTime", "SellerID" });
            CreateIndex("dbo.Orders", "SellerID");
            CreateIndex("dbo.Orders", new[] { "CustomerID", "OrderTime" }, unique: true);
        }
    }
}
