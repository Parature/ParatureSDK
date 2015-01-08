namespace ParatureAPI.ParaObjects
{
    public partial class CustomerStatus : Status
    {
        public string Description = "";
        public string Text = "";

        public CustomerStatus()
        {
        }

        public CustomerStatus(CustomerStatus customerStatus)
            : base(customerStatus)
        {
            this.Description = customerStatus.Description;
            this.Text = customerStatus.Text;
            this.Name = customerStatus.Name;
            this.StatusID = customerStatus.StatusID;
        }
    }
}