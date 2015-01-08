namespace ParatureAPI.ParaObjects
{
    /// <summary>
    /// Used only for the downloads module folders.
    /// </summary>
    public partial class DownloadFolder : Folder
    {
        public bool FullyLoaded = false;
        public string Date_Updated = "";
        /// <summary>
        /// To avoid infinite loops, the parent folder is not instantiated when 
        /// you instantiate a new DownloadFolder object. In the case you are creating a download folder, please make sure to create a new download folder, 
        /// set just the id of the folder, then make the ParentFolder equals the one you just created.
        /// </summary>
        public DownloadFolder Parent_Folder;

        public DownloadFolder()
        {
        }

        public DownloadFolder(DownloadFolder downloadFolder)
        {
            this.FullyLoaded = downloadFolder.FullyLoaded;
            this.Date_Updated = downloadFolder.Date_Updated;
            this.Parent_Folder = new DownloadFolder(downloadFolder.Parent_Folder);
        }
    }
}