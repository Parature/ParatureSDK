using System;
using System.Collections.Generic;
using System.Linq;
using ParatureSDK;
using ParatureSDK.ApiHandler;
using ParatureSDK.Query.EntityQuery;
using ParatureSDK.Query.ModuleQuery;

namespace Exercises
{
    class DownloadExamples
    {
        public static List<ParatureSDK.ParaObjects.DownloadFolder> getAllDownloadFolders(ParaCredentials creds)
        {
            var dfQuery = new DownloadFolderQuery();
            dfQuery.RetrieveAllRecords = true;

            var folders = ParatureSDK.ApiHandler.Download.DownloadFolder.GetList(creds, dfQuery);

            if (folders.ApiCallResponse.HasException)
            {
                throw new Exception(folders.ApiCallResponse.ExceptionDetails);
            }

            return folders.ToList();
        }

        public static List<ParatureSDK.ParaObjects.Download> getDownloadsByFolder(ParaCredentials creds, long folderID)
        {
            var downloadQuery = new DownloadQuery();
            downloadQuery.RetrieveAllRecords = true;
            downloadQuery.AddStaticFieldFilter(DownloadQuery.DownloadStaticFields.Folder, ParaEnums.QueryCriteria.Equal, folderID.ToString());

            var downloads = ParatureSDK.ApiHandler.Download.GetList(creds, downloadQuery);

            if (downloads.ApiCallResponse.HasException)
            {
                throw new Exception(downloads.ApiCallResponse.ExceptionDetails);
            }

            return downloads.ToList();
        }

        public static long getDownloadsCountByFolder(ParaCredentials creds, long folderID)
        {
            var downloadQuery = new DownloadQuery();
            downloadQuery.TotalOnly = true;
            downloadQuery.AddStaticFieldFilter(DownloadQuery.DownloadStaticFields.Folder, ParaEnums.QueryCriteria.Equal, folderID.ToString());

            var downloads = ParatureSDK.ApiHandler.Download.GetList(creds, downloadQuery);

            if (downloads.ApiCallResponse.HasException)
            {
                throw new Exception(downloads.ApiCallResponse.ExceptionDetails);
            }

            return downloads.TotalItems;
        } 
    }
}
