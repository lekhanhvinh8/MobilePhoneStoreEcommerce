using MobilePhoneStoreEcommerce.Core.Domain;
using MobilePhoneStoreEcommerce.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context)
            :base(context)
        {

        }
        public IEnumerable<Order> GetAllThenOrderByDate(int sellerID, int status)
        {
            var orders = this.Context.Set<Order>()
                .Where(o => o.SellerID == sellerID && o.Status == status)
                .OrderByDescending(o => o.OrderTime)
                .ToList();

            return orders;
        }
        public IEnumerable<Order> GetAllThenOrderByDate(int customerID)
        {
            var orders = this.Context.Set<Order>()
                .Where(o => o.CustomerID == customerID)
                .OrderByDescending(o => o.OrderTime)
                .ToList();

            return orders;
        }

        public IEnumerable<Order> GetAllThenOrderByDate(Expression<Func<Order, bool>> predicate)
        {
            var orders = this.Context.Set<Order>()
                .Where(predicate)
                .OrderByDescending(o => o.OrderTime)
                .ToList();

            return orders;
        }
    }
}