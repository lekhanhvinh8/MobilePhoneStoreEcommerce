using MobilePhoneStoreEcommerce.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence.EntitiyConfigurations
{
    public class SpecificationValueConfiguration : EntityTypeConfiguration<SpecificationValue>
    {
        public SpecificationValueConfiguration()
        {
            HasKey(s => new { s.ProductSpecificationID, s.Value});

            HasRequired(s => s.ProductSpecification)
                .WithMany(p => p.SpecificationValues)
                .HasForeignKey(s => s.ProductSpecificationID)
                .WillCascadeOnDelete(false);
        }
    }
}