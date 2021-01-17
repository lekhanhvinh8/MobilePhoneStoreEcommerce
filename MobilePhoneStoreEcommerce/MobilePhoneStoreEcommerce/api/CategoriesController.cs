using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.Domain;
using MobilePhoneStoreEcommerce.Core.Dtos;
using MobilePhoneStoreEcommerce.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace MobilePhoneStoreEcommerce.api
{
    public class CategoriesController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoriesController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public List<CategoryDto> GetAll()
        {
            var categories = this._unitOfWork.Categories.GetAll();
            var categoriesDto = new List<CategoryDto>();

            foreach (var category in categories)
            {
                var categoryDto = new CategoryDto(category);
                categoriesDto.Add(categoryDto);
            }
            return categoriesDto;
        }

        public CategoryDto Get(int iD)
        {
            var category = this._unitOfWork.Categories.Get(iD);

            if (category == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return new CategoryDto(category);
        }

        [HttpPost]
        public CategoryDto Create(CategoryDto categoryDto)
        {
            var category = categoryDto.CreateModel();

            this._unitOfWork.Categories.Add(category);
            this._unitOfWork.Complete();

            return new CategoryDto(category);
        }

        [HttpPut]
        public CategoryDto Update(CategoryDto categoryDto)
        {
            var categoryInDb = this._unitOfWork.Categories.Get(categoryDto.CategoryID);

            if(categoryInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            categoryDto.Update(categoryInDb);

            this._unitOfWork.Complete();

            return new CategoryDto(categoryInDb);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            var categoryInDb = this._unitOfWork.Categories.Get(id);

            if (categoryInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            this._unitOfWork.Categories.Remove(categoryInDb);
            this._unitOfWork.Complete();
        }
    }
}
