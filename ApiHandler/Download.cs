using System;
using System.Collections;
using System.Threading;
using System.Xml;
using ParatureSDK.ApiHandler.ApiMethods;
using ParatureSDK.EntityQuery;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Download module.
    /// </summary>
    public class Download : FirstLevelApiMethods<ParaObjects.Download, DownloadQuery>
    {
        /// <summary>
        /// Returns an entity object from an XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="xml">
        /// The entity XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.Download GetDetails(XmlDocument xml)
        {
            var entity = ParaEntityParser.EntityFillDownload(xml);
            entity.FullyLoaded = true;

            entity.ApiCallResponse.XmlReceived = xml;
            entity.ApiCallResponse.Id = entity.Id;

            entity.IsDirty = false;
            return entity;
        }

        /// <summary>
        /// Returns an object with all the properties of the entity.
        /// </summary>
        /// <param name="entityId">
        ///The entity number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="pc">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        public static ParaObjects.Download GetDetails(Int64 entityId, ParaCredentials pc)
        {
            return GetDetails(entityId, pc, new ArrayList());
        }

        public static ParaObjects.Download GetDetails(Int64 entityId, ParaCredentials pc, ArrayList queryStringParams)
        {
            var entity = ApiUtils.ApiGetDownloadEntity(pc, entityId, queryStringParams);

            return entity;
        }

        /// <summary>
        /// Will return the first 25 accounts returned by the APIs.
        /// </summary>            
        public static ParaEntityList<ParaObjects.Download> GetList(ParaCredentials pc)
        {
            return ApiUtils.ApiGetDownloadEntityList(pc);
        }

        /// <summary>
        /// Provides you with the capability to list accounts, following criteria you would set
        /// by instantiating a ModuleQuery.AccountQuery object
        /// </summary>
        public static ParaEntityList<ParaObjects.Download> GetList(ParaCredentials pc, DownloadQuery query)
        {
            // Making a schema call and returning all custom fields to be included in the call.
            if (query.IncludeAllCustomFields)
            {
                var objschem = Schema(pc);
                query.IncludeCustomField(objschem.CustomFields);
            }

            return ApiUtils.ApiGetEntityList<ParaObjects.Download>(pc, query);
        }

        /// <summary>
        /// Returns an accounts list object from an XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="listXml">
        /// The Account List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.Download> GetList(XmlDocument listXml)
        {
            var accountsList = ParaEntityParser.FillListDownload(listXml);

            accountsList.ApiCallResponse.XmlReceived = listXml;

            return accountsList;
        }

        internal static Attachment DownloadUploadFile(ParaCredentials creds, string text, string contentType, string fileName)
        {
            return ApiUtils.UploadFile<ParaObjects.Download>(creds, text, contentType, fileName);
        }

        internal static Attachment DownloadUploadFile(ParaCredentials creds, Byte[] attachment, string contentType, string fileName)
        {
            return ApiUtils.UploadFile<ParaObjects.Download>(creds, attachment, contentType, fileName);
        }

        internal static Attachment DownloadUploadFile(ParaCredentials creds, System.Net.Mail.Attachment attachment)
        {
            return ApiUtils.UploadFile<ParaObjects.Download>(creds, attachment);
        }

        /// <summary>
        /// Modify the XML so it can be parsed, and check to see if it supports multiple folders
        /// </summary>
        /// <param name="xmlReceived"></param>
        /// <returns></returns>
        internal static bool HasMultipleFoldersAndConvert(XmlDocument xmlReceived)
        {
            var foldersNode = xmlReceived.SelectSingleNode("/Download/Folders");
            bool hasMultipleFolders;
            if (foldersNode != null)
            {
                hasMultipleFolders = true;
            }
            else
            {
                var singleFolderNode = xmlReceived.SelectSingleNode("/Download/Folder");
                if (singleFolderNode != null && singleFolderNode.OwnerDocument != null && singleFolderNode.ParentNode != null)
                {
                    //replace the <Folder> with <Folders> for our parser
                    var dlFolders = singleFolderNode.InnerXml;
                    var doc = new XmlDocument();
                    doc.LoadXml(string.Format("<Folders>{0}</Folders>", dlFolders));
                    var newNode = singleFolderNode.OwnerDocument.ImportNode(doc.DocumentElement, true);
                    singleFolderNode.ParentNode.ReplaceChild(newNode, singleFolderNode);
                }
                hasMultipleFolders = false;
            }
            return hasMultipleFolders;
        }

        /// <summary>
        /// Contains all the methods needed to work with the download module's folders.
        /// </summary>
        public class DownloadFolder : FolderApiMethods<ParaObjects.DownloadFolder, DownloadFolderQuery>
        {}
    }
}