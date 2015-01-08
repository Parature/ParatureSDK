using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public partial class CustomerStatusList : PagedData.PagedData
    {
        public List<CustomerStatus> CustomerStatuses = new List<CustomerStatus>();
        public CustomerStatusList()
        {
        }
        public CustomerStatusList(CustomerStatusList Customerstatuslist)
            : base(Customerstatuslist)
        {
            this.CustomerStatuses = new List<CustomerStatus>(Customerstatuslist.CustomerStatuses);
        }
    }
}