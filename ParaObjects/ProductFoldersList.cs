using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public class ProductFoldersList : PagedData.PagedData
    {
        public List<ProductFolder> ProductFolders = new List<ProductFolder>();

        public ProductFoldersList()
        {
        }

        public ProductFoldersList(ProductFoldersList ProductFoldersList)
            : base(ProductFoldersList)
        {
            ProductFolders = new List<ProductFolder>(ProductFoldersList.ProductFolders);
        }
    }
}