using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using ParatureSDK.ParaObjects.EntityReferences;
using System.Text;

namespace ParatureSDK.ParaObjects
{
    public class Action : ActionBase
    {
        public bool FullyLoaded;
        [XmlAttribute("name")]
        public string Name = "";
        public string Comment = "";
        /// <summary>
        /// Indicates whether this action will be visible to the customer or not
        /// Only used for tickets.
        /// </summary>
        public bool ShowToCust = false;
        public bool ShowToAdditionalContact = false;
        public bool NotifyParent = false;
        public string EmailText;
        public ArrayList EmailCsrList;
        public ArrayList EmailCustList;
        public int TimeSpentHours = 0;
        public int TimeSpentMinutes = 0;

        public List<Attachment> Action_Attachments = new List<Attachment>();

        ///  <summary>
        ///  Uploads an attachment for ticket action
        ///  The attachment will also be added to the current Ticket's attachments collection.
        ///  </summary>
        ///  <param name="attachment">
        ///  The binary Byte array of the attachment you would like to add.
        /// </param>
        /// <param name="fileName"></param>
        [Obsolete("To be removed in favor of Action.AddAttachment(ParaService, byte[], string) in the next major revision.", false)]
        public void AddAttachment(ParaCredentials creds, Byte[] attachment, string contentType, string fileName)
        {
            Action_Attachments.Add(ApiHandler.Ticket.AddAttachment(creds, attachment, contentType, fileName));
        }

        ///  <summary>
        ///  Uploads an attachment for ticket action
        ///  The attachment will also be added to the current Ticket's attachments collection.
        ///  </summary>
        ///  <param name="attachment">
        ///  The binary Byte array of the attachment you would like to add.
        /// </param>
        /// <param name="fileName"></param>
        [Obsolete("To be removed in favor of Action.AddAttachment(ParaService, byte[], string) in the next major revision.", false)]
        public void AddAttachment(ParaService service, Byte[] attachment, string contentType, string fileName)
        {
            Action_Attachments.Add(service.UploadFile<Ticket>(attachment, fileName));
        }

        public void AddAttachment(ParaService service, Byte[] attachment, string fileName)
        {
            Action_Attachments.Add(service.UploadFile<Ticket>(attachment, fileName));
        }

        /// <summary>
        /// Uploads a text based file for ticket actions. You need to pass a string, and the mime type of a text based file (html, text, etc...).
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
        [Obsolete("To be removed in favor of Action.AddAttachment(ParaService, string, string) in the next major revision.", false)]
        public void AddAttachment(ParaCredentials creds, string text, string contentType, string fileName)
        {
            Action_Attachments.Add(ApiHandler.Ticket.AddAttachment(creds, text, contentType, fileName));
        }

        /// <summary>
        /// Uploads a text based file for ticket actions. You need to pass a string, and the mime type of a text based file (html, text, etc...).
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
        [Obsolete("To be removed in favor of Action.AddAttachment(ParaService, string, string) in the next major revision.", false)]
        public void AddAttachment(ParaService service, string text, string contentType, string fileName)
        {
            var encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes(text);
            AddAttachment(service, bytes, fileName);
        }

        /// <summary>
        /// Uploads a text based file for ticket actions. You need to pass a string, and the mime type of a text based file (html, text, etc...).
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
        public void AddAttachment(ParaService service, string text, string fileName)
        {
            var encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes(text);
            AddAttachment(service, bytes, fileName);
        }

        /// <summary>
        /// This property will only be considered when
        /// the action if of type Assign to Queue.
        /// </summary>
        public Int64? AssignToQueue = 0;

        /// <summary>
        /// This property will only be considered when
        /// the action if of type Assign to CSR.
        /// </summary>
        public Int64? AssignToCsr = 0;

        public Action()
        {
        }

        public Action(Action action)
        {
            FullyLoaded = action.FullyLoaded;
            Id = action.Id;
            Name = action.Name;
            Comment = action.Comment;
            ShowToCust = action.ShowToCust;
            EmailText = action.EmailText;
            EmailCsrList = action.EmailCsrList;
            EmailCustList = action.EmailCustList;
            TimeSpentHours = action.TimeSpentHours;
            TimeSpentMinutes = action.TimeSpentMinutes;
            AssignToQueue = action.AssignToQueue;
            AssignToCsr = action.AssignToCsr;
            Action_Attachments = action.Action_Attachments;
        }
    }
}