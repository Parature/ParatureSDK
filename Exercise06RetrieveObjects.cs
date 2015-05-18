using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureSDK;

namespace Exercises
{
    class Exercise06RetrieveObjects
    {
        public static ParaObjects.Customer getCustomer(long customerID)
        {
            var customer = ParatureSDK.ApiHandler.Customer.CustomerGetDetails(customerID, CredentialProvider.Creds);

            return customer;
        }

        public static ParaObjects.Account getAccount(long accountID)
        {
            var account = ParatureSDK.ApiHandler.Account.AccountGetDetails(accountID, CredentialProvider.Creds);

            return account;
        }

        public static ParaObjects.Article getArticle(long articleID)
        {
            var article = ParatureSDK.ApiHandler.Article.ArticleGetDetails(articleID, CredentialProvider.Creds);

            return article;
        }

    }
}
