using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using ParatureSDK.ParaObjects;
using ParatureSDK.Query;
using ParatureSDK.Query.ModuleQuery;
using ParatureSDK.XmlToObjectParser;
using Action = ParatureSDK.ParaObjects.Action;

namespace ParatureSDK.ApiHandler
{
    internal static class ApiUtils
    {
        /// <summary>
        /// Internal Method to run an Action, independently from the module.
        /// </summary>
        internal static ApiCallResponse ActionRun<TEntity>(Int64 objectId, Action action, ParaCredentials pc)
            where TEntity : ParaEntity
        {
            var doc = XmlGenerator.GenerateActionXml<TEntity>(action);
            var ar = ApiCallFactory.ObjectCreateUpdate<TEntity>(pc, doc, objectId);
            return ar;
        }

        /// <summary>
        /// Internal method to attach a file for the Parature entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pc"></param>
        /// <param name="attachment"></param>
        /// <returns></returns>
        internal static Attachment UploadFile<TEntity>(ParaCredentials pc, System.Net.Mail.Attachment attachment)
            where TEntity : ParaEntity
        {
            var postUrlR = ApiCallFactory.FileUploadGetUrl<TEntity>(pc);
            var uploadUrlDoc = postUrlR.XmlReceived;
            var postUrl = AttachmentGetUrlToPost(uploadUrlDoc);

            var upresp = ApiCallFactory.FilePerformUpload(postUrl, attachment);

            var attaDoc = upresp.XmlReceived;

            var attach = ParaEntityParser.EntityFill<Attachment>(attaDoc);
            return attach;
        }

        /// <summary>
        /// Internal method to handle the upload of a file to Parature.
        /// </summary>
        internal static Attachment UploadFile<TEntity>(ParaCredentials pc, string text,
            string contentType, string fileName) where TEntity : ParaEntity
        {
            var encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes(text);
            return UploadFile<TEntity>(pc, bytes, contentType, fileName);
        }

        /// <summary>
        /// Internal method to handle the upload of a file to Parature.
        /// </summary>
        internal static Attachment UploadFile<TEntity>(ParaCredentials pc, Byte[] attachment,
            String contentType, String fileName) where TEntity : ParaEntity
        {
            Attachment attach;
            var postUrl = "";
            postUrl = AttachmentGetUrlToPost(ApiCallFactory.FileUploadGetUrl<TEntity>(pc).XmlReceived);

            if (String.IsNullOrEmpty(postUrl) == false)
            {
                var uploadResponse =
                    ApiCallFactory.FilePerformUpload(postUrl, attachment, contentType, fileName)
                        .XmlReceived;

                attach = new Attachment();

                var uploadResult = ParaEntityParser.EntityFill<UploadResult>(uploadResponse);

                if (!string.IsNullOrEmpty(uploadResult.Error))
                {
                    //There was an error uploading
                    attach.HasException = true;
                    attach.Error = uploadResult.Error;
                } else {
                    attach.Name = uploadResult.Result.File.FileName;
                    attach.Guid = uploadResult.Result.File.Guid;
                }   
            }
            else
            {
                attach = new Attachment();
            }
            return attach;
        }

        /// <summary>
        /// Remove the static fields, which screw up the deserializer when fetching the schema
        /// </summary>
        /// <param name="xmlReceived"></param>
        /// <returns></returns>
        internal static XmlDocument RemoveStaticFieldsNodes(XmlDocument xmlReceived)
        {
            var xmlPurged = XDocument.Parse(xmlReceived.OuterXml);
            xmlPurged.Root.Elements()
                .Where(node => node.Name.LocalName != "Custom_Field")
                .Remove();

            var xmlDocument = new XmlDocument();
            using (var xmlReader = xmlPurged.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

        /// <summary>
        /// Fills a Role list object
        /// </summary>
        /// <param name="creds"></param>
        /// <param name="query"></param>
        /// <param name="module"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal static ParaEntityList<TEntity> ApiGetEntityList<TModule, TEntity>(ParaCredentials creds,
            ParaQuery query)
            where TModule : ParaEntity
            where TEntity : ParaEntityBaseProperties
        {
            var rolesList = new ParaEntityList<TEntity>();
            var ar = ApiCallFactory.ObjectSecondLevelGetList<TModule, TEntity>(creds, query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                //...Customer/status is sending "entities" not "Entities", which breaks the parser. Unwind and fix the XML
                var xmlStr = ar.XmlReceived.OuterXml;
                if (xmlStr.Contains("<entities"))
                {
                    xmlStr = xmlStr.Replace("<entities", "<Entities");
                    xmlStr = xmlStr.Replace("entities>", "Entities>");
                    ar.XmlReceived = ParseXmlDoc(xmlStr);
                }

                rolesList = ParaEntityParser.FillList<TEntity>(ar.XmlReceived);
            }
            rolesList.ApiCallResponse = ar;

            // Checking if the system needs to recursively call all of the data returned.
            if (query.RetrieveAllRecords)
            {
                var continueCalling = true;
                while (continueCalling)
                {
                    if (rolesList.TotalItems > rolesList.Data.Count)
                    {
                        // We still need to pull data
                        // Getting next page's data
                        query.PageNumber = query.PageNumber + 1;

                        ar = ApiCallFactory.ObjectSecondLevelGetList<TModule, TEntity>(creds,
                            query.BuildQueryArguments());

                        var objectlist = ParaEntityParser.FillList<TEntity>(ar.XmlReceived);

                        if (objectlist.Data.Count == 0)
                        {
                            continueCalling = false;
                        }

                        rolesList.Data.AddRange(objectlist.Data);
                        rolesList.ResultsReturned = rolesList.Data.Count;
                        rolesList.PageNumber = query.PageNumber;
                    }
                    else
                    {
                        // That is it, pulled all the items.
                        continueCalling = false;
                        rolesList.ApiCallResponse = ar;
                    }
                }
            }

            return rolesList;
        }

        internal static ParaEntityList<T> ApiGetEntityList<T>(ParaCredentials pc)
        {
            var entityList = new ParaEntityList<T>();
            var ar = ApiCallFactory.ObjectGetList<T>(pc, new ArrayList());
            if (ar.HasException == false)
            {
                entityList = ParaEntityParser.FillList<T>(ar.XmlReceived);
            }
            entityList.ApiCallResponse = ar;

            return entityList;
        }


        internal static ParaEntityList<ParaObjects.Download> ApiGetDownloadEntityList(ParaCredentials pc)
        {
            var entityList = new ParaEntityList<ParaObjects.Download>();
            var ar = ApiCallFactory.ObjectGetList<ParaObjects.Download>(pc, new ArrayList());
            if (ar.HasException == false)
            {
                entityList = ParaEntityParser.FillListDownload(ar.XmlReceived);
            }
            entityList.ApiCallResponse = ar;

            return entityList;
        }

        internal static ParaEntityList<ParaObjects.Download> ApiGetDownloadEntityList(ParaCredentials pc, DownloadQuery query)
        {

            var entityList = new ParaEntityList<ParaObjects.Download>();

            // Checking if the system needs to recursively call all of the data returned.
            if (query.RetrieveAllRecords)
            {
                entityList = RetrieveAllDownloadEntities(pc, query);
            }
            else
            {
                var ar = ApiCallFactory.ObjectGetList<ParaObjects.Download>(pc, query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    entityList = ParaEntityParser.FillListDownload(ar.XmlReceived);
                }
                entityList.ApiCallResponse = ar;
            }

            return entityList;
        }

        /// <summary>
        /// Fills a main module's list object.
        /// </summary>
        internal static ParaEntityList<T> ApiGetEntityList<T>(ParaCredentials pc, ParaEntityQuery query)
            where T : ParaEntity, new()
        {
            var entityList = new ParaEntityList<T>();

            // Checking if the system needs to recursively call all of the data returned.
            if (query.RetrieveAllRecords)
            {
                entityList = RetrieveAllEntities<T>(pc, query);
            }
            else
            {
                var ar = ApiCallFactory.ObjectGetList<T>(pc, query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    entityList = ParaEntityParser.FillList<T>(ar.XmlReceived);
                }
                entityList.ApiCallResponse = ar;
            }

            return entityList;
        }

        private static ParaEntityList<ParaObjects.Download> RetrieveAllDownloadEntities(ParaCredentials pc, DownloadQuery query)
        {
            ApiCallResponse ar;
            var downloadList = new ParaEntityList<ParaObjects.Download>();

            ar = ApiCallFactory.ObjectGetList<ParaObjects.Download>(pc, query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                downloadList = ParaEntityParser.FillListDownload(ar.XmlReceived);
            }
            downloadList.ApiCallResponse = ar;

            var continueCalling = true;
            while (continueCalling)
            {
                if (downloadList.TotalItems > downloadList.Data.Count)
                {
                    // We still need to pull data
                    // Getting next page's data
                    query.PageNumber = query.PageNumber + 1;

                    ar = ApiCallFactory.ObjectGetList<ParaObjects.Download>(pc, query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        var objectlist = ParaEntityParser.FillListDownload(ar.XmlReceived);
                        downloadList.Data.AddRange(objectlist.Data);
                        downloadList.ResultsReturned = downloadList.Data.Count;
                        downloadList.PageNumber = query.PageNumber;
                    }
                    else
                    {
                        // There is an error processing request
                        downloadList.ApiCallResponse = ar;
                        continueCalling = false;
                    }
                }
                else
                {
                    // That is it, pulled all the items.
                    continueCalling = false;
                    downloadList.ApiCallResponse = ar;
                }
            }

            return downloadList;
        }

        private static ParaEntityList<T> RetrieveAllEntities<T>(ParaCredentials pc, ParaEntityQuery query) 
            where T : ParaEntity, new()
        {
            ApiCallResponse ar;
            var entityList = new ParaEntityList<T>();

            ar = ApiCallFactory.ObjectGetList<T>(pc, query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                entityList = ParaEntityParser.FillList<T>(ar.XmlReceived);
            }
            entityList.ApiCallResponse = ar;

            var continueCalling = true;
            while (continueCalling)
            {
                if (entityList.TotalItems > entityList.Data.Count)
                {
                    // We still need to pull data
                    // Getting next page's data
                    query.PageNumber = query.PageNumber + 1;

                    ar = ApiCallFactory.ObjectGetList<T>(pc, query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        var objectlist = ParaEntityParser.FillList<T>(ar.XmlReceived);
                        entityList.Data.AddRange(objectlist.Data);
                        entityList.ResultsReturned = entityList.Data.Count;
                        entityList.PageNumber = query.PageNumber;
                    }
                    else
                    {
                        // There is an error processing request
                        entityList.ApiCallResponse = ar;
                        continueCalling = false;
                    }
                }
                else
                {
                    // That is it, pulled all the items.
                    continueCalling = false;
                    entityList.ApiCallResponse = ar;
                }
            }

            return entityList;
        }

        /// <summary>
        /// Retrieve the details for a specific Parature module entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pc"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        internal static T ApiGetEntity<T>(ParaCredentials pc, long entityId)
            where T : ParaEntityBaseProperties, new()
        {
            var entity = new T();
            var req = ApiCallFactory.ObjectGetDetail<T>(pc, entityId);
            if (req.HasException == false)
            {
                entity = ParaEntityParser.EntityFill<T>(req.XmlReceived);
                entity.FullyLoaded = true;
            }
            else
            {
                entity.FullyLoaded = false;
                entity.Id = 0;
            }
            entity.ApiCallResponse = req;
            entity.IsDirty = false;
            return entity;
        }

        /// <summary>
        /// Retrieve the details for a specific Parature module entity with custom query string arguments
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pc"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        internal static T ApiGetEntity<T>(ParaCredentials pc, long entityId, ArrayList arl) where T : ParaEntityBaseProperties, new()
        {
            var entity = new T();
            var req = ApiCallFactory.ObjectGetDetail<T>(pc, entityId, arl);
            if (req.HasException == false)
            {
                entity = ParaEntityParser.EntityFill<T>(req.XmlReceived);
                entity.FullyLoaded = true;
            }
            else
            {
                entity.FullyLoaded = false;
                entity.Id = 0;
            }
            entity.ApiCallResponse = req;
            entity.IsDirty = false;
            return entity;
        }

        /// <summary>
        /// Retrieve the details for a specific Parature module entity with custom query string arguments
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pc"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        internal static ParaObjects.Download ApiGetDownloadEntity(ParaCredentials pc, long entityId,
            ArrayList arl)
        {
            var entity = new ParaObjects.Download();
            var req = ApiCallFactory.ObjectGetDetail<ParaObjects.Download>(pc, entityId, arl);
            if (req.HasException == false)
            {
                entity = ParaEntityParser.EntityFillDownload(req.XmlReceived);
                entity.FullyLoaded = true;
            }
            else
            {
                entity.FullyLoaded = false;
                entity.Id = 0;
            }
            entity.ApiCallResponse = req;
            entity.IsDirty = false;
            return entity;
        }

        private static XmlDocument ParseXmlDoc(string xmlDoc)
        {
            var xml = new XmlDocument();
            xml.LoadXml(xmlDoc);

            return xml;
        }

        internal static string AttachmentGetUrlToPost(XmlDocument doc)
        {
            if (doc != null && doc.DocumentElement.HasAttribute("href"))
            {
                return doc.DocumentElement.Attributes["href"].InnerText;
            }
            else
            {
                throw new Exception(doc.OuterXml);
            }
        }
    }
}