namespace MobilePhoneStoreEcommerce.Migrations
{
    using MobilePhoneStoreEcommerce.Models.ControllerModels;
    using MobilePhoneStoreEcommerce.Persistence.Consts;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 249),
                        PasswordHash = c.String(nullable: false),
                        RoleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Roles", t => t.RoleID)
                .Index(t => t.UserName, unique: true)
                .Index(t => t.RoleID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(nullable: false, maxLength: 249),
                        DeliveryAddress = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Accounts", t => t.ID, cascadeDelete: true)
                .Index(t => t.ID)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        CustomerID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CustomerID, t.ProductID })
                .ForeignKey("dbo.Customers", t => t.CustomerID)
                .ForeignKey("dbo.Products", t => t.ProductID)
                .Index(t => t.CustomerID)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 249),
                        Quantity = c.Int(nullable: false),
                        Description = c.String(),
                        Status = c.Boolean(nullable: false),
                        Price = c.Int(nullable: false),
                        ProducerID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                        SellerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.CategoryID)
                .ForeignKey("dbo.Producers", t => t.ProducerID)
                .ForeignKey("dbo.Sellers", t => t.SellerID)
                .Index(t => t.Name, unique: true)
                .Index(t => t.ProducerID)
                .Index(t => t.CategoryID)
                .Index(t => t.SellerID);
            
            CreateTable(
                "dbo.AvatarOfProducts",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        Avatar = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 249),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CommentTime = c.DateTime(nullable: false),
                        Content = c.String(),
                        CustomerID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.CustomerID)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.Producers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 249),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.ProductsOfOrders",
                c => new
                    {
                        OrderID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrderID, t.ProductID })
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        CustomerID = c.Int(nullable: false),
                        InvoiceID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.CustomerID)
                .Index(t => new { t.CustomerID, t.OrderTime }, unique: true);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 249),
                        DateOfInvoice = c.DateTime(nullable: false),
                        TotalCost = c.Double(nullable: false),
                        OrderID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Orders", t => t.OrderID)
                .Index(t => t.OrderID);
            
            CreateTable(
                "dbo.Sellers",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(nullable: false, maxLength: 249),
                        WarehouseAddress = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Accounts", t => t.ID)
                .Index(t => t.ID)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "dbo.SpecificationValues",
                c => new
                    {
                        ProductSpecificationID = c.Int(nullable: false),
                        Value = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ProductSpecificationID, t.Value })
                .ForeignKey("dbo.ProductSpecifications", t => t.ProductSpecificationID)
                .Index(t => t.ProductSpecificationID);
            
            CreateTable(
                "dbo.ProductSpecifications",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 249),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.StarRatings",
                c => new
                    {
                        CustomerID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        NumberOfStart = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CustomerID, t.ProductID })
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.CustomerID)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 249),
                        Descriptions = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.SpecificationValueProducts",
                c => new
                    {
                        SpecificationValue_ProductSpecificationID = c.Int(nullable: false),
                        SpecificationValue_Value = c.String(nullable: false, maxLength: 128),
                        Product_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SpecificationValue_ProductSpecificationID, t.SpecificationValue_Value, t.Product_ID })
                .ForeignKey("dbo.SpecificationValues", t => new { t.SpecificationValue_ProductSpecificationID, t.SpecificationValue_Value }, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_ID, cascadeDelete: true)
                .Index(t => new { t.SpecificationValue_ProductSpecificationID, t.SpecificationValue_Value })
                .Index(t => t.Product_ID);
            
            CreateTable(
                "dbo.Wishlists",
                c => new
                    {
                        Customer_ID = c.Int(nullable: false),
                        Product_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Customer_ID, t.Product_ID })
                .ForeignKey("dbo.Customers", t => t.Customer_ID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_ID, cascadeDelete: true)
                .Index(t => t.Customer_ID)
                .Index(t => t.Product_ID);

            Sql("insert into Roles(Name) Values('Admin')");
            Sql("insert into Roles(Name) Values('Seller')");
            Sql("insert into Roles(Name) Values('Customer')");
            Sql("insert into Roles(Name) Values('Shipper')");

            var passwordHash = AccountModels.Encrypt("Admin", true);

            Sql("insert into Accounts(UserName, PasswordHash, RoleID) Values('Admin', '" + passwordHash  + "', " + RoleIds.Admin + ")");
            Sql("insert into Accounts(UserName, PasswordHash, RoleID) Values('Seller1', '" + passwordHash + "', " + RoleIds.Seller + ")");
            Sql("insert into Accounts(UserName, PasswordHash, RoleID) Values('Seller2', '" + passwordHash + "', " + RoleIds.Seller + ")");
            Sql("insert into Accounts(UserName, PasswordHash, RoleID) Values('Customer1', '" + passwordHash + "', " + RoleIds.Customer + ")");
            Sql("insert into Accounts(UserName, PasswordHash, RoleID) Values('Customer2', '" + passwordHash + "', " + RoleIds.Customer + ")");


            Sql("insert into Sellers(ID, Name, PhoneNumber, Email, WarehouseAddress) Values('2', 'Vinh', '0765764050', 'a@gmail.com', 'Vinh Long')");
            Sql("insert into Sellers(ID, Name, PhoneNumber, Email, WarehouseAddress) Values('3', 'Vinh', '0765764050', 'b@gmail.com', 'Vinh Long')");
            Sql("insert into Customers(ID, Name, PhoneNumber, Email, DeliveryAddress) Values('4', 'Vinh', '0765764050', 'c@gmail.com', 'Vinh Long')");
            Sql("insert into Customers(ID, Name, PhoneNumber, Email, DeliveryAddress) Values('5', 'Vinh', '0765764050', 'd@gmail.com', 'Vinh Long')");




        }

        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.Wishlists", "Product_ID", "dbo.Products");
            DropForeignKey("dbo.Wishlists", "Customer_ID", "dbo.Customers");
            DropForeignKey("dbo.Carts", "ProductID", "dbo.Products");
            DropForeignKey("dbo.StarRatings", "ProductID", "dbo.Products");
            DropForeignKey("dbo.StarRatings", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.SpecificationValues", "ProductSpecificationID", "dbo.ProductSpecifications");
            DropForeignKey("dbo.SpecificationValueProducts", "Product_ID", "dbo.Products");
            DropForeignKey("dbo.SpecificationValueProducts", new[] { "SpecificationValue_ProductSpecificationID", "SpecificationValue_Value" }, "dbo.SpecificationValues");
            DropForeignKey("dbo.Products", "SellerID", "dbo.Sellers");
            DropForeignKey("dbo.Sellers", "ID", "dbo.Accounts");
            DropForeignKey("dbo.ProductsOfOrders", "ProductID", "dbo.Products");
            DropForeignKey("dbo.ProductsOfOrders", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Invoices", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Orders", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Products", "ProducerID", "dbo.Producers");
            DropForeignKey("dbo.Comments", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Comments", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.AvatarOfProducts", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Carts", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Customers", "ID", "dbo.Accounts");
            DropIndex("dbo.Wishlists", new[] { "Product_ID" });
            DropIndex("dbo.Wishlists", new[] { "Customer_ID" });
            DropIndex("dbo.SpecificationValueProducts", new[] { "Product_ID" });
            DropIndex("dbo.SpecificationValueProducts", new[] { "SpecificationValue_ProductSpecificationID", "SpecificationValue_Value" });
            DropIndex("dbo.Roles", new[] { "Name" });
            DropIndex("dbo.StarRatings", new[] { "ProductID" });
            DropIndex("dbo.StarRatings", new[] { "CustomerID" });
            DropIndex("dbo.ProductSpecifications", new[] { "Name" });
            DropIndex("dbo.SpecificationValues", new[] { "ProductSpecificationID" });
            DropIndex("dbo.Sellers", new[] { "Email" });
            DropIndex("dbo.Sellers", new[] { "ID" });
            DropIndex("dbo.Invoices", new[] { "OrderID" });
            DropIndex("dbo.Orders", new[] { "CustomerID", "OrderTime" });
            DropIndex("dbo.ProductsOfOrders", new[] { "ProductID" });
            DropIndex("dbo.ProductsOfOrders", new[] { "OrderID" });
            DropIndex("dbo.Producers", new[] { "Name" });
            DropIndex("dbo.Comments", new[] { "ProductID" });
            DropIndex("dbo.Comments", new[] { "CustomerID" });
            DropIndex("dbo.Categories", new[] { "Name" });
            DropIndex("dbo.AvatarOfProducts", new[] { "ProductID" });
            DropIndex("dbo.Products", new[] { "SellerID" });
            DropIndex("dbo.Products", new[] { "CategoryID" });
            DropIndex("dbo.Products", new[] { "ProducerID" });
            DropIndex("dbo.Products", new[] { "Name" });
            DropIndex("dbo.Carts", new[] { "ProductID" });
            DropIndex("dbo.Carts", new[] { "CustomerID" });
            DropIndex("dbo.Customers", new[] { "Email" });
            DropIndex("dbo.Customers", new[] { "ID" });
            DropIndex("dbo.Accounts", new[] { "RoleID" });
            DropIndex("dbo.Accounts", new[] { "UserName" });
            DropTable("dbo.Wishlists");
            DropTable("dbo.SpecificationValueProducts");
            DropTable("dbo.Roles");
            DropTable("dbo.StarRatings");
            DropTable("dbo.ProductSpecifications");
            DropTable("dbo.SpecificationValues");
            DropTable("dbo.Sellers");
            DropTable("dbo.Invoices");
            DropTable("dbo.Orders");
            DropTable("dbo.ProductsOfOrders");
            DropTable("dbo.Producers");
            DropTable("dbo.Comments");
            DropTable("dbo.Categories");
            DropTable("dbo.AvatarOfProducts");
            DropTable("dbo.Products");
            DropTable("dbo.Carts");
            DropTable("dbo.Customers");
            DropTable("dbo.Accounts");
        }
    }
}
