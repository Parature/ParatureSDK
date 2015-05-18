using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureSDK;

namespace Exercises
{
    class Exercise10DeleteObjects
    {
        public static bool TrashCustomer(long customerID)
        {
            bool isSuccess;

            var trashResponse = ParatureSDK.ApiHandler.Customer.CustomerDelete(customerID, CredentialProvider.Creds, false);

            isSuccess = !trashResponse.HasException;

            return isSuccess;
        }

        public static bool PurgeCustomer(long customerID)
        {
            bool isSuccess;

            var purgeResponse = ParatureSDK.ApiHandler.Customer.CustomerDelete(customerID, CredentialProvider.Creds, true);

            isSuccess = !purgeResponse.HasException;

            return isSuccess;
        }
    }
}
