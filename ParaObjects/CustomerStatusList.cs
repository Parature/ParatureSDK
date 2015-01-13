using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public class CustomerStatusList : PagedData.PagedData
    {
        public List<CustomerStatus> CustomerStatuses = new List<CustomerStatus>();
        public CustomerStatusList()
        {
        }
        public CustomerStatusList(CustomerStatusList Customerstatuslist)
            : base(Customerstatuslist)
        {
            CustomerStatuses = new List<CustomerStatus>(Customerstatuslist.CustomerStatuses);
        }
    }
}