using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ParatureAPI.Fields;

namespace ParatureAPI.ParaObjects
{
    /// <summary>
    /// Holds all the properties of the Ticket module.
    /// </summary>
    public class Ticket : ParaEntity
    {
        /// <summary>
        /// The full ticket number, including the account number. Usually in the format 
        /// of Account #-Ticket # 
        /// </summary>
        public string Ticket_Number
        {
            get
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Ticket_Number");
                var val = string.Empty;
                try
                {
                    val = field.Value;
                }
                catch (Exception e) { }

                return val;
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Ticket_Number");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Ticket_Number",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }

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
        public bool Email_Notification
        {
            get
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Email_Notification");
                var val = false;
                try
                {
                    val = Convert.ToBoolean(field.Value);
                }
                catch (Exception e) { }

                return val;
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Email_Notification");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Email_Notification",
                        DataType = ParaEnums.FieldDataType.boolean
                    };
                    Fields.Add(field);
                }

                field.Value = value.ToString();
            }
        }

        /// <summary>
        /// Whether email notification to Additional Contact is turned on or off.
        /// </summary>
        public bool? Email_Notification_Additional_Contact
        {
            get
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Email_Notification_Additional_Contact");
                var val = false;
                try
                {
                    val = Convert.ToBoolean(field.Value);
                }
                catch (Exception e) { }

                return val;
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Email_Notification_Additional_Contact");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Email_Notification_Additional_Contact",
                        DataType = ParaEnums.FieldDataType.boolean
                    };
                    Fields.Add(field);
                }

                field.Value = value.ToString();
            }
        }

        /// <summary>
        /// Whether email notification to Additional Contact is turned on or off.
        /// </summary>
        public bool? Hide_From_Customer
        {
            get
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Hide_From_Customer");
                var val = false;
                try
                {
                    val = Convert.ToBoolean(field.Value);
                }
                catch (Exception e) { }

                return val;
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Hide_From_Customer");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Hide_From_Customer",
                        DataType = ParaEnums.FieldDataType.boolean
                    };
                    Fields.Add(field);
                }

                field.Value = value.ToString();
            }
        }

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

        public DateTime Date_Created
        {
            get
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Date_Created");
                var val = DateTime.MinValue;
                try
                {
                    return DateTime.Parse(field.Value);
                }
                catch (Exception e) { }

                return val;
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Date_Created");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Date_Created",
                        DataType = ParaEnums.FieldDataType.DateTime
                    };
                    Fields.Add(field);
                }

                field.Value = value.ToString();
            }
        }

        public DateTime Date_Updated
        {
            get
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Date_Updated");
                var val = DateTime.MinValue;
                try
                {
                    return DateTime.Parse(field.Value);
                }
                catch (Exception e) { }

                return val;
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Date_Updated");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Date_Updated",
                        DataType = ParaEnums.FieldDataType.DateTime
                    };
                    Fields.Add(field);
                }

                field.Value = value.ToString();
            }
        }

        ///  <summary>
        ///  Uploads an attachment to the current ticket. 
        ///  The attachment will also be added to the current Ticket's attachments collection.
        ///  </summary>
        ///  <param name="attachment">
        ///  The binary Byte array of the attachment you would like to add. 
        /// </param>
        /// <param name="fileName"></param>
        public void AttachmentsAdd(ParaCredentials creds, Byte[] attachment, string contentType, string fileName)
        {
            Ticket_Attachments.Add(ApiHandler.Ticket.TicketAddAttachment(creds, attachment, contentType, fileName));
        }

        /// <summary>
        /// Uploads a text based file to the current ticket. You need to pass a string, and the mime type of a text based file (html, text, etc...).            
        /// </summary>
        /// <param name="text">
        /// The content of the text based file. 
        ///</param>           
        /// <param name="creds">
        /// The parature credentials class for the APIs.
        /// </param>            
        /// <param name="contentType">
        /// The type of content being uploaded, you have to make sure this is the right text.
        /// </param>
        /// <param name="fileName">
        /// The name you woule like the attachment to have.
        ///</param>
        public void AttachmentsAdd(ParaCredentials creds, string text, string contentType, string fileName)
        {
            Ticket_Attachments.Add(ApiHandler.Ticket.TicketAddAttachment(creds, text, contentType, fileName));
        }

        /// <summary>
        /// Updates the current Ticket attachment with a text based file. You need to pass a string, and the mime type of a text based file (html, text, etc...).            
        /// </summary>
        /// <param name="text">
        /// The content of the text based file. 
        ///</param>           
        /// <param name="creds">
        /// The parature credentials class for the APIs.
        /// </param>            
        /// <param name="contentType">
        /// The type of content being uploaded, you have to make sure this is the right text.
        /// </param>
        /// <param name="fileName">
        /// The name you woule like the attachment to have.
        ///</param>
        public void AttachmentsUpdate(ParaCredentials creds, string text, string attachmentGuid, string contentType, string fileName)
        {
            AttachmentsDelete(attachmentGuid);
            Ticket_Attachments.Add(ApiHandler.Ticket.TicketAddAttachment(creds, text, contentType, fileName));
        }

        /// <summary>
        /// If you have an attachment and would like to replace the file, use this method. It will actually delete 
        /// the existing attachment, and then add a new one to replace it.
        /// </summary>
        public void AttachmentsUpdate(ParaCredentials creds, Byte[] attachment, string attachmentGuid, string contentType, string fileName)
        {
            AttachmentsDelete(attachmentGuid);
            Ticket_Attachments.Add(ApiHandler.Ticket.TicketAddAttachment(creds, attachment, contentType, fileName));
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
                ActionHistory = new List<ActionHistory>(ticket.ActionHistory);
                Actions = new List<Action>(ticket.Actions);
                Additional_Contact = new Customer(ticket.Additional_Contact);
                Assigned_To = new Csr(ticket.Assigned_To);
                Cc_Csr = new ArrayList(ticket.Cc_Csr);
                Cc_Customer = new ArrayList(ticket.Cc_Customer);
                Date_Created = ticket.Date_Created;
                Date_Updated = ticket.Date_Updated;
                Department = new Department(ticket.Department);
                Email_Notification = ticket.Email_Notification;
                Email_Notification_Additional_Contact = ticket.Email_Notification_Additional_Contact;
                Entered_By = new Csr(ticket.Entered_By);
                Hide_From_Customer = ticket.Hide_From_Customer;
                Id = ticket.Id;
                operation = ticket.operation;
                Ticket_Asset = new Asset(ticket.Ticket_Asset);
                Ticket_Attachments = new List<Attachment>(ticket.Ticket_Attachments);
                if (ticket.Ticket_Customer != null)
                {
                    Ticket_Customer = new Customer(ticket.Ticket_Customer);
                }
                if (ticket.Ticket_Children != null)
                {
                    Ticket_Children = new List<Ticket>(ticket.Ticket_Children);
                }
                Ticket_Number = ticket.Ticket_Number;
                if (ticket.Ticket_Parent != null)
                {
                    Ticket_Parent = new Ticket(ticket.Ticket_Parent);
                }
                Ticket_Product = new Product(ticket.Ticket_Product);
                Ticket_Queue = new Queue(ticket.Ticket_Queue);
                Ticket_Status = new TicketStatus(ticket.Ticket_Status);
            }
        }

        public override string GetReadableName()
        {
            return "Ticket #" + uniqueIdentifier;
        }
    }
}