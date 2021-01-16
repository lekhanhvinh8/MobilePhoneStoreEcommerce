using MobilePhoneStoreEcommerce.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence.EntitiyConfigurations
{
    public class CommentConfiguration : EntityTypeConfiguration<Comment>
    {
        public CommentConfiguration()
        {
            HasRequired(c => c.Customer)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.CustomerID);

            HasRequired(c => c.Product)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.ProductID);
        }
    }
}