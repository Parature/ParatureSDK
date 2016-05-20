using System;
using System.Collections.Generic;
using System.Linq;
using ParatureSDK;
using ParatureSDK.Query.EntityQuery;
using ParatureSDK.Query.ModuleQuery;
using ParatureSDK.ParaObjects;
using System.Text;

namespace Exercises
{
    static class DownloadExamples
    {
        static ParaService Service { get; set; }

        static DownloadExamples()
        {
            Service = new ParaService(CredentialProvider.Creds);
        }

        public static List<ParatureSDK.ParaObjects.DownloadFolder> getAllDownloadFolders(ParaCredentials creds)
        {
            var dfQuery = new DownloadFolderQuery();
            dfQuery.RetrieveAllRecords = true;

            var folders = Service.GetList<DownloadFolder>(dfQuery);

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

            var downloads = Service.GetList<Download>(downloadQuery);

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

            var downloads = Service.GetList<Download>(downloadQuery);

            if (downloads.ApiCallResponse.HasException)
            {
                throw new Exception(downloads.ApiCallResponse.ExceptionDetails);
            }

            return downloads.TotalItems;
        }

        public static void UploadFileForDownload()
        {
            var download = new Download()
            {
                Name = "API Test File",
                Title = "API Test File Title"
            };

            var bytes = Encoding.ASCII.GetBytes("test file");

            download.AddAttachment(Service, bytes, "text/plain", "readme.md");

            if (download.ApiCallResponse.HasException)
            {
                throw new Exception(download.ApiCallResponse.ExceptionDetails);
            }
        }
    }
}
