using MobilePhoneStoreEcommerce.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence.EntitiyConfigurations
{
    public class ProductsOfOrderConfiguration : EntityTypeConfiguration<ProductsOfOrder>
    {
        public ProductsOfOrderConfiguration()
        {
            HasKey(p => new { p.OrderID, p.ProductID });

            HasRequired(p => p.Order)
                .WithMany(o => o.ProductsOfOrders)
                .HasForeignKey(p => p.OrderID);

            HasRequired(p => p.Product)
                .WithMany(p => p.ProductsOfOrders)
                .HasForeignKey(p => p.ProductID);

        }
    }
}