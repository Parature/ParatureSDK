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
        public static Customer getCustomer(long customerID)
        {
            var parature = new ParaService(CredentialProvider.Creds);
            return parature.GetDetails<Customer>(customerID);
        }

        public static Account getAccount(long accountID)
        {
            var parature = new ParaService(CredentialProvider.Creds);
            return parature.GetDetails<Account>(accountID);
        }

        public static Article getArticle(long articleID)
        {
            var parature = new ParaService(CredentialProvider.Creds);
            return parature.GetDetails<Article>(articleID);
        }

    }
}
