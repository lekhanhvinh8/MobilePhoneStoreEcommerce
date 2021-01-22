namespace MobilePhoneStoreEcommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeliveryAddressToOrderTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "DeliveryAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "DeliveryAddress");
        }
    }
}
