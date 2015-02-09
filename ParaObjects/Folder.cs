using System;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects
{
    public class Folder
    {
        // Specific properties for this module
        [XmlAttribute("id")]
        public Int64 Id = 0;
        public string Name = "";
        public string Description = "";
        public bool? Is_Private;

        /// <summary>
        /// Contains all the information regarding the API Call that was made.
        /// </summary>
        public ApiCallResponse ApiCallResponse = new ApiCallResponse();


        public Folder()
        {
        }

        public Folder(Folder folder)
        {
            Id = folder.Id;
            Name = folder.Name;
            Description = folder.Description;
            Is_Private = folder.Is_Private;
            ApiCallResponse = folder.ApiCallResponse;
        }

    }
}