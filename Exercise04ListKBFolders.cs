using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureAPI;

namespace Exercises
{
    class Exercise04ListKBFolders
    {
        public static ParaObjects.ArticleFoldersList getPrivateFolders()
        {
            var folderQuery = new EntityQuery.ArticleFolderQuery();
            folderQuery.RetrieveAllRecords = true;
            folderQuery.AddCustomFilter("Is_Private=true");

            var folders = ApiHandler.Article.ArticleFolder.ArticleFoldersGetList(CredentialProvider.Creds, folderQuery);

            return folders;
        }

        public static ParaObjects.ArticleFoldersList getFoldersByParentID(long parentID)
        {
            var folderQuery = new EntityQuery.ArticleFolderQuery();
            folderQuery.RetrieveAllRecords = true;
            folderQuery.AddStaticFieldFilter(EntityQuery.ArticleFolderQuery.ArticleFolderStaticFields.ParentFolder, Paraenums.QueryCriteria.Equal, parentID.ToString());

            var folders = ApiHandler.Article.ArticleFolder.ArticleFoldersGetList(CredentialProvider.Creds, folderQuery);

            return folders;
        }

        public static ParaObjects.ArticleFoldersList getFoldersByParentID(string folderName)
        {
            var folderQuery = new EntityQuery.ArticleFolderQuery();
            folderQuery.RetrieveAllRecords = true;
            folderQuery.AddStaticFieldFilter(EntityQuery.ArticleFolderQuery.ArticleFolderStaticFields.Name, Paraenums.QueryCriteria.Equal, folderName);

            var folders = ApiHandler.Article.ArticleFolder.ArticleFoldersGetList(CredentialProvider.Creds, folderQuery);

            return folders;
        }
    }
}
