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
        public ParaEnums.ActionHistoryTargetType ActionHistoryTargetType = ParaEnums.ActionHistoryTargetType.System;

        // Will be loaded with the CSR id and name, only if it was a CSR that performed the action.
        [XmlElement("Csr")]
        public Csr CsrTarget;

        // Will be loaded with the Customer id and name, only if it was a Customer that performed the action.
        [XmlElement("Customer")]
        public Customer CustomerTarget;

        // Will be loaded with the Queue id and name, only if it was performed on a Queue
        [XmlElement("Queue")]
        public Queue QueueTarget;

        // Will be loaded with the Queue id and name, only if it was performed on a Queue
        [XmlElement("Download")]
        public ChatHistoryDownload DownloadTarget;

        public ActionHistoryTarget()
        {
        }

        public ActionHistoryTarget(ActionHistoryTarget actionHistoryTarget)
        {
            ActionHistoryTargetType = actionHistoryTarget.ActionHistoryTargetType;
            CsrTarget = actionHistoryTarget.CsrTarget;
            CustomerTarget = actionHistoryTarget.CustomerTarget;
            QueueTarget = actionHistoryTarget.QueueTarget;
        }

        public string GetDisplayName()
        {
            switch (ActionHistoryTargetType)
            {
                case ParaEnums.ActionHistoryTargetType.Csr:
                    return CsrTarget.Full_Name;
                case ParaEnums.ActionHistoryTargetType.Customer:
                    return CustomerTarget.Full_Name;
                case ParaEnums.ActionHistoryTargetType.Queue:
                    return QueueTarget.Name;
                case ParaEnums.ActionHistoryTargetType.Download:
                    return DownloadTarget.Name;
                case ParaEnums.ActionHistoryTargetType.System:
                    return "System";
                default:
                    return "";
            }
        }
    }
}