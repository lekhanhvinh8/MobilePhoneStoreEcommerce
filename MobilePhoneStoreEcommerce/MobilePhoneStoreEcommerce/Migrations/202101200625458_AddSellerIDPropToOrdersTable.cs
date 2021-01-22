namespace MobilePhoneStoreEcommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSellerIDPropToOrdersTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "SellerID", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "SellerID");
            AddForeignKey("dbo.Orders", "SellerID", "dbo.Sellers", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "SellerID", "dbo.Sellers");
            DropIndex("dbo.Orders", new[] { "SellerID" });
            DropColumn("dbo.Orders", "SellerID");
        }
    }
}
