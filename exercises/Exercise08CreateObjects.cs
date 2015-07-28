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
            var parature = new ParaService(CredentialProvider.Creds);
            var newAccount = parature.Create<Account>();

            newAccount.Account_Name = accountName;
            newAccount.Sla = new SlaReference { Sla = new Sla { Id = 1 }}; //Default SLA

            //TODO update custom field setting
            //Setting custom fields below
            //emptyAccount.CustomFieldSetValue(fieldID, value); //String, Date, Checkbox
            //emptyAccount.CustomFieldSetSelectedFieldOption(fieldID, optionid) //Dropdown, Radio, Multiple Dropdown

            // The new version of Insert sets the server-provided Id field on the passed-in entity.
            var accountCreateResponse = parature.Insert(newAccount);

            return newAccount;
        }

        public static Customer CreateCustomer(string firstname, string lastname, string email, string password, long accountID)
        {
            var parature = new ParaService(CredentialProvider.Creds);
            var newCustomer = parature.Create<Customer>();
            newCustomer.First_Name = firstname;
            newCustomer.Last_Name = lastname;
            newCustomer.Sla = new SlaReference {Sla = new Sla {Id = 1}};
            newCustomer.Email = email;
            newCustomer.Account = new AccountReference {Entity = new Account {Id = accountID}};
            newCustomer.Password = password; //This in most cases will be randomly generated
            newCustomer.Password_Confirm = password;

            // Customer status are usually 1 = Pending, 2 = Registered, 3 = Trashed, 16 = Unregistered};
            newCustomer.Status = new StatusReference{ Status = new Status() { Id = 2 }};

            // The new version of Insert sets the server-provided Id field on the passed-in entity.
            var customerCreateResponse = parature.Insert(newCustomer);

            return newCustomer;
        }

        public static ArticleFolder CreateKBFolder(string name, bool isPrivate, long parentFolderID)
        {
            var parature = new ParaService(CredentialProvider.Creds);
            var newFolder = Exercise07GetSchemas.ArticleFolderSchema();
            newFolder.Name = name;
            newFolder.Parent_Folder = new ArticleFolder { Id = parentFolderID };
            newFolder.Is_Private = isPrivate;

            var createResponse = parature.Insert(newFolder);

            // The new version of Insert sets the server-provided Id field on the passed-in entity.
            newFolder.Id = createResponse.Id;

            return newFolder;
        }

        public static Article CreateArticle(string title, string content, bool published, List<long> folders)
        {
            var parature = new ParaService(CredentialProvider.Creds);
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
