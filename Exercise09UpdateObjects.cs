using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureSDK;

namespace Exercises
{
    class Exercise09UpdateObjects
    {
        public static bool UpdateCustomer(ParaObjects.Customer modifiedCustomer) {
            //Customer object would be modifed before calling this method

            bool isSuccess;

            var updateResponse = ParatureSDK.ApiHandler.Customer.CustomerUpdate(modifiedCustomer, CredentialProvider.Creds);

            isSuccess = !updateResponse.HasException;

            return isSuccess;
        }

        public static bool UpdateAccount(ParaObjects.Account modifiedAccount)
        {
            bool isSuccess;

            var updateResponse = ParatureSDK.ApiHandler.Account.AccountUpdate(modifiedAccount, CredentialProvider.Creds);

            isSuccess = !updateResponse.HasException;

            return isSuccess;
        }

        //Article and ArticleFolder methods would look exactly the same as the above, just using different objects
    }
}
