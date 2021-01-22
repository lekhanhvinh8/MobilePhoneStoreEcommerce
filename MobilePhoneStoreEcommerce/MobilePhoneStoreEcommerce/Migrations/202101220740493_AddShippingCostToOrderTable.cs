namespace MobilePhoneStoreEcommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShippingCostToOrderTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ShippingCost", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ShippingCost");
        }
    }
}
