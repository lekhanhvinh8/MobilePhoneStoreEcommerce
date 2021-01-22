﻿using MobilePhoneStoreEcommerce.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePhoneStoreEcommerce.Core.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        IEnumerable<Order> GetAllThenOrderByDate(int sellerID, int status);
        IEnumerable<Order> GetAllThenOrderByDate(int customerID);
    }
}
