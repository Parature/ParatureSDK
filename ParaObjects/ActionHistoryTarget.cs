using System.Xml.Serialization;
using ParatureSDK.ParaObjects.EntityReferences;

namespace ParatureSDK.ParaObjects
{
    /// <summary>
    /// Indicates who performed an action history item, whether a CSR or a Customer and includes the id and name of the performer.
    /// </summary>
    public class ActionHistoryTarget
    {
        [XmlAttribute("target-type")]
        public ParaEnums.ActionHistoryPerformerType ActionHistoryTargetType = ParaEnums.ActionHistoryPerformerType.System;

        // Will be loaded with the CSR id and name, only if it was a CSR that performed the action.
        [XmlElement("Csr")]
        public Csr CsrPerformer = new Csr();

        // Will be loaded with the Customer id and name, only if it was a Customer that performed the action.
        [XmlElement("Customer")]
        public Customer CustomerPerformer = new Customer();

        public ActionHistoryTarget()
        {
        }

        public ActionHistoryTarget(ActionHistoryTarget actionHistoryperformer)
        {
            ActionHistoryTargetType = actionHistoryperformer.ActionHistoryTargetType;
            CsrPerformer = actionHistoryperformer.CsrPerformer;
            CustomerPerformer = actionHistoryperformer.CustomerPerformer;
        }

        public string GetDisplayName()
        {
            switch (ActionHistoryTargetType)
            {
                case ParaEnums.ActionHistoryPerformerType.Csr:
                    return CsrPerformer.Full_Name;
                case ParaEnums.ActionHistoryPerformerType.Customer:
                    return CustomerPerformer.Full_Name;
                case ParaEnums.ActionHistoryPerformerType.System:
                    return "System";
                default:
                    return "";
            }
        }
    }
}