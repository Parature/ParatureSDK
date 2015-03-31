using System.Xml;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects
{
    /// <summary>
    /// An attachment object holds the information about any attachment, whether in the ticket history, in the ticket itself, a download, or any other module that supports the attachments feature.
    /// </summary>
    public class Attachment
    {
        /// <summary>
        /// This is the unique identifier of the Attachment/Download file in your Parature license.
        /// </summary>
        [XmlElement("Guid")]
        public string Guid = "";
        /// <summary>
        /// The name of the attachment.
        /// </summary>
        public string Name = "";

        /// <summary>
        /// This is the public permanent URL
        /// </summary>
        [XmlAttribute("href")]
        public string Href;

        [XmlAttribute("secure-service-desk-url")]
        public string SecureServiceDeskUrl;

        [XmlAttribute("secure-portal-url")]
        public string SecurePortalUrl;

        /// <summary>
        /// The details of the error message, if the call generated an exception.
        /// </summary>
        public string Error = "";
        /// <summary>
        /// Whether or not there was an exception.
        /// </summary>
        public bool HasException;

        public Attachment()
        {
        }

        public Attachment(Attachment attachment)
        {
            Guid = attachment.Guid;
            Name = attachment.Name;
            Error = attachment.Error;
            HasException = attachment.HasException;
            Href = attachment.Href;
            SecurePortalUrl = attachment.SecurePortalUrl;
            SecureServiceDeskUrl = attachment.SecureServiceDeskUrl;
        }
    }
}