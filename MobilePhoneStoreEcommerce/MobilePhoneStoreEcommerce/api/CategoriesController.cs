using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.Domain;
using MobilePhoneStoreEcommerce.Core.Dtos;
using MobilePhoneStoreEcommerce.Core.Services;
using MobilePhoneStoreEcommerce.Persistence;
using MobilePhoneStoreEcommerce.Persistence.Consts;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace MobilePhoneStoreEcommerce.api
{
    public class CategoriesController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountAuthentication _accountAuthentication;
        public CategoriesController(IUnitOfWork unitOfWork, IAccountAuthentication accountAuthentication)
        {
            this._unitOfWork = unitOfWork;
            this._accountAuthentication = accountAuthentication;
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
            if (!IsAuthorized())
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var category = categoryDto.CreateModel();

            this._unitOfWork.Categories.Add(category);
            this._unitOfWork.Complete();

            return new CategoryDto(category);
        }

        [HttpPut]
        public CategoryDto Update(CategoryDto categoryDto)
        {
            if (!IsAuthorized())
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

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
            if (!IsAuthorized())
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var categoryInDb = this._unitOfWork.Categories.Get(id);

            if (categoryInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            this._unitOfWork.Categories.Remove(categoryInDb);
            this._unitOfWork.Complete();
        }

        private bool IsAuthorized()
        {
            var sessionAdminID = HttpContext.Current.Session[SessionNames.AdminID];

            if (!this._accountAuthentication.IsAuthentic(sessionAdminID))
                return false;

            return true;
        }
    }
}
