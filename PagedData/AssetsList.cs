using System.Collections.Generic;

namespace ParatureAPI.PagedData
{
    /// <summary>
    /// Instantiate this class to hold the result set of a list call to APIs. Whenever you need to get a list of 
    /// Assets
    /// </summary>
    public class AssetsList : PagedData
    {
        public List<ParaObjects.Asset> Assets = new List<ParaObjects.Asset>();

        public AssetsList()
        {
        }

        public AssetsList(AssetsList assetsList)
            : base(assetsList)
        {
            Assets = new List<ParaObjects.Asset>(assetsList.Assets);
        }
    }
}