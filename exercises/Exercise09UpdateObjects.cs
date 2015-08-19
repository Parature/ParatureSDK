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
        static ParaService Service { get; set; }

        public Exercise09UpdateObjects()
        {
            Service = new ParaService(CredentialProvider.Creds);
        }

        public static bool UpdateCustomer(Customer modifiedCustomer) {
            //Customer object would be modifed before calling this method

            bool isSuccess;

            var updateResponse = Service.Update(modifiedCustomer);

            isSuccess = !updateResponse.HasException;

            return isSuccess;
        }

        public static bool UpdateAccount(Account modifiedAccount)
        {
            bool isSuccess;

            var updateResponse = Service.Update(modifiedAccount);

            isSuccess = !updateResponse.HasException;

            return isSuccess;
        }

        //Article and ArticleFolder methods would look exactly the same as the above, just using different objects
    }
}
