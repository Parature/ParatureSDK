using System;
using System.Collections.Generic;
using System.Text;
using ParatureAPI;

namespace Exercises
{
    class Exercise10DeleteObjects
    {
        public static bool TrashCustomer(long customerID)
        {
            bool isSuccess;

            var trashResponse = ApiHandler.Customer.CustomerDelete(customerID, CredentialProvider.Creds, false);

            isSuccess = !trashResponse.HasException;

            return isSuccess;
        }

        public static bool PurgeCustomer(long customerID)
        {
            bool isSuccess;

            var purgeResponse = ApiHandler.Customer.CustomerDelete(customerID, CredentialProvider.Creds, true);

            isSuccess = !purgeResponse.HasException;

            return isSuccess;
        }
    }
}
