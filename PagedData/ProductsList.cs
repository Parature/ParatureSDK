using System.Collections.Generic;
using ParatureAPI.ParaObjects;

namespace ParatureAPI.PagedData
{
    /// <summary>
    /// Instantiate this class to hold the result set of a list call to APIs. Whenever you need to get a list of 
    /// Products
    /// </summary>
    public class ProductsList : PagedData
    {
        public List<Product> Products = new List<Product>();

        public ProductsList()
        {
        }

        public ProductsList(ProductsList productsList)
            : base(productsList)
        {
            Products = new List<Product>(productsList.Products);
        }
    }
}