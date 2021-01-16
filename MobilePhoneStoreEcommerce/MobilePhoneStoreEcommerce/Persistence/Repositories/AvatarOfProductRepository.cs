using MobilePhoneStoreEcommerce.Core.Domain;
using MobilePhoneStoreEcommerce.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence.Repositories
{
    public class AvatarOfProductRepository : Repository<AvatarOfProduct>, IAvatarOfProductRepository
    {
        public AvatarOfProductRepository(ApplicationDbContext context)
            :base(context)
        {

        }
    }
}