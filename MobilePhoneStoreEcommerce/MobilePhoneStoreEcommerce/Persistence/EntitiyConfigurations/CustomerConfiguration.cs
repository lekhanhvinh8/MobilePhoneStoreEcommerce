using MobilePhoneStoreEcommerce.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence.EntitiyConfigurations
{
    public class CustomerConfiguration : EntityTypeConfiguration<Customer>
    {
        public CustomerConfiguration()
        {
            Property(c => c.Name)
                .IsRequired();
            Property(c => c.PhoneNumber)
                .IsRequired();
            Property(c => c.DeliveryAddress)
                .IsRequired();
            Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(249);

            HasIndex(c => c.Email)
                .IsUnique();

            HasRequired(c => c.Account)
                .WithOptional(a => a.Customer)
                .WillCascadeOnDelete(true);

            HasMany(c => c.Wishlists)
                .WithMany(p => p.Wishlists)
                .Map(m => m.ToTable("Wishlists"));
        }
    }
}