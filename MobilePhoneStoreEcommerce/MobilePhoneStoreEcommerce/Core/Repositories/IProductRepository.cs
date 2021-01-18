using MobilePhoneStoreEcommerce.Core.Domain;
using MobilePhoneStoreEcommerce.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePhoneStoreEcommerce.Core.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        void Create(ProductForSellerDto productForSellerDto, byte[] avatar);
    }
}
