using MobilePhoneStoreEcommerce.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence.EntitiyConfigurations
{
    public class ProducerConfiguration : EntityTypeConfiguration<Producer>
    {
        public ProducerConfiguration()
        {
            Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(249);

            HasIndex(p => p.Name)
                .IsUnique();
        }
    }
}