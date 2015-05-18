using System;
using System.Collections.Generic;
using System.Text;
using ParatureAPI;

namespace Exercises
{
    class Exercise06RetrieveObjects
    {
        public static ParaObjects.Customer getCustomer(long customerID)
        {
            var customer = ApiHandler.Customer.CustomerGetDetails(customerID, CredentialProvider.Creds);

            return customer;
        }

        public static ParaObjects.Account getAccount(long accountID)
        {
            var account = ApiHandler.Account.AccountGetDetails(accountID, CredentialProvider.Creds);

            return account;
        }

        public static ParaObjects.Article getArticle(long articleID)
        {
            var article = ApiHandler.Article.ArticleGetDetails(articleID, CredentialProvider.Creds);

            return article;
        }

    }
}
