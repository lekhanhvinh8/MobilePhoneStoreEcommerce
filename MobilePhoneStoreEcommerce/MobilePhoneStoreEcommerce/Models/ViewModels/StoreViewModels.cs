using MobilePhoneStoreEcommerce.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Models.ViewModels
{
    public class StoreViewModels
    {
        public PagedList.IPagedList<Product> allAvailableProducts { set; get; }
        public IEnumerable<Producer> allProducers { get; set; }
    }
}