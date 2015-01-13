using System;
using System.Collections;
using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    /// <summary>
    /// Holds all the properties of the Ticket module.
    /// </summary>
    public partial class Ticket : ParaEntity
    {
        /// <summary>
        /// The unique identifier of the ticket
        /// </summary>
        public Int64 id = 0;

        /// <summary>
        /// The full ticket number, including the account number. Usually in the format 
        /// of Account #-Ticket # 
        /// </summary>
        public string Ticket_Number = "";

        ///// <summary>
        ///// The id of the department the ticket is part of.
        ///// </summary>
        //public Int64 DepartmentID = 0;

        /// <summary>
        /// The product associated to a ticket. It will only be populated in certain configurations.
        /// </summary>
        public Product Ticket_Product = new Product();

        /// <summary>
        /// The status of the ticket
        /// </summary>
        public TicketStatus Ticket_Status = new TicketStatus();

        /// <summary>
        /// The asset linked to the ticket. this is only populated for certain Product/Asset configurations, when the ticket is linked to an Asset.
        /// </summary>
        public Asset Ticket_Asset = new Asset();

        public Sla Ticket_Sla = new Sla();

        /// <summary>
        /// The department the tickets belongs to. While you specified already the department id in your
        /// credentials class, it could be that the user you are passing the Token of has access to multiple
        /// departments. In which case, the tickets that account has access to will be visible (no matter their departments).
        /// </summary>
        public Department Department = new Department();


        /// <summary>
        /// The customer that owns the ticket. If your only requested a standard Ticket read, only the customer id is returned withing the Customer class.
        /// </summary>
        public Customer Ticket_Customer = new Customer();

        /// <summary>
        /// The additional contact associated to this ticket.
        /// </summary>
        public Customer Additional_Contact = new Customer();

        /// <summary>
        /// The CSR that has entered this ticket (this class is filled only when a Ticket has been created by a CSR). Only the CSR id and Name are filled in case of a standard ticket read.
        /// </summary>
        public Csr Entered_By = new Csr();

        /// <summary>
        /// The CSR that is has this ticket assigned to. This class is only filled if the ticket is assigned to a CSR (as opposed to a Queue). If the ticket is assigned to a CSR, this class will only be filled with the ID of the CSR (unless you requested an appropriate request depth.
        /// </summary>
        public Csr Assigned_To = new Csr();

        /// <summary>
        /// Whether email notification is turned on or off.
        /// </summary>
        public bool Email_Notification;

        /// <summary>
        /// Whether email notification to Additional Contact is turned on or off.
        /// </summary>
        public Nullable<Boolean> Email_Notification_Additional_Contact;

        /// <summary>
        /// Whether email notification to Additional Contact is turned on or off.
        /// </summary>
        public Nullable<Boolean> Hide_From_Customer;


        /// <summary>
        /// An optional string array of CSR emails that are CCed when an email notification is sent.
        /// </summary>
        public ArrayList Cc_Csr = new ArrayList();

        /// <summary>
        /// An optional string array of customer emails that are CCed when an email notification is sent.
        /// </summary>
        public ArrayList Cc_Customer = new ArrayList();

        /// <summary>
        /// The Queue that has this ticket assigned to. This class is only filled if the ticket is assigned to a Queue (as opposed to a CSR).
        /// </summary>
        public Queue Ticket_Queue = new Queue();

        /// <summary>
        /// Parent Ticket of this ticket. Only filled whenever there is a parent ticket. Also, only the ticket id will be filled. Please make sure
        /// </summary>
        public Ticket Ticket_Parent;

        /// <summary>
        /// The list, if any exists, of all the child tickets. Please note that, by default, only the ticket id is filled.
        /// </summary>
        public List<Ticket> Ticket_Children;

        /// <summary>
        /// The list, if any exists, of all the related chats.
        /// </summary>
        public List<Chat> Related_Chats;

        /// <summary>
        /// The list, if any exists, of all the Attachments of this ticket.
        /// </summary>
        public List<Attachment> Ticket_Attachments = new List<Attachment>();

        /// <summary>
        /// The list, if any exists, of all the available actions that can be run agains this ticket.
        /// Only the id and the name of the action
        /// </summary>
        public List<Action> Actions = new List<Action>();

        /// <summary>
        /// The actions that ran on this ticket. This is only populated if you requested the ticket action history.
        /// </summary>
        public List<ActionHistory> ActionHistory = new List<ActionHistory>();

        public string Date_Created = "";
        public string Date_Updated = "";

        /// <summary>
        /// Uploads an attachment to the current ticket. 
        /// The attachment will also be added to the current Ticket's attachments collection.
        /// </summary>
        /// <param name="Attachment">
        /// The binary Byte array of the attachment you would like to add. 
        ///</param>
        public void AttachmentsAdd(ParaCredentials paracredentials, Byte[] Attachment, string contentType, string FileName)
        {
            Ticket_Attachments.Add(ApiHandler.Ticket.TicketAddAttachment(paracredentials, Attachment, contentType, FileName));
        }

        /// <summary>
        /// Uploads a text based file to the current ticket. You need to pass a string, and the mime type of a text based file (html, text, etc...).            
        /// </summary>
        /// <param name="text">
        /// The content of the text based file. 
        ///</param>           
        /// <param name="paracredentials">
        /// The parature credentials class for the APIs.
        /// </param>            
        /// <param name="contentType">
        /// The type of content being uploaded, you have to make sure this is the right text.
        /// </param>
        /// <param name="FileName">
        /// The name you woule like the attachment to have.
        ///</param>
        public void AttachmentsAdd(ParaCredentials paracredentials, string text, string contentType, string FileName)
        {
            Ticket_Attachments.Add(ApiHandler.Ticket.TicketAddAttachment(paracredentials, text, contentType, FileName));
        }

        /// <summary>
        /// Updates the current Ticket attachment with a text based file. You need to pass a string, and the mime type of a text based file (html, text, etc...).            
        /// </summary>
        /// <param name="text">
        /// The content of the text based file. 
        ///</param>           
        /// <param name="paracredentials">
        /// The parature credentials class for the APIs.
        /// </param>            
        /// <param name="contentType">
        /// The type of content being uploaded, you have to make sure this is the right text.
        /// </param>
        /// <param name="FileName">
        /// The name you woule like the attachment to have.
        ///</param>
        public void AttachmentsUpdate(ParaCredentials paracredentials, string text, string AttachmentGuid, string contentType, string FileName)
        {
            AttachmentsDelete(AttachmentGuid);
            Ticket_Attachments.Add(ApiHandler.Ticket.TicketAddAttachment(paracredentials, text, contentType, FileName));
        }



        /// <summary>
        /// If you have an attachment and would like to replace the file, use this method. It will actually delete 
        /// the existing attachment, and then add a new one to replace it.
        /// </summary>
        public void AttachmentsUpdate(ParaCredentials paracredentials, Byte[] Attachment, string AttachmentGuid, string contentType, string FileName)
        {
            AttachmentsDelete(AttachmentGuid);
            Ticket_Attachments.Add(ApiHandler.Ticket.TicketAddAttachment(paracredentials, Attachment, contentType, FileName));
        }

        /// <summary>
        /// If you have an attachment and would like to delete, just pass the id here.
        /// </summary>
        public void AttachmentsDelete(string AttachmentGuid)
        {
            foreach (Attachment at in Ticket_Attachments)
            {
                if (at.GUID == AttachmentGuid)
                {
                    Ticket_Attachments.Remove(at);
                }
            }

        }

        public Ticket()
            : base()
        {
        }

        public Ticket(Ticket ticket)
            : base(ticket)
        {
            if (ticket != null)
            {
                this.ActionHistory = new List<ActionHistory>(ticket.ActionHistory);
                this.Actions = new List<Action>(ticket.Actions);
                this.Additional_Contact = new Customer(ticket.Additional_Contact);
                this.Assigned_To = new Csr(ticket.Assigned_To);
                this.Cc_Csr = new ArrayList(ticket.Cc_Csr);
                this.Cc_Customer = new ArrayList(ticket.Cc_Customer);
                this.Date_Created = new string(ticket.Date_Created.ToCharArray());
                this.Date_Updated = ticket.Date_Updated;
                this.Department = new Department(ticket.Department);
                this.Email_Notification = ticket.Email_Notification;
                this.Email_Notification_Additional_Contact = ticket.Email_Notification_Additional_Contact;
                this.Entered_By = new Csr(ticket.Entered_By);
                this.Hide_From_Customer = ticket.Hide_From_Customer;
                this.id = ticket.id;
                this.operation = ticket.operation;
                this.Ticket_Asset = new Asset(ticket.Ticket_Asset);
                this.Ticket_Attachments = new List<Attachment>(ticket.Ticket_Attachments);
                if (ticket.Ticket_Customer != null)
                {
                    this.Ticket_Customer = new Customer(ticket.Ticket_Customer);
                }
                if (ticket.Ticket_Children != null)
                {
                    this.Ticket_Children = new List<Ticket>(ticket.Ticket_Children);
                }
                this.Ticket_Number = ticket.Ticket_Number;
                if (ticket.Ticket_Parent != null)
                {
                    this.Ticket_Parent = new Ticket(ticket.Ticket_Parent);
                }
                this.Ticket_Product = new Product(ticket.Ticket_Product);
                this.Ticket_Queue = new Queue(ticket.Ticket_Queue);
                this.Ticket_Status = new TicketStatus(ticket.Ticket_Status);
            }
        }

        public override string GetReadableName()
        {
            return "Ticket #" + this.uniqueIdentifier.ToString();
        }
    }
}