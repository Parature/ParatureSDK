using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ParatureSDK.Fields;

namespace ParatureSDK.ParaObjects
{
    /// <summary>
    /// Holds all the properties of the Download module.
    /// </summary>
    public class Download : ParaEntity
    {
        // Specific properties for this module
        /// <summary>
        /// An attachment object holds the information about any attachment.
        /// </summary>
        public Attachment Attachment
        {
            get
            {
                return GetFieldValue<Attachment>("Attachment");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Attachment");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Attachment",
                        FieldType = "entity",
                        DataType = "entity"
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
        /// <summary>
        /// The description of the download.
        /// </summary>
        public string Description
        {
            get
            {
                return GetFieldValue<string>("Description");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Description");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Description",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        /// <summary>
        /// In case this download consists of an external link, instead of a file, this property will be populated.
        /// Please make sure, when you use this property (in case of a create/update of a download) 
        /// that the Guid property is set to empty, as only one of these two properties must be filled.
        /// </summary>
        public string External_Link
        {
            get
            {
                return GetFieldValue<string>("External_Link");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "External_Link");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "External_Link",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public bool MultipleFolders
        {
            get
            {
                return GetFieldValue<bool>("MultipleFolders");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "MultipleFolders");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "MultipleFolders",
                        FieldType = "checkbox",
                        DataType = "boolean"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The list of folders under which the download is listed.
        /// In certain configurations, there may only be 1 folder allowed. 
        /// Check the "MultipleFolders" property to see whether more than one folder is allowed.
        /// If extra folders are added when only one is allowed, the first in the list will sent
        /// </summary>
        public List<DownloadFolder> Folders
        {
            get
            {
                return GetFieldValue<List<DownloadFolder>>("Folders");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Folders");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Folders",
                        FieldType = "entitymultiple",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// If the download consists of a file that has been uploaded, this would be the GUID of the file.
        /// Please make sure, when you use this property (in case of a create/update of a download) 
        /// that the ExternalLink property is set to empty, as only one of these two properties must be filled.
        /// </summary>
        public string Guid
        {
            get
            {
                return GetFieldValue<string>("Guid");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Guid");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Guid",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        /// <summary>
        /// List of Sla type objects. These SLAs are the ones allowed to see this download.
        /// </summary>
        public List<Sla> Permissions
        {
            get
            {
                return GetFieldValue<List<Sla>>("Permissions");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Permissions");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Permissions",
                        FieldType = "entitymultiple",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        /// <summary>
        /// The name of the download.
        /// </summary>
        public string Name
        {
            get
            {
                return GetFieldValue<string>("Name");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Name");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Name",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        /// <summary>
        /// List of products that are linked to this download. In case your config uses this feature.
        /// </summary>
        public List<Product> Products
        {
            get
            {
                return GetFieldValue<List<Product>>("Products");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Products");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Products",
                        FieldType = "entitymultiple",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        /// <summary>
        /// Whether the download is published or not.
        /// </summary>
        public bool? Published
        {
            get
            {
                return GetFieldValue<bool?>("Published");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Published");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Published",
                        FieldType = "checkbox",
                        DataType = "boolean"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public string Title
        {
            get
            {
                return GetFieldValue<string>("Title");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Title");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Title",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public bool? Visible
        {
            get
            {
                return GetFieldValue<bool?>("Visible");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Visible");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Visible",
                        FieldType = "checkbox",
                        DataType = "boolean"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        /// <summary>
        /// File extension
        /// </summary>
        [XmlElement("Ext")]
        public string Extension
        {
            get
            {
                return GetFieldValue<string>("Ext");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Ext");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Ext",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public Int32? Portal_Search_Weight
        {
            get
            {
                return GetFieldValue<Int32>("Portal_Search_Weight");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Portal_Search_Weight");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Portal_Search_Weight",
                        FieldType = "int",
                        DataType = "int"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public Int32? Desk_Search_Weight
        {
            get
            {
                return GetFieldValue<Int32>("Desk_Search_Weight");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Desk_Search_Weight");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Desk_Search_Weight",
                        FieldType = "int",
                        DataType = "int"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public bool? Hide_On_Portal
        {
            get
            {
                return GetFieldValue<bool>("Hide_On_Portal");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Hide_On_Portal");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Hide_On_Portal",
                        FieldType = "checkbox",
                        DataType = "boolean"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public bool? Is_Portal_Search_Pinned
        {
            get
            {
                return GetFieldValue<bool>("Is_Portal_Search_Pinned");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Is_Portal_Search_Pinned");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Is_Portal_Search_Pinned",
                        FieldType = "checkbox",
                        DataType = "boolean"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public bool? Is_Desk_Search_Pinned
        {
            get
            {
                return GetFieldValue<bool>("Is_Desk_Search_Pinned");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Is_Desk_Search_Pinned");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Is_Desk_Search_Pinned",
                        FieldType = "checkbox",
                        DataType = "boolean"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public string Language
        {
            get
            {
                return GetFieldValue<string>("Language");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Language");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Language",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// Certain configuration use the End User License Agreement (EULA). this controls what Eula would 
        /// be associated with this download.
        /// </summary>
        public Eula Eula
        {
            get
            {
                return GetFieldValue<Eula>("Eula");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Eula");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Eula",
                        FieldType = "entity",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// Number of times the files where updated.
        /// </summary>
        public Int64 File_Hits
        {
            get
            {
                return GetFieldValue<Int64>("File_Hits");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "File_Hits");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "File_Hits",
                        FieldType = "int",
                        DataType = "int"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// Size of the file.
        /// </summary>
        public Int64 File_Size
        {
            get
            {
                return GetFieldValue<Int64>("File_Size");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "File_Size");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "File_Size",
                        FieldType = "int",
                        DataType = "int"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// Uploads a file to the Parature system, from a standard System.Net.Mail.Attachment object, in case you use this from an email.
        /// </summary>            
        /// <param name="EmailAttachment">
        /// The email attachment to upload.
        /// </param>
        public void AttachmentsAdd(ParaCredentials paracredentials, System.Net.Mail.Attachment EmailAttachment)
        {
            Attachment = ApiHandler.Download.DownloadUploadFile(paracredentials, EmailAttachment);
            Guid = Attachment.Guid;
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
            Guid = this.Attachment.Guid;
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
            Attachment = ApiHandler.Download.DownloadUploadFile(paracredentials, text, contentType, FileName);
            Guid = Attachment.Guid;
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
            Attachment = ApiHandler.Download.DownloadUploadFile(paracredentials, text, contentType, FileName);
            Guid = Attachment.Guid;
            Name = FileName;
        }


        /// <summary>
        /// If you have a download file and would like to replace the file, use this method. It will actually delete 
        /// the existing attachment, and then add a new one to replace it.
        /// </summary>
        public void AttachmentsUpdate(ParaCredentials paracredentials, Byte[] Attachment, string contentType, string FileName)
        {
            this.Attachment = ApiHandler.Download.DownloadUploadFile(paracredentials, Attachment, contentType, FileName);
            Guid = this.Attachment.Guid;
            Name = FileName;
        }

        public Download()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowMultipleFolders">True if Multiple Folders is configured</param>
        public Download(bool allowMultipleFolders)
        {
            MultipleFolders = allowMultipleFolders;
        }

        public Download(Download download)
            : base(download)
        {
            Id = download.Id;
            Date_Created = download.Date_Created;
            Date_Updated = download.Date_Updated;
            Description = download.Description;
            External_Link = download.External_Link;
            MultipleFolders = download.MultipleFolders;
            Folders = download.Folders;
            Guid = download.Guid;
            Permissions = new List<Sla>(download.Permissions);
            Name = download.Name;
            Products = new List<Product>(download.Products);
            Published = download.Published;
            Title = download.Title;
            Visible = download.Visible;
            Eula = new Eula(download.Eula);
            File_Hits = download.File_Hits;
            File_Size = download.File_Size;
        }

        public override string GetReadableName()
        {
            return Title;
        }
    }
}