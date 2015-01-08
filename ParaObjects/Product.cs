using System;

namespace ParatureAPI.ParaObjects
{
    public partial class Product : ModuleWithCustomFields
    {


        ///////////////////////////////////////////////////////////////////////////////////////////
        ////////// Not sure this will work with reflection. Need to check the node name////////////
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string Currency = "";

        public string Date_Created;
        public string Date_Updated;

        public Int64 productid = 0;
        public string Price = "";

        public ProductFolder Folder = new ProductFolder();




        public bool Instock;

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
            this.productid = product.productid;
            this.Price = product.Price;
            this.Currency = product.Currency;
            this.Folder = new ProductFolder(product.Folder);
            this.Instock = product.Instock;
            this.Longdesc = product.Longdesc;
            this.Name = product.Name;
            this.Shortdesc = product.Shortdesc;
            this.Sku = product.Sku;
            this.Visible = product.Visible;
            this.Date_Created = product.Date_Created;
            this.Date_Updated = product.Date_Updated;
        }


        public override string GetReadableName()
        {
            return this.Name;
        }
    }
}