using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using ParatureSDK.ParaObjects.EntityReferences;

namespace ParatureSDK.ParaObjects
{
    public class ActionHistory
    {
        /// <summary>
        /// The unique identifier of this action history item
        /// </summary>
        [XmlAttribute("id")]
        public Int64 Id = 0;
        /// <summary>
        /// the action that run
        /// </summary>
        public Action Action = new Action();
        /// <summary>
        /// The status the object was on, before the action was run
        /// </summary>
        public TicketStatusReference Old_Status = new TicketStatusReference();
        /// <summary>
        /// The new status that the object moved to, after the action was run.
        /// </summary>
        public TicketStatusReference New_Status = new TicketStatusReference();
        public String Comments;
        /// <summary>
        /// Populated comments by the system, generally on routing or TSAs
        /// </summary>
        [XmlElement("Systemcomments")]
        public String SystemComments;
        /// <summary>
        /// Whether this action was exposed to the customer or not.
        /// </summary>
        public bool Show_To_Customer;
        public long Time_Spent;
        /// <summary>
        /// The date this action took place.
        /// </summary>
        public DateTime Action_Date;
        public string Cc_Csr;
        public string Cc_Customer;
        /// <summary>
        /// The list, if any exists, of all the Attachments of this action history item.
        /// </summary>
        public List<Attachment> History_Attachments = new List<Attachment>();
        public ActionHistoryPerformer Action_Performer = new ActionHistoryPerformer();
        public ActionHistoryTarget Action_Target = new ActionHistoryTarget();
        public bool? To_Deflection;

        public ActionHistory()
        {
        }

        public ActionHistory(ActionHistory actionHistory)
        {
            Id = actionHistory.Id;
            Action = new Action(actionHistory.Action);
            Old_Status = actionHistory.Old_Status;
            New_Status = actionHistory.New_Status;
            Comments = actionHistory.Comments;
            Show_To_Customer = actionHistory.Show_To_Customer;
            Time_Spent = actionHistory.Time_Spent;
            Action_Date = actionHistory.Action_Date;
            History_Attachments = new List<Attachment>(actionHistory.History_Attachments);
            Action_Performer = new ActionHistoryPerformer(actionHistory.Action_Performer);
            Cc_Csr = actionHistory.Cc_Csr;
            Cc_Customer = actionHistory.Cc_Customer;
        }
    }
}