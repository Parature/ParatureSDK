using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects
{
    /// <summary>
    /// Result of uploading a file
    /// </summary>
    [XmlRoot("result")]
    public class UploadResult
    {
        [XmlElement("passed")]
        public UploadInnerResult Result { get; set; }
        [XmlElement("error")]
        public string Error { get; set; }
    }

    /// <summary>
    /// Indicates if the upload was successful or not
    /// </summary>
    public class UploadInnerResult
    {
        [XmlElement("file")]
        public UploadFile File { get; set; }
    }

    /// <summary>
    /// Object for file metadata. Only used when uploading files
    /// </summary>
    public class UploadFile
    {
        [XmlElement("inputname")]
        public string InputName { get; set; }
        
        [XmlElement("filename")]
        public string FileName { get; set; }
        
        [XmlElement("guid")]
        public string Guid { get; set; }
    }
}
