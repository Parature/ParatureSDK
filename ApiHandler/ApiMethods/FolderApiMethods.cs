using System;
using System.Xml;
using ParatureSDK.EntityQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler.ApiMethods
{
    public abstract class FolderApiMethods<TFolder, TQuery>
        where TFolder : Folder, new()
        where TQuery : FolderQuery, new()
    {
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
        public static Int64 GetIdByName(string folderName, bool ignoreCase, ParaCredentials creds)
        {
            Int64 id = 0;
            var query = new TQuery {PageSize = 500};
            var folders = GetList(creds, query);
            foreach (var folder in folders.Data)
            {
                if (String.Compare(folder.Name, folderName, ignoreCase) == 0)
                {
                    id = folder.Id;
                    break;
                }
            }
            return id;
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
        public static Int64 GetIdByName(string folderName, bool ignoreCase, ParaCredentials creds,
            Int64 parentFolderId)
        {
            Int64 id = 0;
            var fQuery = new TQuery();
            fQuery.AddStaticFieldFilter(FolderQuery.FolderStaticFields.ParentFolder, ParaEnums.QueryCriteria.Equal,
                parentFolderId.ToString());
            fQuery.PageSize = 500;
            var folders = GetList(creds, fQuery);
            foreach (var folder in folders.Data)
            {
                if (String.Compare(folder.Name, folderName, ignoreCase) == 0)
                {
                    id = folder.Id;
                    break;
                }
            }
            return id;
        }

        /// <summary>
        /// Creates a Parature Download Folder. Requires an Object and a credentials object. Will return the Newly Created Downloadid. Returns 0 if the Customer creation fails.
        /// </summary>
        public static ApiCallResponse Insert(TFolder folder, ParaCredentials creds)
        {
            var doc = XmlGenerator.GenerateXml(folder);
            var ar = ApiCallFactory.ObjectCreateUpdate<TFolder>(creds, doc, 0);
            folder.Id = ar.Id;
            return ar;
        }

        /// <summary>
        /// Updates a Parature Download. Requires an Object and a credentials object.  Will return the updated Downloadid. Returns 0 if the Customer update operation fails.
        /// </summary>
        public static ApiCallResponse Update(TFolder folder, ParaCredentials creds)
        {
            var ar = ApiCallFactory.ObjectCreateUpdate<TFolder>(creds, XmlGenerator.GenerateXml(folder), folder.Id);

            return ar;
        }


        /// <summary>
        /// Provides the capability to delete an Article Folder.
        /// </summary>
        /// <param name="articleFolder">
        /// The id of the Article Folder to delete
        /// </param>
        public static ApiCallResponse Delete(Int64 folderId, ParaCredentials creds)
        {
            return ApiCallFactory.ObjectDelete<TFolder>(creds, folderId, true);
        }

        ///  <summary>
        ///  Returns a Download object with all the properties of a customer.
        ///  </summary>
        ///  <param name="Downloadid">
        /// The Download number that you would like to get the details of. 
        /// Value Type: <see cref="Int64" />   (System.Int64)
        /// </param>
        ///  <param name="creds">
        ///  The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        ///  </param>
        public static TFolder GetDetails(Int64 folderId, ParaCredentials creds)
        {
            var folder = FillDetails(folderId, creds);

            return folder;
        }

        /// <summary>
        /// Provides you with the capability to list Downloads, following criteria you would set
        /// by instantiating a ModuleQuery.DownloadQuery object
        /// </summary>
        public static ParaEntityList<TFolder> GetList(ParaCredentials creds, TQuery query)
        {
            return FillList(creds, query);
        }

        /// <summary>
        /// Fills an Download list object.
        /// </summary>
        private static ParaEntityList<TFolder> FillList(ParaCredentials creds, TQuery query)
        {
            var folderList = new ParaEntityList<TFolder>();
            var ar = ApiCallFactory.ObjectGetList<TFolder>(creds, query.BuildQueryArguments());
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

                        ar = ApiCallFactory.ObjectGetList<TFolder>(creds, query.BuildQueryArguments());

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

        private static TFolder FillDetails(Int64 folderId, ParaCredentials creds)
        {
            var folder = new TFolder();
            var ar = ApiCallFactory.ObjectGetDetail<TFolder>(creds, folderId);
            if (ar.HasException == false)
            {
                folder = ParaEntityParser.EntityFill<TFolder>(ar.XmlReceived);
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

        public static ParaEntityList<TFolder> GetList(ParaCredentials creds)
        {
            var eq = new TQuery() {RetrieveAllRecords = true};
            return FillList(creds, eq);
        }

        /// <summary>
        /// Returns an downloadFolder object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="xml">
        /// The DownloadFolder XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static TFolder GetDetails(XmlDocument xml)
        {
            var folder = ParaEntityParser.EntityFill<TFolder>(xml);
            folder.FullyLoaded = true;

            folder.ApiCallResponse.XmlReceived = xml;
            folder.ApiCallResponse.Id = folder.Id;

            return folder;
        }

        /// <summary>
        /// Returns an downloadFolder list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="listXml">
        /// The DownloadFolder List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<TFolder> GetList(XmlDocument listXml)
        {
            var downloadFoldersList = ParaEntityParser.FillList<TFolder>(listXml);

            downloadFoldersList.ApiCallResponse.XmlReceived = listXml;

            return downloadFoldersList;
        }
    }
}