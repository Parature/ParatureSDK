namespace ParatureSDK.ParaObjects
{
    /// <summary>
    /// Indicates who performed an action history item, whether a CSR or a Customer and includes the id and name of the performer.
    /// </summary>
    public class ActionHistoryPerformer
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
            ActionHistoryPerformerType = actionHistoryperformer.ActionHistoryPerformerType;
            CsrPerformer = actionHistoryperformer.CsrPerformer;
            CustomerPerformer = actionHistoryperformer.CustomerPerformer;
        }

        public string GetDisplayName()
        {
            switch (ActionHistoryPerformerType)
            {
                case ParaEnums.ActionHistoryPerformerType.Csr:
                    return CsrPerformer.Full_Name;
                case ParaEnums.ActionHistoryPerformerType.Customer:
                    return CustomerPerformer.First_Name + " " + CustomerPerformer.Last_Name;
                case ParaEnums.ActionHistoryPerformerType.System:
                    return "System";
                default:
                    return "";
            }
        }
    }
}