using MobilePhoneStoreEcommerce.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence.EntitiyConfigurations
{
    public class CartConfiguration : EntityTypeConfiguration<Cart>
    {
        public CartConfiguration()
        {
            HasKey(c => new { c.CustomerID, c.ProductID });

            HasRequired(c => c.Customer)
                .WithMany(c => c.Carts)
                .HasForeignKey(c => c.CustomerID)
                .WillCascadeOnDelete(false);

            HasRequired(c => c.Product)
                .WithMany(p => p.Carts)
                .HasForeignKey(c => c.ProductID)
                .WillCascadeOnDelete(false);
        }
    }
}