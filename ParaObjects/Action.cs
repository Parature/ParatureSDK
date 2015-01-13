using System;
using System.Collections;
using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public class Action : ActionBase
    {
        public bool FullyLoaded;
        public string Name = "";
        internal ParaEnums.ActionType actionType;
        public string Comment = "";
        /// <summary>
        /// Indicates whether this action will be visible to the customer or not
        /// Only used for tickets.
        /// </summary>
        public bool VisibleToCustomer = false;
        public string EmailText;
        public ArrayList EmailListCsr;
        public ArrayList EmailListCustomers;
        public int TimeSpentHours = 0;
        public int TimeSpentMinutes = 0;

        public List<Attachment> Action_Attachments = new List<Attachment>();

        /// <summary>
        /// This property will only be considered when 
        /// the action if of type Assign to Queue.
        /// </summary>
        internal Int64 AssignToQueueid = 0;

        /// <summary>
        /// This property will only be considered when 
        /// the action if of type Assign to CSR.
        /// </summary>
        internal Int64 AssigntToCsrid = 0;

        public Action()
        {
        }

        public Action(Action action)
        {
            FullyLoaded = action.FullyLoaded;
            Id = action.Id;
            Name = action.Name;
            actionType = action.actionType;
            Comment = action.Comment;
            VisibleToCustomer = action.VisibleToCustomer;
            EmailText = action.EmailText;
            EmailListCsr = action.EmailListCsr;
            EmailListCustomers = action.EmailListCustomers;
            TimeSpentHours = action.TimeSpentHours;
            TimeSpentMinutes = action.TimeSpentMinutes;
            AssignToQueueid = action.AssignToQueueid;
            AssigntToCsrid = action.AssigntToCsrid;
            Action_Attachments = action.Action_Attachments;
        }
    }
}