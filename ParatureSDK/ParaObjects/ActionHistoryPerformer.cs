using System.Xml.Serialization;
using ParatureSDK.ParaObjects.EntityReferences;

namespace ParatureSDK.ParaObjects
{
    /// <summary>
    /// Indicates who performed an action history item, whether a CSR or a Customer and includes the id and name of the performer.
    /// </summary>
    public class ActionHistoryPerformer
    {
        [XmlAttribute("performer-type")]
        public ParaEnums.ActionHistoryPerformerType ActionHistoryPerformerType = ParaEnums.ActionHistoryPerformerType.System;

        // Will be loaded with the CSR id and name, only if it was a CSR that performed the action.
        [XmlElement("Csr")]
        public Csr CsrPerformer;

        // Will be loaded with the Customer id and name, only if it was a Customer that performed the action.
        [XmlElement("Customer")]
        public Customer CustomerPerformer;

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
                    return CustomerPerformer.Full_Name;
                case ParaEnums.ActionHistoryPerformerType.Rule:
                    return "Rule";
                case ParaEnums.ActionHistoryPerformerType.System:
                    return "System";
                default:
                    return "";
            }
        }
    }
}