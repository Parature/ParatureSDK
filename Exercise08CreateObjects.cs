using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureAPI;

namespace Exercises
{
    class Exercise08CreateObjects
    {
        public static ParaObjects.Account CreateAccount(string accountName)
        {
            var emptyAccount = ApiHandler.Account.AccountSchema(CredentialProvider.Creds);

            emptyAccount.Account_Name = accountName;
            emptyAccount.Sla = new ParaObjects.Sla { SlaID = 1 }; //Default SLA
            
            //Setting custom fields below
            //emptyAccount.CustomFieldSetValue(fieldID, value); //String, Date, Checkbox
            //emptyAccount.CustomFieldSetSelectedFieldOption(fieldID, optionid) //Dropdown, Radio, Multiple Dropdown
        
            var accountCreateResponse = ApiHandler.Account.AccountInsert(emptyAccount, CredentialProvider.Creds);

            emptyAccount.Accountid = accountCreateResponse.Objectid; 
            //Create response only provides an object ID, returning our modified schema

            return emptyAccount;
        }

        public static ParaObjects.Customer CreateCustomer(string firstname, string lastname, string email, string password, long accountID)
        {
            var emptyCustomer = ApiHandler.Customer.CustomerSchema(CredentialProvider.Creds);
            emptyCustomer.First_Name = firstname;
            emptyCustomer.Last_Name = lastname;
            emptyCustomer.Sla = new ParaObjects.Sla { SlaID = 1 }; //Still need to provide even though inherited from the Account
            emptyCustomer.Email = email;
            emptyCustomer.Account = new ParaObjects.Account { Accountid = accountID };
            emptyCustomer.Password = password; //This in most cases will be randomly generated
            emptyCustomer.Password_Confirm = password;
            emptyCustomer.Status = new ParaObjects.CustomerStatus { StatusID = 2 }; // Customer status are universal 1 = Pending, 2 = Registered, 3 = Trashed, 16 = Unregistered

            var customerCreateResponse = ApiHandler.Customer.CustomerInsert(emptyCustomer, CredentialProvider.Creds);

            emptyCustomer.customerid = customerCreateResponse.Objectid;

            return emptyCustomer;
        }

        public static ParaObjects.ArticleFolder CreateKBFolder(string name, bool isPrivate, long parentFolderID)
        {
            var emptyFolder = ApiHandler.Article.ArticleFolder.ArticleFolderSchema(CredentialProvider.Creds);
            emptyFolder.Name = name;
            emptyFolder.Parent_Folder = new ParaObjects.ArticleFolder { FolderID = parentFolderID };
            emptyFolder.Is_Private = isPrivate;

            var createResponse = ApiHandler.Article.ArticleFolder.Insert(emptyFolder, CredentialProvider.Creds);

            emptyFolder.FolderID = createResponse.Objectid;

            return emptyFolder;
        }

        public static ParaObjects.Article CreateArticle(string title, string content, bool published, List<long> folders)
        {
            var emptyArticle = ApiHandler.Article.ArticleSchema(CredentialProvider.Creds);
            emptyArticle.Question = title;
            emptyArticle.Answer = content;
            emptyArticle.Published = published;
            emptyArticle.Folders = new List<ParaObjects.Folder>();

            foreach (var folderID in folders)
            {
                //Only need the IDs
                emptyArticle.Folders.Add(new ParaObjects.Folder { FolderID = folderID });
            }

            var createResponse = ApiHandler.Article.ArticleInsert(emptyArticle, CredentialProvider.Creds);

            emptyArticle.Articleid = createResponse.Objectid;

            return emptyArticle;
        }

    }
}
