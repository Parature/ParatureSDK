using System.Collections.Generic;

namespace ParatureAPI.PagedData
{
    /// <summary>
    /// Instantiate this class to hold the result set of a list call to APIs. Whenever you need to get a list of 
    /// Downloads
    /// </summary>
    public class DownloadsList : PagedData
    {
        public List<ParaObjects.Download> Downloads = new List<ParaObjects.Download>();

        public DownloadsList()
        {
        }

        public DownloadsList(DownloadsList downloadsList)
            : base(downloadsList)
        {
            Downloads = new List<ParaObjects.Download>(downloadsList.Downloads);
        }
    }
}