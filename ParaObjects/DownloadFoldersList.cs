using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public class DownloadFoldersList : PagedData.PagedData
    {
        public List<DownloadFolder> DownloadFolders = new List<DownloadFolder>();

        public DownloadFoldersList()
        {
        }

        public DownloadFoldersList(DownloadFoldersList downloadFoldersList)
            : base(downloadFoldersList)
        {
            DownloadFolders = new List<DownloadFolder>(downloadFoldersList.DownloadFolders);
        }
    }
}