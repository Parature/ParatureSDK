using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;
using Action = ParatureSDK.ParaObjects.Action;

namespace ParatureSDK.ApiHandler
{
    internal static class ApiUtils
    {
        /// <summary>
        /// Internal Method to run an Action, independently from the module.
        /// </summary>
        public static ApiCallResponse ActionRun(Int64 objectId, Action action, ParaCredentials pc,
            ParaEnums.ParatureModule module)
        {
            var doc = XmlGenerator.GenerateXml(action, module);
            var ar = ApiCallFactory.ObjectCreateUpdate(pc, module, doc, objectId);
            return ar;
        }

        public static Attachment UploadFile(ParaEnums.ParatureModule module, ParaCredentials pc,
            System.Net.Mail.Attachment attachment)
        {
            var postUrlR = ApiCallFactory.FileUploadGetUrl(pc, module);
            var uploadUrlDoc = postUrlR.XmlReceived;
            var postUrl = AttachmentGetUrlToPost(uploadUrlDoc);

            var upresp = ApiCallFactory.FilePerformUpload(postUrl, attachment, pc.Instanceid, pc);

            var attaDoc = upresp.XmlReceived;

            var attach = ParaEntityParser.EntityFill<Attachment>(attaDoc);
            return attach;
        }

        /// <summary>
        /// Internal method to handle the upload of a file to Parature.
        /// </summary>
        public static Attachment UploadFile(ParaEnums.ParatureModule module, ParaCredentials pc, string text,
            string contentType, string fileName)
        {
            var encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes(text);
            return UploadFile(module, pc, bytes, contentType, fileName);
        }

        /// <summary>
        /// Internal method to handle the upload of a file to Parature.
        /// </summary>
        public static Attachment UploadFile(ParaEnums.ParatureModule module, ParaCredentials pc, Byte[] attachment,
            String contentType, String fileName)
        {
            Attachment attach;
            var postUrl = "";
            postUrl = AttachmentGetUrlToPost(ApiCallFactory.FileUploadGetUrl(pc, module).XmlReceived);

            if (String.IsNullOrEmpty(postUrl) == false)
            {
                var attaDoc =
                    ApiCallFactory.FilePerformUpload(postUrl, attachment, contentType, fileName, pc.Instanceid, pc)
                        .XmlReceived;
                attach = ParaEntityParser.EntityFill<Attachment>(attaDoc);
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
        public static XmlDocument RemoveStaticFieldsNodes(XmlDocument xmlReceived)
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
        internal static ParaEntityList<T> ApiGetEntityList<T>(ParaCredentials creds, ParaQuery query,
            ParaEnums.ParatureModule module, ParaEnums.ParatureEntity entity)
        {
            var rolesList = new ParaEntityList<T>();
            var ar = ApiCallFactory.ObjectSecondLevelGetList(creds, module, entity, query.BuildQueryArguments());
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

                rolesList = ParaEntityParser.FillList<T>(ar.XmlReceived);
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

                        ar = ApiCallFactory.ObjectSecondLevelGetList(creds, module, entity, query.BuildQueryArguments());

                        var objectlist = ParaEntityParser.FillList<T>(ar.XmlReceived);

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

        internal static T ApiGetEntity<T>(ParaCredentials pc, ParaEnums.ParatureEntity entityType, long entityId)
            where T : ParaEntityBaseProperties, new()
        {
            var role = new T();
            var ar = ApiCallFactory.ObjectGetDetail(pc, entityType, entityId);
            if (ar.HasException == false)
            {
                role = ParaEntityParser.EntityFill<T>(ar.XmlReceived);
            }
            else
            {
                role.Id = 0;
            }
            return role;
        }

        internal static ParaEntityList<T> ApiGetEntityList<T>(ParaCredentials pc, ParaEnums.ParatureModule module)
        {
            var entityList = new ParaEntityList<T>();
            var ar = ApiCallFactory.ObjectGetList(pc, module, new ArrayList());
            if (ar.HasException == false)
            {
                entityList = ParaEntityParser.FillList<T>(ar.XmlReceived);
            }
            entityList.ApiCallResponse = ar;

            return entityList;
        }


        internal static ParaEntityList<ParaObjects.Download> ApiGetDownloadEntityList(ParaCredentials pc, ParaEnums.ParatureModule module)
        {
            var entityList = new ParaEntityList<ParaObjects.Download>();
            var ar = ApiCallFactory.ObjectGetList(pc, module, new ArrayList());
            if (ar.HasException == false)
            {
                entityList = ParaEntityParser.FillListDownload(ar.XmlReceived);
            }
            entityList.ApiCallResponse = ar;

            return entityList;
        }

        /// <summary>
        /// Fills a main module's list object.
        /// </summary>
        internal static ParaEntityList<T> ApiGetEntityList<T>(ParaCredentials pc, ParaEnums.ParatureModule module,
            ParaEntityQuery query) where T : ParaEntity, new()
        {
            ApiCallResponse ar;
            var entityList = new ParaEntityList<T>();

            ar = ApiCallFactory.ObjectGetList(pc, module, query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                entityList = ParaEntityParser.FillList<T>(ar.XmlReceived);
            }
            entityList.ApiCallResponse = ar;

            // Checking if the system needs to recursively call all of the data returned.
            if (query.RetrieveAllRecords && !ar.HasException)
            {
                // A flag variable to check if we need to make more calls
                if (query.OptimizeCalls)
                {
                    var callsRequired =
                        (int) Math.Ceiling((double) (entityList.TotalItems/(double) entityList.PageSize));
                    for (var i = 2; i <= callsRequired; i++)
                    {
                        query.PageNumber = i;
                        //implement semaphore right here (in the thread pool instance to control the generation of threads
                        var instance = new ThreadPool.ObjectList(pc, module, query.BuildQueryArguments());
                        var t = new Thread(() => instance.Go(entityList));
                        t.Start();
                    }

                    while (entityList.TotalItems > entityList.Data.Count)
                    {
                        Thread.Sleep(500);
                    }

                    entityList.ResultsReturned = entityList.Data.Count;
                    entityList.PageNumber = callsRequired;
                }
                else
                {
                    var continueCalling = true;
                    while (continueCalling)
                    {
                        if (entityList.TotalItems > entityList.Data.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            query.PageNumber = query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(pc, module, query.BuildQueryArguments());
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
                }
            }

            return entityList;
        }

        /// <summary>
        /// Retrieve the details for a specific Parature module entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pc"></param>
        /// <param name="module"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        internal static T ApiGetEntity<T>(ParaCredentials pc, ParaEnums.ParatureModule module, long entityId)
            where T : ParaEntity, new()
        {
            var entity = new T();
            var req = ApiCallFactory.ObjectGetDetail<T>(pc, module, entityId);
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
        /// <param name="module"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        internal static T ApiGetEntity<T>(ParaCredentials pc, ParaEnums.ParatureModule module, long entityId,
            ArrayList arl) where T : ParaEntity, new()
        {
            var entity = new T();
            var req = ApiCallFactory.ObjectGetDetail<T>(pc, module, entityId, arl);
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
        /// <param name="module"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        internal static ParaObjects.Download ApiGetDownloadEntity(ParaCredentials pc, ParaEnums.ParatureModule module, long entityId,
            ArrayList arl)
        {
            var entity = new ParaObjects.Download();
            var req = ApiCallFactory.ObjectGetDetail<ParaObjects.Download>(pc, module, entityId, arl);
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

        static internal string AttachmentGetUrlToPost(XmlDocument doc)
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