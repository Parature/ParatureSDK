namespace ParatureAPI.ParaObjects
{
    /// <summary>
    /// Used only for the downloads module folders.
    /// </summary>
    public partial class ProductFolder : Folder
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
            this.FolderID = ProductFolder.FolderID;
            this.Date_Updated = ProductFolder.Date_Updated;
            this.FullyLoaded = ProductFolder.FullyLoaded;
            if (ProductFolder.Parent_Folder != null)
            {
                this.Parent_Folder = new ProductFolder();
                this.Parent_Folder = ProductFolder.Parent_Folder;
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