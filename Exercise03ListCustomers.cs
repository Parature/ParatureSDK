using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureSDK;

namespace Exercises
{
    class Exercise03ListCustomers
    {
        //Customers by account and date
        public static ParaObjects.CustomersList getCustomersByAccountAndDate(long acccountID, DateTime date)
        {
            var customerQuery = new ModuleQuery.CustomerQuery();
            customerQuery.RetrieveAllRecords = true;
            customerQuery.AddStaticFieldFilter(ModuleQuery.CustomerQuery.CustomerStaticFields.AccountID, ParaEnums.QueryCriteria.Equal, acccountID.ToString());
            //Use a custom filter if correct enums aren't available
            customerQuery.AddCustomFilter(String.Format("Date_Created_max_={0}/{1}/{2}", date.Month, date.Day, date.Year));

            var customers = ParatureSDK.ApiHandler.Customer.CustomersGetList(CredentialProvider.Creds, customerQuery);

            return customers;
        }

        public static ParaObjects.CustomersList getCustomersByEmail(string email)
        {
            var customerQuery = new ModuleQuery.CustomerQuery();
            customerQuery.RetrieveAllRecords = true;
            customerQuery.AddStaticFieldFilter(ModuleQuery.CustomerQuery.CustomerStaticFields.CustomerEmail, ParaEnums.QueryCriteria.Equal, email);

            var customers = ParatureSDK.ApiHandler.Customer.CustomersGetList(CredentialProvider.Creds, customerQuery);

            return customers;
        }

        public static ParaObjects.CustomersList getCustomersByStatus(long statusID)
        {
            var customerQuery = new ModuleQuery.CustomerQuery();
            customerQuery.RetrieveAllRecords = true;
            customerQuery.AddStaticFieldFilter(ModuleQuery.CustomerQuery.CustomerStaticFields.Status, ParaEnums.QueryCriteria.Equal, statusID.ToString());

            var customers = ParatureSDK.ApiHandler.Customer.CustomersGetList(CredentialProvider.Creds, customerQuery);

            return customers;
        }

        public static ParaObjects.CustomersList getCustomersAndOrderByLastName()
        {
            var customerQuery = new ModuleQuery.CustomerQuery();
            customerQuery.RetrieveAllRecords = true;
            customerQuery.AddSortOrder(ModuleQuery.CustomerQuery.CustomerStaticFields.LastName, ParaEnums.QuerySortBy.Asc);

            var customers = ParatureSDK.ApiHandler.Customer.CustomersGetList(CredentialProvider.Creds, customerQuery);

            return customers;
        }
    }
}
