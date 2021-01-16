
using System.Collections.Generic;

namespace MobilePhoneStoreEcommerce.Core.Domain
{
    public partial class AvatarOfProduct
    {
        public AvatarOfProduct()
        {
            this.Images = new List<byte[]>();
        }
        public int ProductID { get; set; }
        public byte[] Avatar { get; set; }
        public List<byte[]> Images { get; set; }
        public virtual Product Product { get; set; }
    }
}
