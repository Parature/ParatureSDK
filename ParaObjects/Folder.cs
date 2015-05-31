using System;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects
{
    public class Folder : ParaEntityBaseProperties
    {
        public string Name = "";
        public string Description = "";
        public bool? Is_Private;

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