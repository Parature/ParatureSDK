using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public class ProductFoldersList : PagedData.PagedData
    {
        public List<ProductFolder> ProductFolders = new List<ProductFolder>();
    }
}