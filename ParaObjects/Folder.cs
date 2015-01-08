using System;

namespace ParatureAPI.ParaObjects
{
    public partial class Folder
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
            this.FolderID = folder.FolderID;
            this.Name = folder.Name;
            this.Description = folder.Description;
            this.Is_Private = folder.Is_Private;
            this.ApiCallResponse = folder.ApiCallResponse;
        }

    }
}