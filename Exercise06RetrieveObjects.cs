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
            var customer = ParatureSDK.ApiHandler.Customer.GetDetails(customerID, CredentialProvider.Creds);

            return customer;
        }

        public static Account getAccount(long accountID)
        {
            var account = ParatureSDK.ApiHandler.Account.GetDetails(accountID, CredentialProvider.Creds);

            return account;
        }

        public static Article getArticle(long articleID)
        {
            var article = ParatureSDK.ApiHandler.Article.GetDetails(articleID, CredentialProvider.Creds);

            return article;
        }

    }
}
