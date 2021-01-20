using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePhoneStoreEcommerce.Core.Services
{
    public interface IAccountAuthentication
    {
        bool IsAuthentic(object sessionAccountID);
        bool IsAuthentic(int accountID, object sessionAccountID);
        //bool IsAuthorized(int accountID, int roleID);
    }
}
