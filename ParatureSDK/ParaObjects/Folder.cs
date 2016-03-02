using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ParaObjects
{
    public class Folder : ParaEntityBaseProperties, IMutableEntity, ParaXmlSerializer.IXmlDeserializationCallback
    {
        public string Name = "";
        public string Description = "";
        public bool Is_Private;

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

        /// <summary>
        /// Callback after Deserialization to set parent folder correctly. Implemented to preserve backwards compatability when creating folders.
        /// </summary>
        /// <param name="xml">The XML received</param>
        public void OnXmlDeserialization(XDocument xml)
        {
            try
            {
                //Find the parent folder
                var parentFolderNode = xml.Descendants("Parent_Folder").FirstOrDefault();
                var nameNode = parentFolderNode.Descendants("Name").FirstOrDefault();
                var parentFolder = XElement.Parse(parentFolderNode.FirstNode.ToString());

                Parent_Folder = new Folder();
                Parent_Folder.Name = nameNode.Value;
                Parent_Folder.Id = long.Parse(parentFolder.Attribute("id").Value);
            }
            catch (Exception e)
            {
                Parent_Folder = new Folder();
            }
            
        }
    }
}