using ParatureSDK.ApiHandler;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Folder folder = null;
            ApiCallResponse reply = null;

            if (pe == null)
            {
                folder = entity as Folder;
                if (folder == null)
                {
                    throw new ArgumentException("You can only call this function on a Folder-derived or ParaEntity-derived object.", "entity");
                }
                else
                {
                    var doc = XmlGenerator.GenerateXml(folder);
                    reply = ApiCallFactory.ObjectCreateUpdate<Folder>(Credentials, doc, 0);
                    folder.Id = reply.Id;
                }
            }
            else
            {
                reply = ApiCallFactory.ObjectCreateUpdate(Credentials, pe.GetType().Name, XmlGenerator.GenerateXml(pe), pe.Id);
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
                else
                {
                    var doc = XmlGenerator.GenerateXml(folder);
                    reply = ApiCallFactory.ObjectCreateUpdate<Folder>(Credentials, doc, folder.Id);
                }
            }
            else
            {
                reply = ApiCallFactory.ObjectCreateUpdate(Credentials, pe.GetType().Name, XmlGenerator.GenerateXml(pe), pe.Id);
            }

            return reply;
        }

        /// <summary>
        /// Deletes the entity from the server.
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        /// <returns></returns>
        public ApiCallResponse Delete(IMutableEntity entity)
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
                else
                {
                    reply = ApiCallFactory.ObjectDelete<Folder>(Credentials, folder.Id, true);
                }
            }
            else
            {
                reply = ApiCallFactory.ObjectCreateUpdate(Credentials, pe.GetType().Name, XmlGenerator.GenerateXml(pe), pe.Id);
            }

            return reply;
        }

        /// <summary>
        /// Deletes the entity from the server.
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        /// <param name="purge">To delete the entity permanently or not (to the trash instead)</param>
        /// <returns></returns>
        public ApiCallResponse Delete<TEntity>(long id, bool purge)
            where TEntity : ParaEntityBaseProperties, new()
        {
            return ApiCallFactory.ObjectDelete<TEntity>(Credentials, id, purge);
        }
    }
}
