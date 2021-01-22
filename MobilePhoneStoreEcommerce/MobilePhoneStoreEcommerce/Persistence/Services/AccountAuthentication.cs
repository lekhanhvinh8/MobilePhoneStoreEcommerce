using MobilePhoneStoreEcommerce.Core;
using MobilePhoneStoreEcommerce.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreEcommerce.Persistence.Services
{
    public class AccountAuthentication : IAccountAuthentication
    {
        private IUnitOfWork _unitOfWork;
        public AccountAuthentication(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public bool IsAuthentic(int accountID, object sessionAccountID)
        {
            if (sessionAccountID == null)
                return false;

            try
            {
                var accountIDInSession = Convert.ToInt32(sessionAccountID);

                var account = this._unitOfWork.Accounts.Get(accountIDInSession);
                if (account == null)
                    return false;

                if (accountIDInSession != accountID)
                return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsAuthentic(object sessionAccountID)
        {
            if (sessionAccountID == null)
                return false;

            try
            {
                var accountIDInSession = Convert.ToInt32(sessionAccountID);

                var account = this._unitOfWork.Accounts.Get(accountIDInSession);
                if (account == null)
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /*
        public bool IsAuthorized(int accountID, int roleID)
        {
            var account = this._unitOfWork.Accounts.Get(accountID);

            if (account == null)
                return false;

            if (account.RoleID != roleID)
                return false;

            return true;
        }
        */
    }
}