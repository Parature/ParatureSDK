using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ParatureSDK.Fields;
using ParatureSDK.ParaObjects.EntityReferences;

namespace ParatureSDK.ParaObjects
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
                return GetFieldValue<string>("Ticket_Number");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Ticket_Number");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Ticket_Number",
                        FieldDataType = ParaEnums.FieldDataType.String
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The product associated to a ticket. It will only be populated in certain configurations.
        /// </summary>
        public ProductReference Ticket_Product
        {
            get
            {
                return GetFieldValue<ProductReference>("Ticket_Product");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Ticket_Product");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Ticket_Product",
                        FieldDataType = ParaEnums.FieldDataType.EntityReference
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The status of the ticket
        /// </summary>
        public TicketStatusReference Ticket_Status
        {
            get
            {
                return GetFieldValue<TicketStatusReference>("Ticket_Status");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Ticket_Status");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Ticket_Status",
                        FieldDataType = ParaEnums.FieldDataType.Status
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The asset linked to the ticket. this is only populated for certain Product/Asset configurations, when the ticket is linked to an Asset.
        /// </summary>
        public AssetReference Ticket_Asset
        {
            get
            {
                return GetFieldValue<AssetReference>("Ticket_Asset");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Ticket_Asset");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Ticket_Asset",
                        FieldDataType = ParaEnums.FieldDataType.EntityReference
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public SlaReference Ticket_Sla
        {
            get
            {
                return GetFieldValue<SlaReference>("Ticket_Sla");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Ticket_Sla");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Ticket_Sla",
                        FieldDataType = ParaEnums.FieldDataType.Sla
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The department the tickets belongs to. While you specified already the department id in your
        /// credentials class, it could be that the user you are passing the Token of has access to multiple
        /// departments. In which case, the tickets that account has access to will be visible (no matter their departments).
        /// </summary>
        public DepartmentReference Department
        {
            get
            {
                return GetFieldValue<DepartmentReference>("Department");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Department");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Department",
                        FieldDataType = ParaEnums.FieldDataType.Department
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The customer that owns the ticket. If your only requested a standard Ticket read, only the customer id is returned withing the Customer class.
        /// </summary>
        public CustomerReference Ticket_Customer
        {
            get
            {
                return GetFieldValue<CustomerReference>("Ticket_Customer");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Ticket_Customer");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Ticket_Customer",
                        FieldDataType = ParaEnums.FieldDataType.EntityReference
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The additional contact associated to this ticket.
        /// </summary>
        public CustomerReference Additional_Contact
        {
            get
            {
                return GetFieldValue<CustomerReference>("Additional_Contact");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Additional_Contact");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Additional_Contact",
                        FieldDataType = ParaEnums.FieldDataType.EntityReference
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The CSR that has entered this ticket (this class is filled only when a Ticket has been created by a CSR). Only the CSR id and Name are filled in case of a standard ticket read.
        /// </summary>
        public CsrReference Entered_By
        {
            get
            {
                return GetFieldValue<CsrReference>("Entered_By");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Entered_By");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Entered_By",
                        FieldDataType = ParaEnums.FieldDataType.EntityReference
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The CSR that is has this ticket assigned to. This class is only filled if the ticket is assigned to a CSR (as opposed to a Queue). If the ticket is assigned to a CSR, this class will only be filled with the ID of the CSR (unless you requested an appropriate request depth.
        /// </summary>
        public CsrReference Assigned_To
        {
            get
            {
                return GetFieldValue<CsrReference>("Assigned_To");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Assigned_To");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Assigned_To",
                        FieldDataType = ParaEnums.FieldDataType.EntityReference
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// Whether email notification is turned on or off.
        /// </summary>
        public bool? Email_Notification
        {
            get
            {
                return GetFieldValue<bool?>("Email_Notification");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Email_Notification");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Email_Notification",
                        FieldDataType = ParaEnums.FieldDataType.Boolean
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// Whether email notification to Additional Contact is turned on or off.
        /// </summary>
        public bool? Email_Notification_Additional_Contact
        {
            get
            {
                return GetFieldValue<bool?>("Email_Notification_Additional_Contact");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Email_Notification_Additional_Contact");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Email_Notification_Additional_Contact",
                        FieldDataType = ParaEnums.FieldDataType.Boolean
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// Whether email notification to Additional Contact is turned on or off.
        /// </summary>
        public bool? Hide_From_Customer
        {
            get
            {
                return GetFieldValue<bool?>("Hide_From_Customer");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Hide_From_Customer");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Hide_From_Customer",
                        FieldDataType = ParaEnums.FieldDataType.Boolean
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// An optional string array of CSR emails that are CCed when an email notification is sent.
        /// </summary>
        public string Cc_Csr
        {
            get
            {
                return GetFieldValue<string>("Cc_Csr");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Cc_Csr");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Cc_Csr",
                        FieldDataType = ParaEnums.FieldDataType.EntityReference
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// An optional string array of customer emails that are CCed when an email notification is sent.
        /// </summary>
        public string Cc_Customer
        {
            get
            {
                return GetFieldValue<string>("Cc_Customer");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Cc_Customer");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Cc_Customer",
                        FieldDataType = ParaEnums.FieldDataType.EntityReference
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The Queue that has this ticket assigned to. This class is only filled if the ticket is assigned to a Queue (as opposed to a CSR).
        /// </summary>
        public QueueReference Ticket_Queue
        {
            get
            {
                return GetFieldValue<QueueReference>("Ticket_Queue");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Ticket_Queue");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Ticket_Queue",
                        FieldDataType = ParaEnums.FieldDataType.Queue
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// Parent Ticket of this ticket. Only filled whenever there is a parent ticket. Also, only the ticket id will be filled. Please make sure
        /// </summary>
        public TicketReference Ticket_Parent
        {
            get
            {
                return GetFieldValue<TicketReference>("Ticket_Parent");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Ticket_Parent");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Ticket_Parent",
                        FieldDataType = ParaEnums.FieldDataType.EntityReference
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The list, if any exists, of all the child tickets. Please note that, by default, only the ticket id is filled.
        /// </summary>
        public List<Ticket> Ticket_Children
        {
            get
            {
                return GetFieldValue<List<Ticket>>("Ticket_Children");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Ticket_Children");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Ticket_Children",
                        FieldDataType = ParaEnums.FieldDataType.EntityReference
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The list, if any exists, of all the related chats.
        /// </summary>
        public List<Chat> Related_Chats
        {
            get
            {
                return GetFieldValue<List<Chat>>("Related_Chats");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Related_Chats");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Related_Chats",
                        FieldDataType = ParaEnums.FieldDataType.EntityReference
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The list, if any exists, of all the Attachments of this ticket.
        /// </summary>
        public List<Attachment> Ticket_Attachments
        {
            get
            {
                return GetFieldValue<List<Attachment>>("Ticket_Attachments");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Ticket_Attachments");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Ticket_Attachments",
                        FieldDataType = ParaEnums.FieldDataType.Attachment
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The list, if any exists, of all the available actions that can be run agains this ticket.
        /// Only the id and the name of the action
        /// </summary>
        public List<Action> Actions
        {
            get
            {
                return GetFieldValue<List<Action>>("Actions");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Actions");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Actions",
                        FieldDataType = ParaEnums.FieldDataType.Action
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The actions that ran on this ticket. This is only populated if you requested the ticket action history.
        /// </summary>
        [XmlArray("ActionHistory")]
        [XmlArrayItem("History")]
        public List<ActionHistory> ActionHistory
        {
            get
            {
                return GetFieldValue<List<ActionHistory>>("ActionHistory");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "ActionHistory");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "ActionHistory",
                        FieldDataType = ParaEnums.FieldDataType.History
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public DateTime Date_Created
        {
            get
            {
                return GetFieldValue<DateTime>("Date_Created");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Date_Created");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Date_Created",
                        FieldDataType = ParaEnums.FieldDataType.DateTime
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public DateTime Date_Updated
        {
            get
            {
                return GetFieldValue<DateTime>("Date_Updated");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Date_Updated");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Date_Updated",
                        FieldDataType = ParaEnums.FieldDataType.DateTime
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
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
            Ticket_Attachments.Add(ApiHandler.Ticket.AddAttachment(creds, attachment, contentType, fileName));
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
            Ticket_Attachments.Add(ApiHandler.Ticket.AddAttachment(creds, text, contentType, fileName));
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
            Ticket_Attachments.Add(ApiHandler.Ticket.AddAttachment(creds, text, contentType, fileName));
        }

        /// <summary>
        /// If you have an attachment and would like to replace the file, use this method. It will actually delete 
        /// the existing attachment, and then add a new one to replace it.
        /// </summary>
        public void AttachmentsUpdate(ParaCredentials creds, Byte[] attachment, string attachmentGuid, string contentType, string fileName)
        {
            AttachmentsDelete(attachmentGuid);
            Ticket_Attachments.Add(ApiHandler.Ticket.AddAttachment(creds, attachment, contentType, fileName));
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
                Additional_Contact = ticket.Additional_Contact;
                Assigned_To = ticket.Assigned_To;
                Cc_Csr = ticket.Cc_Csr;
                Cc_Customer = ticket.Cc_Customer;
                Date_Created = ticket.Date_Created;
                Date_Updated = ticket.Date_Updated;
                Department = ticket.Department;
                Email_Notification = ticket.Email_Notification;
                Email_Notification_Additional_Contact = ticket.Email_Notification_Additional_Contact;
                Entered_By = ticket.Entered_By;
                Hide_From_Customer = ticket.Hide_From_Customer;
                Id = ticket.Id;
                operation = ticket.operation;
                Ticket_Asset = ticket.Ticket_Asset;
                Ticket_Attachments = new List<Attachment>(ticket.Ticket_Attachments);
                if (ticket.Ticket_Customer != null)
                {
                    Ticket_Customer = ticket.Ticket_Customer;
                }
                if (ticket.Ticket_Children != null)
                {
                    Ticket_Children = new List<Ticket>(ticket.Ticket_Children);
                }
                Ticket_Number = ticket.Ticket_Number;
                if (ticket.Ticket_Parent != null)
                {
                    Ticket_Parent = ticket.Ticket_Parent;
                }
                Ticket_Product = ticket.Ticket_Product;
                Ticket_Queue = ticket.Ticket_Queue;
                Ticket_Status = ticket.Ticket_Status;
            }
        }

        public override string GetReadableName()
        {
            return "Ticket #" + uniqueIdentifier;
        }
    }
}