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

        /// <summary>
        /// To avoid infinite loops, the parent folder is not instantiated when 
        /// you instantiate a new ProductFolder object. In the case you are creating a download folder, please make sure to create a new ProductFolder, 
        /// set just the id of the folder, then make the ParentFolder equals the one you just created.
        /// </summary>
        public Folder Parent_Folder;
    }
}