using System;
using System.Collections;
using System.Threading;
using System.Xml;
using ParatureSDK.ApiHandler.ApiMethods;
using ParatureSDK.EntityQuery;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Customer module.
    /// </summary>
    public class Customer : FirstLevelApiGetMethods<ParaObjects.Customer, CustomerQuery>
    {
        private static ParaEnums.ParatureModule _module = ParaEnums.ParatureModule.Customer;

        /// <summary>
        /// Creates a Parature Customer. Requires an Object and a credentials object. Need to specific whether to include a customer notification for the insert, as well as if the password should be included in the Notificaiton. 
        /// Will return the Newly Created Customerid. Returns 0 if the Customer creation fails.
        /// </summary>
        public static ApiCallResponse Insert(ParaObjects.Customer customer, ParaCredentials ParaCredentials, bool NotifyCustomer, bool IncludePasswordInNotification)
        {
            // Extra arguments for the customer module
            var arguments = new ArrayList
            {
                "_notify_=" + NotifyCustomer.ToString().ToLower()
            };

            // Only if the user selects to notify the customer that we include the password.
            if (NotifyCustomer)
            {
                // This is currently a bug. Needs to be uncommented when fixed.
                arguments.Add("_includePassword_=" + IncludePasswordInNotification.ToString().ToLower());
            }

            var doc = XmlGenerator.GenerateXml(customer);
            var ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Customer, doc, 0, arguments);
            customer.Id = ar.Id;
            return ar;
        }

        /// <summary>
        /// Updates a Parature Customer. Requires an Object and a credentials object. Need to specific whether to include a customer notification for the insert, as well as if the password should be included in the Notificaiton. Will return the updated Customerid. Returns 0 if the Customer update operation fails.
        /// </summary>
        public static ApiCallResponse Update(ParaObjects.Customer Customer, ParaCredentials ParaCredentials, bool NotifyCustomer, bool IncludePasswordInNotification)
        {
            // Extra arguments for the customer module
            var arguments = new ArrayList();
            arguments.Add("_notify_=" + NotifyCustomer.ToString().ToLower());
            // Only if the user selects to notify the customer that we include the password.
            if (NotifyCustomer == true)
            {
                //arguments.Add("_include_password_=" + IncludePasswordInNotification.ToString().ToLower());
            }
            var ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Customer, XmlGenerator.GenerateXml(Customer), Customer.Id, arguments);
            return ar;
        }

        public class Role : SecondLevelApiMethods<ParaObjects.CustomerRole, RoleQuery, ParaObjects.Customer>
        {}

        public class Status : SecondLevelApiMethods<ParaObjects.Status, StatusQuery, ParaObjects.Customer>
        {}

        public class View : SecondLevelApiMethods<ParaObjects.View, ViewQuery, ParaObjects.Customer>
        {}

    }
}