using System;
using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    /// <summary>
    /// Holds all the properties of the Download module.
    /// </summary>
    public partial class Download : ObjectBaseProperties
    {
        // Specific properties for this module
        /// <summary>
        /// An attachment object holds the information about any attachment.
        /// </summary>
        public Attachment Attachment = new Attachment();
        /// <summary>
        /// The unique identified of the download.
        /// </summary>
        public Int64 Downloadid = 0;
        public string Date_Created = "";
        public string Date_Updated = "";
        /// <summary>
        /// The description of the download.
        /// </summary>
        public string Description = "";
        /// <summary>
        /// In case this download consists of an external link, instead of a file, this property will be populated.
        /// Please make sure, when you use this property (in case of a create/update of a download) 
        /// that the Guid property is set to empty, as only one of these two properties must be filled.
        /// </summary>
        public string External_Link = "";

        public bool MultipleFolders;

        /// <summary>
        /// The list of folders under which the download is listed.
        /// </summary>
        public List<DownloadFolder> Folders = new List<DownloadFolder>();

        /// <summary>
        /// If the download consists of a file that has been uploaded, this would be the GUID of the file.
        /// Please make sure, when you use this property (in case of a create/update of a download) 
        /// that the ExternalLink property is set to empty, as only one of these two properties must be filled.
        /// </summary>
        public string Guid = "";
        /// <summary>
        /// List of Sla type objects. These SLAs are the ones allowed to see this download.
        /// </summary>
        public List<Sla> Permissions = new List<Sla>();
        /// <summary>
        /// The name of the download.
        /// </summary>
        public string Name = "";
        /// <summary>
        /// List of products that are linked to this download. In case your config uses this feature.
        /// </summary>
        public List<Product> Products = new List<Product>();
        /// <summary>
        /// Whether the download is published or not.
        /// </summary>
        public Boolean Published = new Boolean();
        public string Title = "";
        public bool Visible;
        /// <summary>
        /// File extension
        /// </summary>
        public string Extension = "";

        /// <summary>
        /// Certain configuration use the End User License Agreement (EULA). this controls what Eula would 
        /// be associated with this download.
        /// </summary>
        public Eula Eula = new Eula();

        /// <summary>
        /// Number of times the files where updated.
        /// </summary>
        public Int64 File_Hits = 0;

        /// <summary>
        /// Size of the file.
        /// </summary>
        public Int64 File_Size = 0;

        /// <summary>
        /// Uploads a file to the Parature system, from a standard System.Net.Mail.Attachment object, in case you use this from an email.
        /// </summary>            
        /// <param name="EmailAttachment">
        /// The email attachment to upload.
        /// </param>
        public void AttachmentsAdd(ParaCredentials paracredentials, System.Net.Mail.Attachment EmailAttachment)
        {
            this.Attachment = ApiHandler.Download.DownloadUploadFile(paracredentials, EmailAttachment);
            this.Guid = this.Attachment.GUID;
        }
            
        /// <summary>
        /// Uploads the file to the current Download. 
        /// The file will also be added to the current Downloads's Guid.
        /// </summary>
        /// <param name="Attachment">
        /// The binary Byte array of the file you would like to upload. 
        ///</param>           
        /// <param name="paracredentials">
        /// The parature credentials class for the APIs.
        /// </param>            
        /// <param name="contentType">
        /// The type of content being uploaded, you have to make sure this is the right text.
        /// </param>
        /// <param name="FileName">
        /// 
        ///</param>
        public void AttachmentsAdd(ParaCredentials paracredentials, Byte[] Attachment, string contentType, string FileName)
        {
            this.Attachment = ApiHandler.Download.DownloadUploadFile(paracredentials, Attachment, contentType, FileName);
            this.Guid = this.Attachment.GUID;
        }

        /// <summary>
        /// Uploads a text based file to the current Download. You need to pass a string, and the mime type of a text based file (html, text, etc...).            
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
            this.Attachment = ApiHandler.Download.DownloadUploadFile(paracredentials, text, contentType, FileName);
            this.Guid = this.Attachment.GUID;
        }

        /// <summary>
        /// Updates the current download attachment with a text based file. You need to pass a string, and the mime type of a text based file (html, text, etc...).            
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
        public void AttachmentsUpdate(ParaCredentials paracredentials, string text, string contentType, string FileName)
        {
            this.Attachment = ApiHandler.Download.DownloadUploadFile(paracredentials, text, contentType, FileName);
            this.Guid = this.Attachment.GUID;
            this.Name = FileName;
        }


        /// <summary>
        /// If you have a download file and would like to replace the file, use this method. It will actually delete 
        /// the existing attachment, and then add a new one to replace it.
        /// </summary>
        public void AttachmentsUpdate(ParaCredentials paracredentials, Byte[] Attachment, string contentType, string FileName)
        {
            this.Attachment = ApiHandler.Download.DownloadUploadFile(paracredentials, Attachment, contentType, FileName);
            this.Guid = this.Attachment.GUID;
            this.Name = FileName;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowMultipleFolders">True if Multiple Folders is configured</param>
        public Download(bool allowMultipleFolders)
        {
            this.MultipleFolders = allowMultipleFolders;
        }

        public Download(Download download)
            : base(download)
        {
            this.Downloadid = download.Downloadid;
            this.Date_Created = download.Date_Created;
            this.Date_Updated = download.Date_Updated;
            this.Description = download.Description;
            this.External_Link = download.External_Link;
            this.MultipleFolders = download.MultipleFolders;
            this.Folders = download.Folders;
            this.Guid = download.Guid;
            this.Permissions = new List<Sla>(download.Permissions);
            this.Name = download.Name;
            this.Products = new List<Product>(download.Products);
            this.Published = download.Published;
            this.Title = download.Title;
            this.Visible = download.Visible;
            this.Eula = new Eula(download.Eula);
            this.File_Hits = download.File_Hits;
            this.File_Size = download.File_Size;
        }
    }
}