using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureSDK;
using ParatureSDK.ParaObjects;

namespace Exercises
{
    class Exercise09UpdateObjects
    {
        public static bool UpdateCustomer(Customer modifiedCustomer) {
            //Customer object would be modifed before calling this method

            bool isSuccess;

            ParaService.Credentials = CredentialProvider.Creds;
            var updateResponse = ParaService.Update(modifiedCustomer);

            isSuccess = !updateResponse.HasException;

            return isSuccess;
        }

        public static bool UpdateAccount(Account modifiedAccount)
        {
            bool isSuccess;

            ParaService.Credentials = CredentialProvider.Creds;
            var updateResponse = ParaService.Update(modifiedAccount);

            isSuccess = !updateResponse.HasException;

            return isSuccess;
        }

        //Article and ArticleFolder methods would look exactly the same as the above, just using different objects
    }
}
