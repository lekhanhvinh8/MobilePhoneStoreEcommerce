using MobilePhoneStoreEcommerce.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence.EntitiyConfigurations
{
    public class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            HasIndex(o => new { o.CustomerID, o.OrderTime, o.SellerID})
                .IsUnique();

            HasRequired(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerID)
                .WillCascadeOnDelete(false);

            HasRequired(o => o.Seller)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.SellerID)
                .WillCascadeOnDelete(false);
        }
    }
}