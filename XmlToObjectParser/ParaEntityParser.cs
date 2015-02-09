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
            var wrapperElements = xml.Root.Elements().Where(node => node.HasElements);
            foreach (var wrapper in wrapperElements)
            {
                //Name that will be used in the lookup of the field. It is the element text
                var wrapperTagName = wrapper.Name.ToString();
                var field = new StaticField();

                //deserialize static fields which are found in an 
                #region switch on tags for serialization of Static Fields
                switch (wrapperTagName)
                {
                    //customer
                    case "Account":
                        field.DataType = ParaEnums.FieldDataType.EntityReference;
                        field.Value = OverrideNodeFill<Account>(wrapper);
                        break;
                    case "Customer_Role":
                        field.DataType = ParaEnums.FieldDataType.Sla;
                        field.Value = OverrideNodeFill<Sla>(wrapper);
                        break;
                    case "Status": //Asset, customer, chat
                        field.DataType = ParaEnums.FieldDataType.Status;
                        field.Value = OverrideNodeFill<Status>(wrapper);
                        break;
                    //customer or account
                    case "Sla":
                        field.DataType = ParaEnums.FieldDataType.Sla;
                        field.Value = OverrideNodeFill<Sla>(wrapper);
                        break;
                    //download
                    case "Eula":
                        field.DataType = ParaEnums.FieldDataType.Eula;
                        field.Value = OverrideNodeFill<Eula>(wrapper);
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
                                    field.DataType = ParaEnums.FieldDataType.Folder;
                                    field.Value = OverrideNodeFill<List<DownloadFolder>>(wrapper);
                                    break;
                                case "ArticleFolder":
                                    field.DataType = ParaEnums.FieldDataType.Folder;
                                    field.Value = OverrideNodeFill<List<ArticleFolder>>(wrapper);
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
                                    field.DataType = ParaEnums.FieldDataType.Folder;
                                    field.Value = OverrideNodeFill<List<DownloadFolder>>(wrapper);
                                    break;
                                case "ProductFolder":
                                    field.DataType = ParaEnums.FieldDataType.Folder;
                                    field.Value = OverrideNodeFill<List<ProductFolder>>(wrapper);
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    //Articles
                    case "Created_By": //also products
                        field.DataType = ParaEnums.FieldDataType.EntityReference;
                        field.Value = OverrideNodeFill<Csr>(wrapper);
                        break;
                    //Products
                    case "Customer_Owner":
                        field.DataType = ParaEnums.FieldDataType.EntityReference;
                        field.Value = OverrideNodeFill<Customer>(wrapper);
                        break;
                    //Asset
                    case "Product":
                        field.DataType = ParaEnums.FieldDataType.EntityReference;
                        field.Value = OverrideNodeFill<Product>(wrapper);
                        break;
                    //chat
                    case "Initial_Csr":
                        field.DataType = ParaEnums.FieldDataType.EntityReference;
                        field.Value = OverrideNodeFill<Csr>(wrapper);
                        break;
                    default:
                        break;
                }
                #endregion
                        
                //Check whether the wrapped Static Field was properly deserialized
                if (entity != null)
                {
                    field.Name = wrapperTagName;
                    entity.Fields.Add(field);
                }
            }

            var customFieldNodes = xml.Root.Elements().Where(node => node.Name.ToString() == "Custom_Field");
            var newRoot = new XDocument();
            foreach (var element in customFieldNodes)
            {
                var custFieldSerializer = new XmlSerializer(typeof(CustomField));
                var custField = custFieldSerializer.Deserialize(element.CreateReader()) as CustomField;

                var text = element.Value;
                if (string.IsNullOrEmpty(text) == false
                    && custField.DataType != null)
                {
                    switch (custField.DataType)
                    {
                        case ParaEnums.FieldDataType.String:
                            custField.Value = text;
                            break;
                        default:
                            break;
                    }
                }

                entity.Fields.Add(custField);
            }

            return entity;
        }

        /// <summary>
        /// Retrieves an Entity Object from an Entity Node
        /// </summary>
        /// <typeparam name="T">Type of Entity</typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        public static T OverrideNodeFill<T>(XElement node)
        {
            var childNode = node.FirstNode as XElement;
            var root = new XmlRootAttribute
            {
                ElementName = childNode.Name.ToString(), 
                IsNullable = true
            };
            //Our entity is the first node from a parent entity
            var xmlSerializer = new XmlSerializer(typeof(T), root);
            return (T)xmlSerializer.Deserialize(node.FirstNode.CreateReader());
        }
    }
}
