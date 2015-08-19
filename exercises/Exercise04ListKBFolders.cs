using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureSDK;
using ParatureSDK.ParaObjects;
using ParatureSDK.Query.EntityQuery;

namespace Exercises
{
    class Exercise04ListKBFolders
    {
        static ParaService Service { get; set; }

        public Exercise04ListKBFolders()
        {
            Service = new ParaService(CredentialProvider.Creds);
        }

        public static ParaEntityList<ArticleFolder> getPrivateFolders()
        {
            var folderQuery = new ArticleFolderQuery();
            folderQuery.RetrieveAllRecords = true;
            folderQuery.AddCustomFilter("Is_Private=true");

            return Service.GetList<ArticleFolder>(folderQuery);
        }

        public static ParaEntityList<ArticleFolder> getFoldersByParentID(long parentID)
        {
            var folderQuery = new ArticleFolderQuery();
            folderQuery.RetrieveAllRecords = true;
            folderQuery.AddStaticFieldFilter(ArticleFolderQuery.FolderStaticFields.ParentFolder, ParaEnums.QueryCriteria.Equal, parentID.ToString());

            return Service.GetList<ArticleFolder>(folderQuery);
        }

        public static ParaEntityList<ArticleFolder> getFoldersByParentID(string folderName)
        {
            var folderQuery = new ArticleFolderQuery();
            folderQuery.RetrieveAllRecords = true;
            folderQuery.AddStaticFieldFilter(ArticleFolderQuery.FolderStaticFields.Name, ParaEnums.QueryCriteria.Equal, folderName);

            return Service.GetList<ArticleFolder>(folderQuery);
        }
    }
}
