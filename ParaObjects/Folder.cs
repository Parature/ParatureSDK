using System;

namespace ParatureSDK.ParaObjects
{
    public class Folder
    {
        // Specific properties for this module
        public Int64 FolderID = 0;
        public string Name = "";
        public string Description = "";
        public bool Is_Private = false;

        /// <summary>
        /// Contains all the information regarding the API Call that was made.
        /// </summary>
        public ApiCallResponse ApiCallResponse = new ApiCallResponse();


        public Folder()
        {
        }

        public Folder(Folder folder)
        {
            FolderID = folder.FolderID;
            Name = folder.Name;
            Description = folder.Description;
            Is_Private = folder.Is_Private;
            ApiCallResponse = folder.ApiCallResponse;
        }

    }
}