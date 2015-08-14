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
            ParaService.Credentials = CredentialProvider.Creds;
            return ParaService.GetDetails<Customer>(customerID);
        }

        public static Account getAccount(long accountID)
        {
            ParaService.Credentials = CredentialProvider.Creds;
            return ParaService.GetDetails<Account>(accountID);
        }

        public static Article getArticle(long articleID)
        {
            ParaService.Credentials = CredentialProvider.Creds;
            return ParaService.GetDetails<Article>(articleID);
        }

    }
}
