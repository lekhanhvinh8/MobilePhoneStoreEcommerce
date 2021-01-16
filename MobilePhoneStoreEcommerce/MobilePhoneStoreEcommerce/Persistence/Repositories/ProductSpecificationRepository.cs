using MobilePhoneStoreEcommerce.Core.Domain;
using MobilePhoneStoreEcommerce.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence.Repositories
{
    public class ProductSpecificationRepository : Repository<ProductSpecification>, IProductSpecificationRepository
    {
        public ProductSpecificationRepository(ApplicationDbContext context)
            :base(context)
        {

        }
    }
}