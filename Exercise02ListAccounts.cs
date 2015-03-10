using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParatureAPI;

namespace Exercises
{
    class Exercise02ListAccounts
    {
        //List all of the accounts
        public static ParaObjects.AccountsList getAllAccounts()
        {
            //Query object is a query string builder
            var accountQuery = new ModuleQuery.AccountQuery();
            accountQuery.RetrieveAllRecords = true; //Retrieve all records will maximize page size and make necessary additional calls to retrieve all pages

            var accounts = ApiHandler.Account.AccountsGetList(CredentialProvider.Creds, accountQuery);

            var apiResponse = accounts.ApiCallResponse; //API response is provided with object

            if (apiResponse.HasException)
            {  
                //Exception handling is a best practice
                Console.Write(apiResponse.ExceptionDetails);
                return null;
            }

            return accounts;
        }

        public static ParaObjects.AccountsList searchAccountsByName(string accountName) {
            var accountQuery = new ModuleQuery.AccountQuery();
            accountQuery.RetrieveAllRecords = true;
            
            //There is a difference between static fields and custom fields
            accountQuery.AddStaticFieldFilter(ModuleQuery.AccountQuery.AccountStaticFields.Accountname, Paraenums.QueryCriteria.Equal, accountName);

            var accounts = ApiHandler.Account.AccountsGetList(CredentialProvider.Creds, accountQuery);

            return accounts;
        }

        public static ParaObjects.AccountsList searchAccountsByField(long fieldID, string fieldValue)
        {
            var accountQuery = new ModuleQuery.AccountQuery();
            accountQuery.RetrieveAllRecords = true;
            accountQuery.AddCustomFieldFilter(fieldID, Paraenums.QueryCriteria.Equal, fieldValue);
            
            var accounts = ApiHandler.Account.AccountsGetList(CredentialProvider.Creds, accountQuery);

            return accounts;
        }

        //Overload for an option field
        public static ParaObjects.AccountsList searchAccountsByField(long fieldID, long fieldValue)
        {
            //field value would be option ID
            var accountQuery = new ModuleQuery.AccountQuery();
            accountQuery.RetrieveAllRecords = true;
            accountQuery.AddCustomFieldFilter(fieldID, Paraenums.QueryCriteria.Equal, fieldValue);

            var accounts = ApiHandler.Account.AccountsGetList(CredentialProvider.Creds, accountQuery);

            return accounts;
        }

        public static ParaObjects.AccountsList getAccountsByView(long viewID)
        {
            //field value would be option ID
            var accountQuery = new ModuleQuery.AccountQuery();
            accountQuery.RetrieveAllRecords = true;
            accountQuery.View = viewID;

            var accounts = ApiHandler.Account.AccountsGetList(CredentialProvider.Creds, accountQuery);

            return accounts;
        }
    }

    
}
