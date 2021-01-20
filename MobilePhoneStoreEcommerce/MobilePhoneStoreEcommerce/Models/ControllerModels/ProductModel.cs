using PagedList;
using MobilePhoneStoreEcommerce.Core.Domain;
using MobilePhoneStoreEcommerce.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Models.ControllerModels
{
    public class ProductModel   
    {
        private ApplicationDbContext _context;

        public ProductModel()
        {
            _context = new ApplicationDbContext();
        }
        public IEnumerable<Product> allAvailableProducts(int page, int pagesize)
        {
            return _context.Products.Where(x => x.Status == true).OrderBy(x => x.Name).ToPagedList(page, pagesize);
        }
    }
}