using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureSDK;
using ParatureSDK.ParaObjects;
using ParatureSDK.ParaObjects.EntityReferences;

namespace Exercises
{
    class Exercise08CreateObjects
    {
        public static Account CreateAccount(string accountName)
        {
            var emptyAccount = ParatureSDK.ApiHandler.Account.Schema(CredentialProvider.Creds);

            emptyAccount.Account_Name = accountName;
            emptyAccount.Sla = new SlaReference { Sla = new Sla { Id = 1 }}; //Default SLA
            
            //TODO update custom field setting
            //Setting custom fields below
            //emptyAccount.CustomFieldSetValue(fieldID, value); //String, Date, Checkbox
            //emptyAccount.CustomFieldSetSelectedFieldOption(fieldID, optionid) //Dropdown, Radio, Multiple Dropdown
        
            var accountCreateResponse = ParatureSDK.ApiHandler.Account.Insert(emptyAccount, CredentialProvider.Creds);

            emptyAccount.Id = accountCreateResponse.Id; 
            //Create response only provides an object ID, returning our modified schema

            return emptyAccount;
        }

        public static Customer CreateCustomer(string firstname, string lastname, string email, string password, long accountID)
        {
            var emptyCustomer = ParatureSDK.ApiHandler.Customer.Schema(CredentialProvider.Creds);
            emptyCustomer.First_Name = firstname;
            emptyCustomer.Last_Name = lastname;
            emptyCustomer.Sla = new SlaReference {Sla = new Sla {Id = 1}};
            emptyCustomer.Email = email;
            emptyCustomer.Account = new AccountReference {Entity = new Account {Id = accountID}};
            emptyCustomer.Password = password; //This in most cases will be randomly generated
            emptyCustomer.Password_Confirm = password;

            // Customer status are usually 1 = Pending, 2 = Registered, 3 = Trashed, 16 = Unregistered};
            emptyCustomer.Status = new StatusReference{ Status = new Status() { Id = 2 }};

            var customerCreateResponse = ParatureSDK.ApiHandler.Customer.Insert(emptyCustomer, CredentialProvider.Creds);

            emptyCustomer.Id = customerCreateResponse.Id;

            return emptyCustomer;
        }

        public static ArticleFolder CreateKBFolder(string name, bool isPrivate, long parentFolderID)
        {
            var emptyFolder = Exercise07GetSchemas.ArticleFolderSchema();
            emptyFolder.Name = name;
            emptyFolder.Parent_Folder = new ArticleFolder { Id = parentFolderID };
            emptyFolder.Is_Private = isPrivate;

            var createResponse = ParatureSDK.ApiHandler.Article.ArticleFolder.Insert(emptyFolder, CredentialProvider.Creds);

            emptyFolder.Id = createResponse.Id;

            return emptyFolder;
        }

        public static Article CreateArticle(string title, string content, bool published, List<long> folders)
        {
            var emptyArticle = ParatureSDK.ApiHandler.Article.Schema(CredentialProvider.Creds);
            emptyArticle.Question = title;
            emptyArticle.Answer = content;
            emptyArticle.Published = published;
            emptyArticle.Folders = new List<ArticleFolder>();

            foreach (var folderID in folders)
            {
                //Only need the IDs
                emptyArticle.Folders.Add(new ArticleFolder { Id = folderID });
            }

            var createResponse = ParatureSDK.ApiHandler.Article.Insert(emptyArticle, CredentialProvider.Creds);

            emptyArticle.Id = createResponse.Id;

            return emptyArticle;
        }

    }
}
