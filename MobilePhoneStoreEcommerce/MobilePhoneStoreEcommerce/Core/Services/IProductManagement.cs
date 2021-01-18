using MobilePhoneStoreEcommerce.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePhoneStoreEcommerce.Core.Services
{
    public interface IProductManagement
    {
        void Create(ProductDto productDto, byte[] avatar);
    }
}
