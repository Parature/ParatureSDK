namespace ParatureSDK.ParaObjects
{
    /// <summary>
    /// Used only for the downloads module folders.
    /// </summary>
    public class DownloadFolder : Folder
    {
        public string Date_Updated = "";

        public DownloadFolder()
        {
        }

        public DownloadFolder(DownloadFolder downloadFolder)
        {
            FullyLoaded = downloadFolder.FullyLoaded;
            Date_Updated = downloadFolder.Date_Updated;
            Parent_Folder = new Folder(downloadFolder.Parent_Folder);
        }
    }
}