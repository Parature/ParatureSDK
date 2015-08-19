using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureSDK;
using ParatureSDK.ParaObjects;

namespace Exercises
{
    class Exercise06RetrieveObjects
    {
        static ParaService Service { get; set; }

        public Exercise06RetrieveObjects()
        {
            Service = new ParaService(CredentialProvider.Creds);
        }

        public static Customer getCustomer(long customerID)
        {
            return Service.GetDetails<Customer>(customerID);
        }

        public static Account getAccount(long accountID)
        {
           return Service.GetDetails<Account>(accountID);
        }

        public static Article getArticle(long articleID)
        {
            return Service.GetDetails<Article>(articleID);
        }

    }
}
