namespace ParatureSDK.ParaObjects
{
    /// <summary>
    /// Used only for the downloads module folders.
    /// </summary>
    public class ProductFolder : Folder
    {
        /// <summary>
        /// The last date this folder was updated.
        /// </summary>
        public string Date_Updated = "";


        public ProductFolder()
        {
        }

        public ProductFolder(ProductFolder ProductFolder)
        {
            Id = ProductFolder.Id;
            Date_Updated = ProductFolder.Date_Updated;
            FullyLoaded = ProductFolder.FullyLoaded;
            if (ProductFolder.Parent_Folder != null)
            {
                Parent_Folder = new ProductFolder();
                Parent_Folder = ProductFolder.Parent_Folder;
            }
        }
    }
}