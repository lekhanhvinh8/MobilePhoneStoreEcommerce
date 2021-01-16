using MobilePhoneStoreEcommerce.Core.Domain;
using MobilePhoneStoreEcommerce.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence.Repositories
{
    public class StarRatingRepository : Repository<StarRating>, IStarRatingRepository
    {
        public StarRatingRepository(ApplicationDbContext context)
            :base(context)
        {

        }
    }
}