namespace ParatureSDK.ParaObjects
{
    /// <summary>
    /// Used only for the downloads module folders.
    /// </summary>
    public class ProductFolder : Folder
    {
        /// <summary>
        /// Indicates whether this object is fully loaded or not. An object that is not fully loaded means 
        /// that only the id and name are available.
        /// </summary>
        public bool FullyLoaded = false;
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

        /// <summary>
        /// To avoid infinite loops, the parent folder is not instantiated when 
        /// you instantiate a new ProductFolder object. In the case you are creating a download folder, please make sure to create a new ProductFolder, 
        /// set just the id of the folder, then make the ParentFolder equals the one you just created.
        /// </summary>
        public ProductFolder Parent_Folder;
    }
}