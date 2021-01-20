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
        
        public PagedList.IPagedList<Product> allAvailableProducts(int page, int pagesize)
        {
            return _context.Products.Where(x => x.Status == true).OrderBy(x => x.Name).ToPagedList(page, pagesize);
        }

        public Product productDetail(int proId)
        {
            return _context.Products.SingleOrDefault(x => x.ID == proId);
        }

        public IEnumerable<Producer> allProducers()
        {
            return _context.Producers.ToList();
        }

        public PagedList.IPagedList<Product> allAvailableProductsOfProducer(int producerId, int page, int pagesize)
        {
            return _context.Products.Where(x => x.Status == true && x.ProducerID == producerId)
                .OrderBy(x => x.Name)
                .ToPagedList(page, pagesize);
        }

        public PagedList.IPagedList<Product> allAvailableProductsOfSearch(string search, int page, int pagesize)
        {
            return _context.Products.Where(x => x.Status == true && x.Name.Contains(search))
                .OrderBy(x => x.Name)
                .ToPagedList(page, pagesize);
        }

        public List<Category> allCategories()
        {
            return _context.Categories.ToList();
        }

        public PagedList.IPagedList<Product> allAvailableProductsOfCategory(int categoryId, int page, int pagesize)
        {
            return _context.Products.Where(x => x.Status == true && x.CategoryID == categoryId)
                .OrderBy(x => x.Name)
                .ToPagedList(page, pagesize);
        }

        public string getCatNameById(int catId)
        {
            var cat = _context.Categories.SingleOrDefault(c => c.ID == catId);
            return cat.Name;
        }

        public string getProducerNameById(int producerId)
        {
            var producer = _context.Producers.SingleOrDefault(c => c.ID == producerId);
            return producer.Name;
        }
    }
}