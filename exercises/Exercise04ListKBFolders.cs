using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureSDK;
using ParatureSDK.ParaObjects;
using ParatureSDK.EntityQuery;

namespace Exercises
{
    class Exercise04ListKBFolders
    {
        public static ParaEntityList<ArticleFolder> getPrivateFolders()
        {
            var folderQuery = new ParatureSDK.EntityQuery.ArticleFolderQuery();
            folderQuery.RetrieveAllRecords = true;
            folderQuery.AddCustomFilter("Is_Private=true");

            var folders = ParatureSDK.ApiHandler.Article.ArticleFolder.GetList(CredentialProvider.Creds, folderQuery);

            return folders;
        }

        public static ParaEntityList<ArticleFolder> getFoldersByParentID(long parentID)
        {
            var folderQuery = new ParatureSDK.EntityQuery.ArticleFolderQuery();
            folderQuery.RetrieveAllRecords = true;
            folderQuery.AddStaticFieldFilter(ParatureSDK.EntityQuery.ArticleFolderQuery.FolderStaticFields.ParentFolder, ParaEnums.QueryCriteria.Equal, parentID.ToString());

            var folders = ParatureSDK.ApiHandler.Article.ArticleFolder.GetList(CredentialProvider.Creds, folderQuery);

            return folders;
        }

        public static ParaEntityList<ArticleFolder> getFoldersByParentID(string folderName)
        {
            var folderQuery = new ArticleFolderQuery();
            folderQuery.RetrieveAllRecords = true;
            folderQuery.AddStaticFieldFilter(ArticleFolderQuery.FolderStaticFields.Name, ParaEnums.QueryCriteria.Equal, folderName);

            var folders = ParatureSDK.ApiHandler.Article.ArticleFolder.GetList(CredentialProvider.Creds, folderQuery);

            return folders;
        }
    }
}
