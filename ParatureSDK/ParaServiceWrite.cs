using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ParatureSDK
{
    public partial class ParaService
    {
        /// <summary>
        /// Create a new entity object. This object is not saved to the server until you call Insert with it.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public TEntity Create<TEntity>() where TEntity : ParaEntityBaseProperties, new()
        {
            var entity = new TEntity();
            var ar = ApiCallFactory.ObjectGetSchema<TEntity>(Credentials);

            if (ar.HasException == false)
            {
                var purgedSchema = ApiUtils.RemoveStaticFieldsNodes(ar.XmlReceived);
                entity = ParaEntityParser.EntityFill<TEntity>(purgedSchema);
            }

            entity.ApiCallResponse = ar;
            return entity;
        }

        /// <summary>
        /// Adds or updates the entity on the server.
        /// </summary>
        /// <param name="entity">The entity to save</param>
        /// <returns></returns>
        public ApiCallResponse Insert(IMutableEntity entity)
        {
            var pe = entity as ParaEntity;
            ApiCallResponse reply;

            //Check if the object is a ParaEntity, if not its a folder
            if (pe == null)
            {
                var folder = entity as Folder;
                if (folder == null)
                {
                    throw new ArgumentException("You can only call this function on a Folder-derived or ParaEntity-derived object.", "entity");
                }

                reply = ApiCallFactory.ObjectCreateUpdate(Credentials, folder.GetType().Name, XmlGenerator.GenerateXml(folder), 0);
                folder.Id = reply.Id;
            }
            else
            {
                reply = ApiCallFactory.ObjectCreateUpdate(Credentials, pe.GetType().Name, XmlGenerator.GenerateXml(pe), 0);
                pe.Id = reply.Id;
            }

            return reply;
        }

        /// <summary>
        /// Adds or updates the entity on the server.
        /// </summary>
        /// <param name="entity">The entity to save</param>
        /// <returns></returns>
        public ApiCallResponse Update(IMutableEntity entity)
        {
            var pe = entity as ParaEntity;

            Folder folder = null;
            ApiCallResponse reply = null;

            if (pe == null)
            {
                folder = entity as Folder;
                if (folder == null)
                {
                    throw new ArgumentException("You can only call this function on a Folder-derived or ParaEntity-derived object.", "entity");
                }

                reply = ApiCallFactory.ObjectCreateUpdate(Credentials, folder.GetType().Name, XmlGenerator.GenerateXml(folder), folder.Id);
            }
            else
            {
                if (pe.Id == 0)
                {
                    throw new ArgumentException("The update operation requires an existing object ID. Populate the entity ID to perform an update.");
                }

                reply = ApiCallFactory.ObjectCreateUpdate(Credentials, pe.GetType().Name, XmlGenerator.GenerateXml(pe), pe.Id);
            }

            return reply;
        }

        /// <summary>
        /// Deletes the entity from the server.
        /// </summary>
        /// <param name="purge">To delete the entity permanently or not (to the trash instead)</param>
        /// <param name="id">The object ID of the object to trash/delete</param>
        /// <returns></returns>
        public ApiCallResponse Delete<TEntity>(long id, bool purge)
            where TEntity : ParaEntityBaseProperties, new()
        {
            return ApiCallFactory.ObjectDelete<TEntity>(Credentials, id, purge);
        }

        public ApiCallResponse RunActionOn<TEntity>(long entityId, ParaObjects.Action action)
            where TEntity : ParaEntity, IActionRunner, new()
        {
            return ApiUtils.ActionRun<TEntity>(entityId, action, Credentials);
        }


        /// <summary>
        /// Method to attach a file for the Parature entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pc"></param>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public Attachment UploadFile<TEntity>(System.Net.Mail.Attachment attachment)
            where TEntity : ParaEntity
        {
            var postUrlR = ApiCallFactory.FileUploadGetUrl<TEntity>(Credentials);
            var uploadUrlDoc = postUrlR.XmlReceived;
            var postUrl = AttachmentGetUrlToPost(uploadUrlDoc);

            var upresp = ApiCallFactory.FilePerformUpload(postUrl, attachment);

            var attaDoc = upresp.XmlReceived;

            var attach = ParaEntityParser.EntityFill<Attachment>(attaDoc);
            return attach;
        }

        /// <summary>
        /// Method to handle the upload of a file to Parature.
        /// </summary>
        public Attachment UploadFile<TEntity>(Byte[] attachment, String contentType, String fileName)
            where TEntity : ParaEntity
        {
            Attachment attach;
            var postUrl = "";
            postUrl = AttachmentGetUrlToPost(ApiCallFactory.FileUploadGetUrl<TEntity>(Credentials).XmlReceived);

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
                }
                else
                {
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
