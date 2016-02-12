using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ParatureSDK.Fields;
using ParatureSDK.ParaObjects.EntityReferences;
using System.Text;

namespace ParatureSDK.ParaObjects
{
    /// <summary>
    /// Holds all the properties of the Ticket module.
    /// </summary>
    public class Ticket : ParaEntity, IMutableEntity, IHistoricalEntity, IActionRunner
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
                        FieldType = "text",
                        DataType = "string"
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
                        FieldType = "entity",
                        DataType = "entity"
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
                        FieldType = "entity",
                        DataType = "entity"
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
                        FieldType = "entity",
                        DataType = "entity"
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
                        FieldType = "dropdown",
                        DataType = "entity"
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
                        FieldType = "entity",
                        DataType = "entity"
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
                        FieldType = "entity",
                        DataType = "entity"
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
                        FieldType = "entity",
                        DataType = "entity"
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
                        FieldType = "entity",
                        DataType = "entity"
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
                        FieldType = "entity",
                        DataType = "entity"
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
                        FieldType = "checkbox",
                        DataType = "boolean"
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
                        FieldType = "checkbox",
                        DataType = "boolean"
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
                        FieldType = "checkbox",
                        DataType = "boolean"
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
                        FieldType = "entity",
                        DataType = "entity"
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
                        FieldType = "entity",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public bool Overwrite_Sla_In_Rr
        {
            get
            {
                return GetFieldValue<bool>("Overwrite_Sla_In_Rr");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Overwrite_Sla_In_Rr");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Overwrite_Sla_In_Rr",
                        FieldType = "checkbox",
                        DataType = "boolean"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            } 
        }

        public Int64? Initial_Resolution_Target_Duration
        {
            get
            {
                return GetFieldValue<Int64?>("Initial_Resolution_Target_Duration");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Initial_Resolution_Target_Duration");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Initial_Resolution_Target_Duration",
                        FieldType = "int",
                        DataType = "int"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }  
        }

        public DateTime? Initial_Resolution_Date
        {
            get
            {
                return GetFieldValue<DateTime?>("Initial_Resolution_Date");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Initial_Resolution_Date");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Initial_Resolution_Date",
                        FieldType = "usdate",
                        DataType = "date"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public Int64? Initial_Response_Target_Duration
        {
            get
            {
                return GetFieldValue<Int64?>("Initial_Response_Target_Duration");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Initial_Response_Target_Duration");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Initial_Response_Target_Duration",
                        FieldType = "int",
                        DataType = "int"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }  
        }

        public Int64? Initial_Resolution_Duration_Bh
        {
            get
            {
                return GetFieldValue<Int64?>("Initial_Resolution_Duration_Bh");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Initial_Resolution_Duration_Bh");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Initial_Resolution_Duration_Bh",
                        FieldType = "int",
                        DataType = "int"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public Int64? Initial_Response_Duration_Bh
        {
            get
            {
                return GetFieldValue<Int64?>("Initial_Response_Duration_Bh");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Initial_Response_Duration_Bh");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Initial_Response_Duration_Bh",
                        FieldType = "int",
                        DataType = "int"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public DateTime? Resolution_Violation_Date_Bh
        {
            get
            {
                return GetFieldValue<DateTime?>("Resolution_Violation_Date_Bh");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Resolution_Violation_Date_Bh");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Resolution_Violation_Date_Bh",
                        FieldType = "usdate",
                        DataType = "date"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public DateTime? Response_Violation_Date_Bh
        {
            get
            {
                return GetFieldValue<DateTime?>("Response_Violation_Date_Bh");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Response_Violation_Date_Bh");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Response_Violation_Date_Bh",
                        FieldType = "usdate",
                        DataType = "date"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public SlaReference Response_Sla
        {
            get
            {
                return GetFieldValue<SlaReference>("Response_Sla");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Response_Sla");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Response_Sla",
                        FieldType = "dropdown",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public SlaReference Resolution_Sla
        {
            get
            {
                return GetFieldValue<SlaReference>("Resolution_Sla");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Resolution_Sla");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Resolution_Sla",
                        FieldType = "dropdown",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public Int64? Warning_Time
        {
            get
            {
                return GetFieldValue<Int64?>("Warning_Time");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Warning_Time");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Warning_Time",
                        FieldType = "int",
                        DataType = "int"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public Int64? Last_Resolution_Duration_Bh
        {
            get
            {
                return GetFieldValue<Int64?>("Last_Resolution_Duration_Bh");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Last_Resolution_Duration_Bh");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Last_Resolution_Duration_Bh",
                        FieldType = "int",
                        DataType = "int"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public CsrReference Initial_Response_Userid
        {
            get
            {
                return GetFieldValue<CsrReference>("Initial_Response_Userid");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Initial_Response_Userid");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Initial_Response_Userid",
                        FieldType = "dropdown",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public CsrReference Initial_Resolution_Userid
        {
            get
            {
                return GetFieldValue<CsrReference>("Initial_Resolution_Userid");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Initial_Resolution_Userid");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Initial_Resolution_Userid",
                        FieldType = "dropdown",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public CsrReference Final_Resolution_Userid
        {
            get
            {
                return GetFieldValue<CsrReference>("Final_Resolution_Userid");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Final_Resolution_Userid");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Final_Resolution_Userid",
                        FieldType = "dropdown",
                        DataType = "entity"
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
                        FieldType = "dropdown",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The Portal Alias that the ticket was submitted under
        /// </summary>
        public PortalAliasReference Ticket_Portal_Alias
        {
            get
            {
                return GetFieldValue<PortalAliasReference>("Ticket_Portal_Alias");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Ticket_Portal_Alias");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Ticket_Portal_Alias",
                        FieldType = "entity",
                        DataType = "entity"
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
                        FieldType = "entity",
                        DataType = "entity"
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
                        FieldType = "entitymultiple",
                        DataType = "entity"
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
                        FieldType = "entitymultiple",
                        DataType = "entity"
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
                    //the FieldType and DataType are NOT from the actual APIs. 
                    //They are a representation purely added for the SDK
                    field = new StaticField()
                    {
                        Name = "Ticket_Attachments",
                        FieldType = "entitymultiple",
                        DataType = "attachment"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// Internal property. We don't want to allow the user to accidentally delete attachments.
        /// A situation could occur where the developer tries to update tickets but instantiates the Tickets instead of retrieving
        /// This could theoretically allow the developer to delete tickets in rare scenarios.
        /// 
        /// Going to add this as an internal property which needs to be explicitly called by the user before we decide to delete attachments
        /// </summary>
        [XmlIgnore]
        internal bool? AllowDeleteAllAttachments
        {
            get
            {
                return GetFieldValue<bool?>("AllowDeleteAllAttachments");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "AllowDeleteAllAttachments");
                if (field == null)
                {
                    //the FieldType and DataType are NOT from the actual APIs. 
                    //They are a representation purely added for the SDK
                    field = new StaticField()
                    {
                        Name = "AllowDeleteAllAttachments",
                        IgnoreSerializeXml = true,
                        FieldType = "checkbox",
                        DataType = "boolean"
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
                        IgnoreSerializeXml = true,
                        Name = "Actions"
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
                        IgnoreSerializeXml = true,
                        Name = "ActionHistory"
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
                        FieldType = "usdate",
                        DataType = "date"
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
                        FieldType = "usdate",
                        DataType = "date"
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
        [Obsolete("To be removed in favor of Ticket.AddAttachment(ParaService, byte[], string, string) in the next major revision.", false)]
        public void AttachmentsAdd(ParaCredentials creds, Byte[] attachment, string contentType, string fileName)
        {
            Ticket_Attachments.Add(ApiHandler.Ticket.AddAttachment(creds, attachment, contentType, fileName));
        }

        ///  <summary>
        ///  Uploads an attachment to the current ticket. 
        ///  The attachment will also be added to the current Ticket's attachments collection.
        ///  </summary>
        ///  <param name="attachment">
        ///  The binary Byte array of the attachment you would like to add. 
        /// </param>
        /// <param name="fileName"></param>
        public void AddAttachment(ParaService service, Byte[] attachment, string contentType, string fileName)
        {
            Ticket_Attachments.Add(service.UploadFile<Ticket>(attachment, contentType, fileName));
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
        [Obsolete("To be removed in favor of Ticket.AddAttachment(ParaService, string, string, string) in the next major revision.", false)]
        public void AttachmentsAdd(ParaCredentials creds, string text, string contentType, string fileName)
        {
            Ticket_Attachments.Add(ApiHandler.Ticket.AddAttachment(creds, text, contentType, fileName));
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
        public void AddAttachment(ParaService service, string text, string contentType, string fileName)
        {
            var encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes(text);
            Ticket_Attachments.Add(service.UploadFile<Ticket>(bytes, contentType, fileName));
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
        [Obsolete("To be removed in favor of Ticket.UpdateAttachment(string, string, string) in the next major revision.", false)]
        public void AttachmentsUpdate(ParaCredentials creds, string text, string attachmentGuid, string contentType, string fileName)
        {
            AttachmentsDelete(attachmentGuid);
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
        public void UpdateAttachment(ParaService service, string text, string attachmentGuid, string contentType, string fileName)
        {
            var encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes(text);
            UpdateAttachment(service, bytes, attachmentGuid, contentType, fileName);
        }

        /// <summary>
        /// If you have an attachment and would like to replace the file, use this method. It will actually delete 
        /// the existing attachment, and then add a new one to replace it.
        /// </summary>
        [Obsolete("To be removed in favor of Ticket.UpdateAttachment(ParaService, byte[], string, string) in the next major revision.", false)]
        public void AttachmentsUpdate(ParaCredentials creds, Byte[] attachment, string attachmentGuid, string contentType, string fileName)
        {
            AttachmentsDelete(attachmentGuid);
            Ticket_Attachments.Add(ApiHandler.Ticket.AddAttachment(creds, attachment, contentType, fileName));
        }

        /// <summary>
        /// If you have an attachment and would like to replace the file, use this method. It will actually delete 
        /// the existing attachment, and then add a new one to replace it.
        /// </summary>
        public void UpdateAttachment(ParaService service, Byte[] attachment, string attachmentGuid, string contentType, string fileName)
        {
            DeleteAttachment(attachmentGuid);
            Ticket_Attachments.Add(service.UploadFile<Ticket>(attachment, contentType, fileName));
        }

        /// <summary>
        /// If you have an attachment and would like to delete, just pass the id here.
        /// </summary>
        [Obsolete("To be removed in favor of Ticket.DeleteAttachment in the next major revision.", false)]
        public bool AttachmentsDelete(string attachmentGuid)
        {
            return DeleteAttachment(attachmentGuid);
        }

        /// <summary>
        /// If you have an attachment and would like to delete, just pass the id here.
        /// </summary>
        public bool DeleteAttachment(string attachmentGuid)
        {
            if (Ticket_Attachments == null)
            {
                return false;
            }

            var matchingAtt = Ticket_Attachments.FirstOrDefault(at => at.Guid == attachmentGuid);
            if (matchingAtt != null)
            {
                Ticket_Attachments.Remove(matchingAtt);
                if (Ticket_Attachments.Any() == false)
                {
                    AllowDeleteAllAttachments = true;
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Use this method to explicitly delete all attachments.
        /// </summary>
        public void DeleteAllAttachments()
        {
            AllowDeleteAllAttachments = true;
            Ticket_Attachments = new List<Attachment>();
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
            return "Ticket #" + Id;
        }
    }
}