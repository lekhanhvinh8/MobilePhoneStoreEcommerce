using MobilePhoneStoreEcommerce.Core.Domain;
using MobilePhoneStoreEcommerce.Persistence.EntitiyConfigurations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            :base("name=defaultConnection")
        {

        }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Producer> Producers { get; set; }
        public virtual DbSet<ProductSpecification> ProductSpecifications { get; set; }
        public virtual DbSet<SpecificationValue> SpecificationValues { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Customer> Cutomers { get; set; }
        public virtual DbSet<Seller> Sellers { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ProductsOfOrder> ProductsOfOrders { get; set; }
        public virtual DbSet<AvatarOfProduct> AvatarOfProducts { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<StarRating> StarRatings { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new AccountConfiguration());
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new ProducerConfiguration());
            modelBuilder.Configurations.Add(new CustomerConfiguration());
            modelBuilder.Configurations.Add(new SellerConfiguration());
            modelBuilder.Configurations.Add(new ProductSpecificationConfiguration());
            modelBuilder.Configurations.Add(new SpecificationValueConfiguration());
            modelBuilder.Configurations.Add(new ProductConfiguration());
            modelBuilder.Configurations.Add(new AvatarOfProductConfiguration());
            modelBuilder.Configurations.Add(new CartConfiguration());
            modelBuilder.Configurations.Add(new OrderConfiguration());
            modelBuilder.Configurations.Add(new ProductsOfOrderConfiguration());
            modelBuilder.Configurations.Add(new InvoiceConfiguration());
            modelBuilder.Configurations.Add(new CommentConfiguration());
            modelBuilder.Configurations.Add(new StartRatingConfiguration());
        }

    }
}