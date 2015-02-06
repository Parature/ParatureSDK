using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using ParatureSDK.Fields;
using ParatureSDK.ParaObjects;

namespace ParatureSDK.XmlToObjectParser
{
    public class ParaEntityParser
    {
        public static T EntityFill<T>(XmlDocument xmlDoc) where T: ParaObjects.ParaEntity
        {
            var serializer = new XmlSerializer(typeof(T));
            var xml = XDocument.Parse(xmlDoc.OuterXml);
            var entity = serializer.Deserialize(xml.CreateReader()) as T;

            /*
             * Get the first level descendents. 
             * Some entities have a wrapper element around the XML node, which throws off deserialization
             */
            var wrapperElements = xml.Root.Descendants().Where(node => node.HasElements);
            foreach (var wrapper in wrapperElements)
            {
                //Name that will be used in the lookup of the field. It is the element text
                var wrapperTagName = wrapper.Name.ToString();
                XmlSerializer nodeSerializer;
                var field = new StaticField();
                var firstChild = wrapper.FirstNode;

                //deserialize static fields which are found in an 
                #region switch on tags for serialization
                switch (wrapperTagName)
                {
                    //Account
                    case "Default_Customer_Role":
                        nodeSerializer = new XmlSerializer(typeof (Sla));
                        field.DataType = ParaEnums.FieldDataType.Sla;
                        field.Value = nodeSerializer.Deserialize(firstChild.CreateReader()) as Sla;
                        break;
                    case "Modified_By": //Articles and Accounts both have this one
                        nodeSerializer = new XmlSerializer(typeof(Csr));
                        field.DataType = ParaEnums.FieldDataType.EntityReference;
                        field.Value = nodeSerializer.Deserialize(firstChild.CreateReader()) as Csr;
                        break;
                    case "Owned_By":
                        nodeSerializer = new XmlSerializer(typeof(Csr));
                        field.DataType = ParaEnums.FieldDataType.EntityReference;
                        field.Value = nodeSerializer.Deserialize(firstChild.CreateReader()) as Csr;
                        break;
                    //customer
                    case "Account":
                        nodeSerializer = new XmlSerializer(typeof(Account));
                        field.DataType = ParaEnums.FieldDataType.EntityReference;
                        field.Value = nodeSerializer.Deserialize(firstChild.CreateReader()) as Account;
                        break;
                    case "Customer_Role":
                        nodeSerializer = new XmlSerializer(typeof(Sla));
                        field.DataType = ParaEnums.FieldDataType.Sla;
                        field.Value = nodeSerializer.Deserialize(firstChild.CreateReader()) as Sla;
                        break;
                    case "Status": //Asset, customer, chat
                        nodeSerializer = new XmlSerializer(typeof(Status));
                        field.DataType = ParaEnums.FieldDataType.Status;
                        field.Value = nodeSerializer.Deserialize(firstChild.CreateReader()) as Status;
                        break;
                    //customer or account
                    case "Sla":
                        nodeSerializer = new XmlSerializer(typeof(Sla));
                        field.DataType = ParaEnums.FieldDataType.Sla;
                        field.Value = nodeSerializer.Deserialize(firstChild.CreateReader()) as Sla;
                        break;
                    //download
                    case "Eula":
                        nodeSerializer = new XmlSerializer(typeof(Eula));
                        field.DataType = ParaEnums.FieldDataType.Eula;
                        field.Value = nodeSerializer.Deserialize(firstChild.CreateReader()) as Eula;
                        break;
                    case "Folders": //Articles too
                        var folders = wrapper.Descendants().FirstOrDefault();
                        if (folders != null)
                        {
                            var folderName = folders.Name.ToString();
                            switch (folderName)
                            {
                                case "DownloadFolder":
                                    //TODO: Add field for "multipleFolders" too
                                    //nuanced. This can be a different node name depending on the dept configuration
                                    nodeSerializer = new XmlSerializer(typeof(List<DownloadFolder>));
                                    field.DataType = ParaEnums.FieldDataType.Folder;
                                    field.Value = nodeSerializer.Deserialize(wrapper.CreateReader()) as List<DownloadFolder>;
                                    break;
                                case "ArticleFolder":
                                    nodeSerializer = new XmlSerializer(typeof(List<ArticleFolder>));
                                    field.DataType = ParaEnums.FieldDataType.Folder;
                                    field.Value = nodeSerializer.Deserialize(wrapper.CreateReader()) as List<ArticleFolder>;
                                    break;
                                default:
                                    break;
                            }
                        }

                        break;
                    case "Folder":  //Products too
                        var folderWrapper = wrapper.Descendants().FirstOrDefault();
                        if (folderWrapper != null)
                        {
                            var folderName = folderWrapper.Name.ToString();
                            switch (folderName)
                            {
                                case "DownloadFolder":
                                    //TODO: Add field for "multipleFolders" too
                                    //nuanced. This can be a different node name depending on the dept configuration
                                    nodeSerializer = new XmlSerializer(typeof(List<DownloadFolder>));
                                    field.DataType = ParaEnums.FieldDataType.Folder;
                                    field.Value = nodeSerializer.Deserialize(firstChild.CreateReader()) as List<DownloadFolder>;
                                    break;
                                case "ProductFolder":
                                    nodeSerializer = new XmlSerializer(typeof(List<ProductFolder>));
                                    field.DataType = ParaEnums.FieldDataType.Folder;
                                    field.Value = nodeSerializer.Deserialize(firstChild.CreateReader()) as List<ProductFolder>;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //nuanced. This can be a different node name depending on the dept configuration
                        nodeSerializer = new XmlSerializer(typeof(DownloadFolder));
                        field.DataType = ParaEnums.FieldDataType.Folder;
                        field.Value = nodeSerializer.Deserialize(firstChild.CreateReader()) as DownloadFolder;
                        break;
                    //Articles
                    case "Created_By": //also products
                        nodeSerializer = new XmlSerializer(typeof(Csr));
                        field.DataType = ParaEnums.FieldDataType.EntityReference;
                        field.Value = nodeSerializer.Deserialize(firstChild.CreateReader()) as Csr;
                        break;
                    //Products
                    case "Customer_Owner":
                        nodeSerializer = new XmlSerializer(typeof(Customer));
                        field.DataType = ParaEnums.FieldDataType.EntityReference;
                        field.Value = nodeSerializer.Deserialize(firstChild.CreateReader()) as Customer;
                        break;
                    //Asset
                    case "Product":
                        nodeSerializer = new XmlSerializer(typeof(Product));
                        field.DataType = ParaEnums.FieldDataType.EntityReference;
                        field.Value = nodeSerializer.Deserialize(firstChild.CreateReader()) as Product;
                        break;
                    //chat
                    case "Initial_Csr":
                        nodeSerializer = new XmlSerializer(typeof(Product));
                        field.DataType = ParaEnums.FieldDataType.EntityReference;
                        field.Value = nodeSerializer.Deserialize(firstChild.CreateReader()) as Product;
                        break;
                    default:
                        break;
                }
                #endregion

                //Check whether the wrapped Static Field was properly deserialized
                if (entity != null && field.Value != null)
                {
                    entity.Fields.Add(field);
                }
            }

            return entity;
        }

        private static T OverrideNode<T>(XElement node, string newTagName)
        {
            return default(T);
        }
    }
}
