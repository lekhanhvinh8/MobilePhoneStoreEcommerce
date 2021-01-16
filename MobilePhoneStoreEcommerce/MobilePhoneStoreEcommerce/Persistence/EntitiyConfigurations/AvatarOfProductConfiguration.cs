using MobilePhoneStoreEcommerce.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence.EntitiyConfigurations
{
    public class AvatarOfProductConfiguration : EntityTypeConfiguration<AvatarOfProduct>
    {
        public AvatarOfProductConfiguration()
        {
            HasKey(a => a.ProductID);

            Property(a => a.Avatar)
                .IsRequired();

            HasRequired(a => a.Product)
                .WithRequiredDependent(p => p.AvatarOfProduct)
                .WillCascadeOnDelete(true);
        }
    }
}