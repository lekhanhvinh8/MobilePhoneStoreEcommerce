using MobilePhoneStoreEcommerce.Core.Domain;
using System.Linq;

namespace MobilePhoneStoreEcommerce.Core.Dtos
{
    public class SpecificationValueDto
    {
        public SpecificationValueDto()
        {
        }
        public SpecificationValueDto(SpecificationValue specificationValue)
        {
            this.SpecificationID = specificationValue.ProductSpecificationID;
            this.Value = specificationValue.Value;

            if (specificationValue.Products.Count() == 0)
                this.IsHavingProduct = false;
            else
                this.IsHavingProduct = true;
        }
        public int SpecificationID { get; set; }
        public string Value { get; set; }
        public bool IsHavingProduct { get; set; }
        public SpecificationValue CreateModel()
        {
            var specificationValue = new SpecificationValue();
            specificationValue.ProductSpecificationID = this.SpecificationID;
            specificationValue.Value = this.Value;

            return specificationValue;
        }
    }
}