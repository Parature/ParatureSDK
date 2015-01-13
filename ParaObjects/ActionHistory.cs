using System;
using System.Collections;
using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public class ActionHistory
    {
        /// <summary>
        /// The unique identifier of this action history item
        /// </summary>
        public Int64 ActionHistoryID = 0;
        /// <summary>
        /// the action that run
        /// </summary>
        public Action Action = new Action();
        /// <summary>
        /// The status the object was on, before the action was run
        /// </summary>
        public TicketStatus Old_Status = new TicketStatus();
        /// <summary>
        /// The new status that the object moved to, after the action was run.
        /// </summary>
        public TicketStatus New_Status = new TicketStatus();
        public String Comments;
        /// <summary>
        /// Whether this action was exposed to the customer or not.
        /// </summary>
        public bool Show_To_Customer;
        public int Time_Spent;
        /// <summary>
        /// The date this action took place.
        /// </summary>
        public DateTime Action_Date;
        public ArrayList Cc_Csr = new ArrayList();
        public ArrayList Cc_Customer = new ArrayList();
        /// <summary>
        /// The list, if any exists, of all the Attachments of this action history item.
        /// </summary>
        public List<Attachment> History_Attachments = new List<Attachment>();
        public ActionHistoryPerformer Action_Performer = new ActionHistoryPerformer();

        public ActionHistory()
        {
        }

        public ActionHistory(ActionHistory actionHistory)
        {
            ActionHistoryID = actionHistory.ActionHistoryID;
            Action = new Action(actionHistory.Action);
            Old_Status = new TicketStatus(actionHistory.Old_Status);
            New_Status = new TicketStatus(actionHistory.New_Status);
            Comments = actionHistory.Comments;
            Show_To_Customer = actionHistory.Show_To_Customer;
            Time_Spent = actionHistory.Time_Spent;
            Action_Date = actionHistory.Action_Date;
            History_Attachments = new List<Attachment>(actionHistory.History_Attachments);
            Action_Performer = new ActionHistoryPerformer(actionHistory.Action_Performer);
            Cc_Csr = new ArrayList(actionHistory.Cc_Csr);
            Cc_Customer = new ArrayList(actionHistory.Cc_Customer);
        }
    }
}