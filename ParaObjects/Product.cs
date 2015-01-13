using System;

namespace ParatureAPI.ParaObjects
{
    public class Product : ParaEntity
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        ////////// Not sure this will work with reflection. Need to check the node name////////////
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string Currency = "";
        public string Date_Created;
        public string Date_Updated;
        public string Price = "";
        public ProductFolder Folder = new ProductFolder();
        public bool InStock;

        /// <summary>
        /// The long description of the product.
        /// </summary>
        public string Longdesc = "";
        public string Name = "";
        /// <summary>
        /// The short description of the product.
        /// </summary>
        public string Shortdesc = "";

        public string Sku = "";
        public bool Visible;
        public Product()
            : base()
        {
        }
        public Product(Product product)
            : base(product)
        {
            Id = product.Id;
            Price = product.Price;
            Currency = product.Currency;
            Folder = new ProductFolder(product.Folder);
            InStock = product.InStock;
            Longdesc = product.Longdesc;
            Name = product.Name;
            Shortdesc = product.Shortdesc;
            Sku = product.Sku;
            Visible = product.Visible;
            Date_Created = product.Date_Created;
            Date_Updated = product.Date_Updated;
        }


        public override string GetReadableName()
        {
            return Name;
        }
    }
}