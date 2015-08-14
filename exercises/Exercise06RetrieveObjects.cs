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
        public Exercise06RetrieveObjects()
        {
            ParaService.Credentials = CredentialProvider.Creds;
        }

        public static Customer getCustomer(long customerID)
        {
            return ParaService.GetDetails<Customer>(customerID);
        }

        public static Account getAccount(long accountID)
        {
           return ParaService.GetDetails<Account>(accountID);
        }

        public static Article getArticle(long articleID)
        {
            return ParaService.GetDetails<Article>(articleID);
        }

    }
}
