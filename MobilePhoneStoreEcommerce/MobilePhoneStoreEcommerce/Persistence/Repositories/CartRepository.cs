using MobilePhoneStoreEcommerce.Core.Domain;
using MobilePhoneStoreEcommerce.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context)
            :base(context)
        {

        }
        public float TotalMoneyInCart()
        {
            var total = this.Context.Set<Cart>().Sum(c => (double?)c.Product.Price * c.Amount) ?? 0f;
            return (float)total;
        }
    }
}