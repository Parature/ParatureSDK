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
        public string GUID = "";
        /// <summary>
        /// The name of the attachment.
        /// </summary>
        public string Name = "";
        public string AttachmentURL = "";
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
            GUID = attachment.GUID;
            Name = attachment.Name;
            AttachmentURL = attachment.AttachmentURL;
            Error = attachment.Error;
            HasException = attachment.HasException;
        }
    }
}