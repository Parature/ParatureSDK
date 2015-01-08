namespace ParatureAPI.ParaObjects
{
    /// <summary>
    /// Indicates who performed an action history item, whether a CSR or a Customer and includes the id and name of the performer.
    /// </summary>
    public partial class ActionHistoryPerformer
    {
        public ParaEnums.ActionHistoryPerformerType ActionHistoryPerformerType = ParaEnums.ActionHistoryPerformerType.System;
        // Will be loaded with the CSR id and name, only if it was a CSR that performed the action.
        public Csr CsrPerformer = new Csr();

        // Will be loaded with the Customer id and name, only if it was a Customer that performed the action.
        public Customer CustomerPerformer = new Customer();

        public ActionHistoryPerformer()
        {
        }
        public ActionHistoryPerformer(ActionHistoryPerformer actionHistoryperformer)
        {
            this.ActionHistoryPerformerType = actionHistoryperformer.ActionHistoryPerformerType;
            this.CsrPerformer = actionHistoryperformer.CsrPerformer;
            this.CustomerPerformer = actionHistoryperformer.CustomerPerformer;
        }

        public string getDisplayName()
        {
            if (this.ActionHistoryPerformerType == ParaEnums.ActionHistoryPerformerType.Csr)
            {
                return this.CsrPerformer.Full_Name;
            }
            else if (this.ActionHistoryPerformerType == ParaEnums.ActionHistoryPerformerType.Customer)
            {
                return this.CustomerPerformer.First_Name + " " + this.CustomerPerformer.Last_Name;
            }
            else if (this.ActionHistoryPerformerType == ParaEnums.ActionHistoryPerformerType.System)
            {
                return "System";
            }
            else
            {
                return "";
            }
        }

    }
}