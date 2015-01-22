using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public class DownloadFoldersList : PagedData.PagedData
    {
        public List<DownloadFolder> DownloadFolders = new List<DownloadFolder>();
    }
}