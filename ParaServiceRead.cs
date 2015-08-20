using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ParatureSDK.ParaObjects;
using ParatureSDK.Query.ModuleQuery;
using ParatureSDK.XmlToObjectParser;
using System.Collections;
using ParatureSDK.Query;
using ParatureSDK.Query.EntityQuery;

namespace ParatureSDK
{
    public partial class ParaService
    {
        public readonly ParaCredentials Credentials;

        public ParaService(ParaCredentials credentials)
        {
            if (credentials == null)
            {
                throw new ArgumentNullException("credentials");
            }

            Credentials = credentials;
        }

        /// <summary>
        /// Returns a view object with all of its properties filled.
        /// </summary>
        /// <param name="id">
        /// The view number that you would like to get the details of.
        /// Value Type: <see cref="long" />   (System.Int64)
        ///</param>
        /// <returns></returns>
        public TEntity Get<TEntity>(long id) where TEntity : ParaEntity, new()
        {
            return ApiUtils.ApiGetEntity<TEntity>(Credentials, id);
        }

        public TEntity GetDetails<TEntity>(long entityId)
             where TEntity : ParaEntityBaseProperties, new()
        {
            
            return GetDetails<TEntity>(entityId, new ArrayList());
        }

        public TEntity GetDetailsWithHistory<TEntity>(long entityId)
            where TEntity : ParaEntityBaseProperties, IHistoricalEntity, new()
        {
            return GetDetailsWithHistory<TEntity>(entityId, new ArrayList());
        }

        private TEntity GetDetailsWithHistory<TEntity>(long entityId, ArrayList queryString)
            where TEntity : ParaEntityBaseProperties, IHistoricalEntity, new()
        {
            queryString.Add("_history_=true");

            return GetDetails<TEntity>(entityId, queryString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entityId"></param>
        /// <param name="pc"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public TEntity GetDetails<TEntity>(long entityId, ArrayList queryString)
            where TEntity : ParaEntityBaseProperties, new()
        {
            if (typeof(TEntity) != typeof(Folder))
            {
                return ApiUtils.ApiGetEntity<TEntity>(Credentials, entityId, queryString);
            }

            var folder = new TEntity();
            var ar = ApiCallFactory.ObjectGetDetail<TEntity>(Credentials, entityId);
            if (ar.HasException == false)
            {
                folder = ParaEntityParser.EntityFill<TEntity>(ar.XmlReceived);
                folder.FullyLoaded = true;
            }
            else
            {
                folder.FullyLoaded = false;
                folder.Id = 0;
            }

            folder.ApiCallResponse = ar;
            return folder;
        }

        ///  <summary>
        ///  Returns a Download object with all the properties of a customer.
        ///  </summary>
        ///  <param name="folderId">
        /// The Download number that you would like to get the details of. 
        /// Value Type: <see cref="Int64" />   (System.Int64)
        /// </param>
        /// 
        public Folder GetDetails(long folderId)
        {
            var folder = new Folder();
            var ar = ApiCallFactory.ObjectGetDetail<Folder>(Credentials, folderId);
            if (ar.HasException == false)
            {
                folder = ParaEntityParser.EntityFill<Folder>(ar.XmlReceived);
                folder.FullyLoaded = true;
            }
            else
            {
                folder.FullyLoaded = false;
                folder.Id = 0;
            }

            folder.ApiCallResponse = ar;
            return folder;
        }

        public TFolder GetDetails<TFolder>(XmlDocument xml)
            where TFolder : Folder, new()
        {
            var folder = ParaEntityParser.EntityFill<TFolder>(xml);
            folder.FullyLoaded = true;

            folder.ApiCallResponse.XmlReceived = xml;
            folder.ApiCallResponse.Id = folder.Id;

            return folder;
        }

        /// <summary>
        /// Returns an view list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to return</typeparam>
        /// <param name="xml">The view List XML; it should follow the exact template of the XML returned by the Parature APIs.</param>
        /// <returns></returns>
        public ParaEntityList<TEntity> GetList<TEntity>(XmlDocument xml) where TEntity : ParaEntityBaseProperties, new()
        {
            var list = ParaEntityParser.FillList<TEntity>(xml);

            list.ApiCallResponse.XmlReceived = xml;

            return list;
        }

        public ParaEntityList<TEntity> GetList<TEntity>(ParaEntityQuery query)
            where TEntity : ParaEntity, new()
        {
            if (!(query.QueryTargetType is TEntity))
            {
                throw new ArgumentException("Invalid query type for the requested entity result type", "query");
            }

            if (query.IncludeAllCustomFields)
            {
                var objschem = Create<TEntity>();
                query.IncludeCustomField(objschem.CustomFields);
            }

            return ApiUtils.ApiGetEntityList<TEntity>(Credentials, query);
        }

        public ParaEntityList<TFolder> GetList<TFolder>(FolderQuery query)
            where TFolder : Folder, new()
        {
            var folderList = new ParaEntityList<TFolder>();
            var ar = ApiCallFactory.ObjectGetList<TFolder>(Credentials, query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                folderList = ParaEntityParser.FillList<TFolder>(ar.XmlReceived);
            }
            folderList.ApiCallResponse = ar;

            // Checking if the system needs to recursively call all of the data returned.
            if (query.RetrieveAllRecords)
            {
                bool continueCalling = true;
                while (continueCalling)
                {
                    if (folderList.TotalItems > folderList.Data.Count)
                    {
                        // We still need to pull data

                        // Getting next page's data
                        query.PageNumber = query.PageNumber + 1;

                        ar = ApiCallFactory.ObjectGetList<TFolder>(Credentials, query.BuildQueryArguments());

                        var objectlist = ParaEntityParser.FillList<TFolder>(ar.XmlReceived);

                        if (objectlist.Data.Count == 0)
                        {
                            continueCalling = false;
                        }

                        folderList.Data.AddRange(objectlist.Data);
                        folderList.ResultsReturned = folderList.Data.Count;
                        folderList.PageNumber = query.PageNumber;
                    }
                    else
                    {
                        // That is it, pulled all the items.
                        continueCalling = false;
                        folderList.ApiCallResponse = ar;
                    }
                }
            }

            return folderList;
        }

        public ParaEntityList<TFolder> GetList<TFolder>()
             where TFolder : Folder, new()
        {
            var eq = new FolderQuery() { RetrieveAllRecords = true };
            return GetList<TFolder>(eq);
        }

        /// <summary>
        /// Get the List of views from within your Parature license
        /// </summary>
        /// <typeparam name="TParentEntity"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public ParaEntityList<TEntity> GetList<TParentEntity,TEntity>(ParaQuery query)
            where TEntity : ParaEntityBaseProperties, new()
            where TParentEntity : ParaEntity
        {
            if (!(query.QueryTargetType is TEntity))
            {
                throw new ArgumentException("Inavlid query type for the requested entity result type", "query");
            }

            return ApiUtils.ApiGetEntityList<TParentEntity, TEntity>(Credentials, query);
        }

        /// <summary>
        /// Retrieve the transcript for a particualr chat
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="paraCredentials"></param>
        /// <returns>A list of chat messages</returns>
        public List<ChatMessage> GetChatTranscript(long chatId)
        {
            var ar = ApiCallFactory.ChatTranscriptGetDetail(Credentials, chatId);
            if (ar.HasException == false)
            {
               return ParaEntityParser.EntityFill<ParaObjects.Chat>(ar.XmlReceived).Transcript;
            }
            else
            {
                throw new Exception(ar.ExceptionDetails);
            }
        }

        /// <summary>
        /// Locates a folder with the name provided, will return the id if found. Otherwise, it will return 0.
        /// </summary>
        /// <param name="folderName">
        /// The name of the folder you are looking for.
        /// </param>                
        /// <param name="ignoreCase">
        /// Whether or not this methods needs to ignore case when looking for the folder name or not.
        /// </param>
        /// <returns></returns>
        public long GetIdByName(string folderName, bool ignoreCase)
        {
            var query = new FolderQuery { PageSize = 500 };
            var folder = GetList<Folder>(query).FirstOrDefault(f => String.Compare(f.Name, folderName, ignoreCase) == 0);

            return folder == null ? 0 : folder.Id;
        }

        /// <summary>
        /// Locates a folder with the name provided and which has the parent folder of your choice, will return the id if found. Otherwise, it will return 0.
        /// </summary>
        /// <param name="folderName">
        /// The name of the folder you are looking for.
        /// </param>                
        /// <param name="ignoreCase">
        /// Whether or not this methods needs to ignore case when looking for the folder name or not.
        /// </param>
        /// <param name="parentFolderId">
        /// The parent folder under which to look for a folder by name.
        /// </param>
        /// <returns></returns>
        public long GetIdByName(string folderName, bool ignoreCase, long parentFolderId)
        {
            var fQuery = new FolderQuery();
            fQuery.AddStaticFieldFilter(FolderQuery.FolderStaticFields.ParentFolder, ParaEnums.QueryCriteria.Equal,
                parentFolderId.ToString());
            fQuery.PageSize = 500;
            var folder = GetList<Folder>(fQuery).FirstOrDefault(f => String.Compare(f.Name, folderName, ignoreCase) == 0);

            return folder == null ? 0 : folder.Id;
        }
    }
}
