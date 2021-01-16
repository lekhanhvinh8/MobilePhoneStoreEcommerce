using MobilePhoneStoreEcommerce.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence.EntitiyConfigurations
{
    public class StartRatingConfiguration : EntityTypeConfiguration<StarRating>
    {
        public StartRatingConfiguration()
        {
            HasKey(s => new { s.CustomerID, s.ProductID });

            HasRequired(s => s.Customer)
                .WithMany(c => c.StarRatings)
                .HasForeignKey(s => s.CustomerID);

            HasRequired(s => s.Product)
                .WithMany(p => p.StarRatings)
                .HasForeignKey(s => s.ProductID);
        }
    }
}