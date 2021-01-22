using MobilePhoneStoreEcommerce.Core.Domain;
using MobilePhoneStoreEcommerce.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePhoneStoreEcommerce.Core
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        IProducerRepository Producers { get; }
        IRoleRepository Roles { get; }
        IAccountRepository Accounts { get; }
        IAvatarOfProductRepository AvatarOfProducts { get; }
        ICartRepository Carts { get; }
        ICommentRepository Comments { get; }
        ICustomerRepository Customers { get; }
        ISellerRepository Sellers { get; }
        IInvoiceRepository Invoices { get; }
        IOrderRepository Orders { get; }
        IProductRepository Products { get; }
        IProductsOfOrderRepository ProductsOfOrders { get; }
        IProductSpecificationRepository ProductSpecifications { get; }
        ISpecificationValueRepository SpecificationValues { get; }
        IStarRatingRepository StarRatings { get; }

        int Complete();
    }
}
