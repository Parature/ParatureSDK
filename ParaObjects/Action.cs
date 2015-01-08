using System;
using System.Collections;
using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public partial class Action : ActionBase
    {
        public bool FullyLoaded;
        public Int64 ActionID = 0;
        public string ActionName = "";
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
            this.FullyLoaded = action.FullyLoaded;
            this.ActionID = action.ActionID;
            this.ActionName = action.ActionName;
            this.actionType = action.actionType;
            this.Comment = action.Comment;
            this.VisibleToCustomer = action.VisibleToCustomer;
            this.EmailText = action.EmailText;
            this.EmailListCsr = action.EmailListCsr;
            this.EmailListCustomers = action.EmailListCustomers;
            this.TimeSpentHours = action.TimeSpentHours;
            this.TimeSpentMinutes = action.TimeSpentMinutes;
            this.AssignToQueueid = action.AssignToQueueid;
            this.AssigntToCsrid = action.AssigntToCsrid;
            this.Action_Attachments = action.Action_Attachments;
        }
        ///// <summary>
        ///// Uploads an attachment to the current ticket. 
        ///// The attachment will also be added to the current Actions's attachments collection.
        ///// </summary>
        ///// <param name="Attachment">
        ///// The binary Byte array of the attachment you would like to add. 
        /////</param>
        //public void AttachmentsAdd(ParaCredentials paracredentials, Byte[] Attachment, string contentType, string FileName)
        //{
        //    Action_Attachments.Add(ApiHandler.Ticket.TicketAddAttachment(paracredentials, Attachment, contentType, FileName));
        //}

        ///// <summary>
        ///// Uploads a text based file to the current ticket. You need to pass a string, and the mime type of a text based file (html, text, etc...).            
        ///// </summary>
        ///// <param name="text">
        ///// The content of the text based file. 
        /////</param>           
        ///// <param name="paracredentials">
        ///// The parature credentials class for the APIs.
        ///// </param>            
        ///// <param name="contentType">
        ///// The type of content being uploaded, you have to make sure this is the right text.
        ///// </param>
        ///// <param name="FileName">
        ///// The name you woule like the attachment to have.
        /////</param>
        //public void AttachmentsAdd(ParaCredentials paracredentials, string text, string contentType, string FileName)
        //{
        //    Action_Attachments.Add(ApiHandler.Ticket.TicketAddAttachment(paracredentials, text, contentType, FileName));
        //}
    }
}