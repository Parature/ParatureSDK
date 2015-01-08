using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public partial class DownloadFoldersList : PagedData.PagedData
    {
        public List<DownloadFolder> DownloadFolders = new List<DownloadFolder>();

        public DownloadFoldersList()
        {
        }

        public DownloadFoldersList(DownloadFoldersList downloadFoldersList)
            : base(downloadFoldersList)
        {
            this.DownloadFolders = new List<DownloadFolder>(downloadFoldersList.DownloadFolders);
        }
    }
}