
namespace MobilePhoneStoreEcommerce.Core.Domain
{
    using System;
    using System.Collections.Generic;

    public partial class ProductSpecification
    {
        public ProductSpecification()
        {
            this.SpecificationValues = new List<SpecificationValue>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual List<SpecificationValue> SpecificationValues { get; set; }
    }
}
