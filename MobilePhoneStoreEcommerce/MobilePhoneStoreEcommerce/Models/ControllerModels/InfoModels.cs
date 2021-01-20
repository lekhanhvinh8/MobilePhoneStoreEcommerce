using MobilePhoneStoreEcommerce.Core.Domain;
using MobilePhoneStoreEcommerce.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Models.ControllerModels
{
    public class InfoModels
    {
        private ApplicationDbContext _context;

        public InfoModels()
        {
            _context = new ApplicationDbContext();
        }
        public Customer getCustomerById(int id)
        {
            return _context.Cutomers.SingleOrDefault(c => c.ID == id);
        }
        
        public Seller getSellerById(int id)
        {
            return _context.Sellers.SingleOrDefault(c => c.ID == id);
        }

        public bool UpdateCustomer(Customer customer)
        {
            try
            {
                _context.Cutomers.AddOrUpdate(customer);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public bool UpdateSeller(Seller seller)
        {
            try
            {
                _context.Sellers.AddOrUpdate(seller);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}