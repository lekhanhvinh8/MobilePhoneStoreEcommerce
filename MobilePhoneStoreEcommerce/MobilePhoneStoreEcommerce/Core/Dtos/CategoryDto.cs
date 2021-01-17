using MobilePhoneStoreEcommerce.Core.Domain;
using System.Linq;

namespace MobilePhoneStoreEcommerce.Core.Dtos
{
    public class CategoryDto
    {
        private Category _category;
        public CategoryDto()
        {
            _category = new Category();
        }
        public CategoryDto(Category category)
        {
            if(category != null)
            {
                _category = category;
                this.CategoryID = category.ID;
                this.Name = category.Name;

                if (category.Products.Count() == 0)
                    IsHavingProduct = false;
                else
                    IsHavingProduct = true;
            }
        }
        public Category CreateModel()
        {
            var category = new Category();

            category.Name = this.Name;

            return category;
        }

        public void Update(Category category)
        {
            category.Name = this.Name;
        }
        
        public int CategoryID { get; set; }
        public string Name { get; set; }

        public bool IsHavingProduct { get; set; }
    }
}