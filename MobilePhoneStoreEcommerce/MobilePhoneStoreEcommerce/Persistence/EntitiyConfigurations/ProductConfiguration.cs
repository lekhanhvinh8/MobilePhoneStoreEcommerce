using MobilePhoneStoreEcommerce.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence.EntitiyConfigurations
{
    public class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(249);
            Property(p => p.CategoryID)
                .IsRequired();
            Property(p => p.ProducerID)
                .IsRequired();

            HasIndex(p => p.Name)
                .IsUnique();

            HasRequired(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryID)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Producer)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.ProducerID)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Seller)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SellerID)
                .WillCascadeOnDelete(false);
        }
    }
}