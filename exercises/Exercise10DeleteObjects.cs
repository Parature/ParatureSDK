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
        static ParaService Service { get; set; }

        public Exercise10DeleteObjects()
        {
            Service = new ParaService(CredentialProvider.Creds);
        }

        public static bool TrashCustomer(long customerID)
        {
            bool isSuccess;

            var trashResponse = Service.Delete<Customer>(customerID, false);

            isSuccess = !trashResponse.HasException;

            return isSuccess;
        }

        public static bool PurgeCustomer(long customerID)
        {
            bool isSuccess;

            Service = new ParaService(CredentialProvider.Creds);
            var purgeResponse = Service.Delete<Customer>(customerID, true);

            isSuccess = !purgeResponse.HasException;

            return isSuccess;
        }
    }
}
