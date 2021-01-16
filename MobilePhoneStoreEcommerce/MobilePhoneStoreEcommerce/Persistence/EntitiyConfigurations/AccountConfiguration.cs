using MobilePhoneStoreEcommerce.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence.EntitiyConfigurations
{
    public class AccountConfiguration : EntityTypeConfiguration<Account>
    {
        public AccountConfiguration()
        {
            Property(a => a.UserName)
                .IsRequired()
                .HasMaxLength(249);
            Property(a => a.PasswordHash)
                .IsRequired();
            Property(a => a.RoleID)
                .IsRequired();

            HasIndex(a => a.UserName)
                .IsUnique();

            HasRequired(a => a.Role)
                .WithMany(r => r.Accounts)
                .HasForeignKey(a => a.RoleID)
                .WillCascadeOnDelete(false);
        }
    }
}