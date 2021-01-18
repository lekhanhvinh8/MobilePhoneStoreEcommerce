using MobilePhoneStoreEcommerce.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence.EntitiyConfigurations
{
    public class SellerConfiguration : EntityTypeConfiguration<Seller>
    {
        public SellerConfiguration()
        {
            Property(s => s.Name)
                .IsRequired();
            Property(s => s.PhoneNumber)
                .IsRequired();
            Property(s => s.WarehouseAddress)
                .IsRequired();
            Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(249);

            HasIndex(s => s.Email)
                .IsUnique();

            HasRequired(s => s.Account)
                .WithOptional(a => a.Seller)
                .WillCascadeOnDelete(false);
        }
    }
}