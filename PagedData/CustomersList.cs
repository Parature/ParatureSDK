using System.Collections.Generic;

namespace ParatureAPI.PagedData
{
    /// <summary>
    /// Instantiate this class to hold the result set of a list call to APIs. Whenever you need to get a list of 
    /// Customers
    /// </summary>
    public class CustomersList : PagedData
    {
        public List<ParaObjects.Customer> Customers = new List<ParaObjects.Customer>();

        public CustomersList()
        {
        }

        public CustomersList(CustomersList customersList)
            : base(customersList)
        {
            Customers = new List<ParaObjects.Customer>(customersList.Customers);
        }
    }
}