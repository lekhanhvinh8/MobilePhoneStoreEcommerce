using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.Repositories;
using MobilePhoneStoreEcommerce.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            this._context = context;
            Categories = new CategoryRepository(_context);
            Producers = new ProducerRepository(_context);
            Roles = new RoleRepository(_context);
            Accounts = new AccountRepository(_context);
            AvatarOfProducts = new AvatarOfProductRepository(_context);
            Carts = new CartRepository(_context);
            Comments = new CommentRepository(_context);
            Customers = new CustomerRepository(_context);
            Invoices = new InvoiceRepository(_context);
            Orders = new OrderRepository(_context);
            Products = new ProductRepository(_context);
            ProductsOfOrders = new ProductsOfOrderRepository(_context);
            ProductSpecifications = new ProductSpecificationRepository(_context);
            SpecificationValues = new SpecificationValueRepository(_context);
            StarRatings = new StarRatingRepository(_context);
            Sellers = new SellerRepository(_context);
        }

        public ICategoryRepository Categories { get; private set; }

        public IProducerRepository Producers { get; private set; }

        public IRoleRepository Roles { get; private set; }

        public IAccountRepository Accounts { get; private set; }

        public IAvatarOfProductRepository AvatarOfProducts { get; private set; }

        public ICartRepository Carts { get; private set; }

        public ICommentRepository Comments { get; private set; }

        public ICustomerRepository Customers { get; private set; }

        public IInvoiceRepository Invoices { get; private set; }

        public IOrderRepository Orders { get; private set; }

        public IProductRepository Products { get; private set; }

        public IProductsOfOrderRepository ProductsOfOrders { get; private set; }

        public IProductSpecificationRepository ProductSpecifications { get; private set; }

        public ISpecificationValueRepository SpecificationValues { get; private set; }

        public IStarRatingRepository StarRatings { get; private set; }
        public ISellerRepository Sellers { get; private set; }

        public int Complete()
        {
            return this._context.SaveChanges();
        }

        public void Dispose()
        {
            this._context.Dispose();
        }
    }
}