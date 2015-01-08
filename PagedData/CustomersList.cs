using System.Collections.Generic;
using ParatureAPI.ParaObjects;

namespace ParatureAPI.PagedData
{
    /// <summary>
    /// Instantiate this class to hold the result set of a list call to APIs. Whenever you need to get a list of 
    /// Customers
    /// </summary>
    public class CustomersList : PagedData
    {
        public List<Customer> Customers = new List<Customer>();

        public CustomersList()
        {
        }

        public CustomersList(CustomersList customersList)
            : base(customersList)
        {
            Customers = new List<Customer>(customersList.Customers);
        }
    }
}