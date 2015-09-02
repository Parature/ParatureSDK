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
        static ParaService Service { get; set; }

        public Exercise08CreateObjects()
        {
            Service = new ParaService(CredentialProvider.Creds);
        }

        public static Account CreateAccount(string accountName)
        {
            var newAccount = Service.Create<Account>();

            newAccount.Account_Name = accountName;
            newAccount.Sla = new SlaReference { Sla = new Sla { Id = 1 }}; //Default SLA

            //TODO update custom field setting
            //Setting custom fields below
            //emptyAccount.CustomFieldSetValue(fieldID, value); //String, Date, Checkbox
            //emptyAccount.CustomFieldSetSelectedFieldOption(fieldID, optionid) //Dropdown, Radio, Multiple Dropdown

            // The new version of Insert sets the server-provided Id field on the passed-in entity.
            Service.Insert(newAccount);

            return newAccount;
        }

        public static Customer CreateCustomer(string firstname, string lastname, string email, string password, long accountID)
        {
            var newCustomer = Service.Create<Customer>();
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
            Service.Insert(newCustomer);

            return newCustomer;
        }

        public static ArticleFolder CreateKBFolder(string name, bool isPrivate, long parentFolderID)
        {
            var newFolder = Service.Create<ArticleFolder>();
            newFolder.Name = name;
            newFolder.Parent_Folder = new ArticleFolder { Id = parentFolderID };
            newFolder.Is_Private = isPrivate;


            // The new version of Insert sets the server-provided Id field on the passed-in entity.
            Service.Insert(newFolder);

            return newFolder;
        }

        public static Article CreateArticle(string title, string content, bool published, List<long> folders)
        {
            var newArticle = Service.Create<Article>();
            newArticle.Question = title;
            newArticle.Answer = content;
            newArticle.Published = published;
            newArticle.Folders = new List<ArticleFolder>();

            foreach (var folderID in folders)
            {
                //Only need the IDs
                newArticle.Folders.Add(new ArticleFolder { Id = folderID });
            }

            // The new version of Insert sets the server-provided Id field on the passed-in entity.
            Service.Insert(newArticle);

            return newArticle;
        }
    }
}
