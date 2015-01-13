namespace ParatureAPI.ParaObjects
{
    public class CustomerStatus : Status
    {
        public string Description = "";
        public string Text = "";

        public CustomerStatus()
        {
        }

        public CustomerStatus(CustomerStatus customerStatus)
            : base(customerStatus)
        {
            Description = customerStatus.Description;
            Text = customerStatus.Text;
            Name = customerStatus.Name;
            StatusID = customerStatus.StatusID;
        }
    }
}