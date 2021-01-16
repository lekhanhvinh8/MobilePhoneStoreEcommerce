
namespace MobilePhoneStoreEcommerce.Core.Domain
{
    using System;
    using System.Collections.Generic;

    public partial class SpecificationValue
    {
        public SpecificationValue()
        {
            this.Products = new List<Product>();
        }

        public int ProductSpecificationID { get; set; }
        public string Value { get; set; }

        public virtual ProductSpecification ProductSpecification { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
