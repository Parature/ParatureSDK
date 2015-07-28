using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureSDK;
using ParatureSDK.ParaObjects;

namespace Exercises
{
    class Exercise10DeleteObjects
    {
        public static bool TrashCustomer(long customerID)
        {
            bool isSuccess;

            var parature = new ParaService(CredentialProvider.Creds);
            var trashResponse = parature.Delete<Customer>(customerID, false);

            isSuccess = !trashResponse.HasException;

            return isSuccess;
        }

        public static bool PurgeCustomer(long customerID)
        {
            bool isSuccess;

            var parature = new ParaService(CredentialProvider.Creds);
            var purgeResponse = parature.Delete<Customer>(customerID, true);

            isSuccess = !purgeResponse.HasException;

            return isSuccess;
        }
    }
}
