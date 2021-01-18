using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MobilePhoneStoreEcommerce.Core.Dtos
{
    public class ProductDto
    {
        public ProductDto()
        {
            this.SpecificationValuesDto = new List<SpecificationValueDto>();
        }
        public int ProductID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Price { get; set; }

        [Required]
        public int ProducerID { get; set; }
        public string ProducerName { get; set; }

        [Required]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public virtual ICollection<SpecificationValueDto> SpecificationValuesDto { get; set; }

        [Required]
        public byte[] Avatar { get; set; }
    }
}