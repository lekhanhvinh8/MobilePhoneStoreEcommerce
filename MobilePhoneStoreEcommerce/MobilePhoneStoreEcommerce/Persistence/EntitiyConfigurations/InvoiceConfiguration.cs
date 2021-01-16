using MobilePhoneStoreEcommerce.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence.EntitiyConfigurations
{
    public class InvoiceConfiguration : EntityTypeConfiguration<Invoice>
    {
        public InvoiceConfiguration()
        {
            Property(i => i.ID).HasMaxLength(249);
            Property(i => i.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);


            HasRequired(i => i.Order)
                .WithOptional(o => o.Invoice)
                .Map(m => m.MapKey("OrderID"));
        }
    }
}