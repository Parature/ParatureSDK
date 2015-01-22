using System;
using System.Linq;
using ParatureAPI.Fields;

namespace ParatureAPI.ParaObjects
{
    public class Product : ParaEntity
    {
        public string Currency
        {
            get
            {
                return GetFieldValue<string>("Currency");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Currency");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Currency",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        public DateTime Date_Created
        {
            get
            {
                return GetFieldValue<DateTime>("Date_Created");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Date_Created");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Date_Created",
                        DataType = ParaEnums.FieldDataType.DateTime
                    };
                    Fields.Add(field);
                }

                field.Value = value.ToString();
            }
        }
        public DateTime Date_Updated
        {
            get
            {
                return GetFieldValue<DateTime>("Date_Updated");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Date_Updated");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Date_Updated",
                        DataType = ParaEnums.FieldDataType.DateTime
                    };
                    Fields.Add(field);
                }

                field.Value = value.ToString();
            }
        }
        public string Price
        {
            get
            {
                return GetFieldValue<string>("Price");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Price");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Price",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        public ProductFolder Folder
        {
            get
            {
                return GetFieldValue<ProductFolder>("Folder");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Folder");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Folder",
                        DataType = ParaEnums.FieldDataType.Folder
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        public bool InStock
        {
            get
            {
                return GetFieldValue<bool>("InStock");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "InStock");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "InStock",
                        DataType = ParaEnums.FieldDataType.Boolean
                    };
                    Fields.Add(field);
                }

                field.Value = value.ToString();
            }
        }
        /// <summary>
        /// The long description of the product.
        /// </summary>
        public string Longdesc
        {
            get
            {
                return GetFieldValue<string>("Longdesc");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Longdesc");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Longdesc",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        public string Name
        {
            get
            {
                return GetFieldValue<string>("Name");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Name");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Name",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        /// <summary>
        /// The short description of the product.
        /// </summary>
        public string Shortdesc
        {
            get
            {
                return GetFieldValue<string>("Shortdesc");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Shortdesc");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Shortdesc",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        public string Sku
        {
            get
            {
                return GetFieldValue<string>("Sku");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Sku");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Sku",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        public bool Visible
        {
            get
            {
                return GetFieldValue<bool>("Visible");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Visible");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Visible",
                        DataType = ParaEnums.FieldDataType.Boolean
                    };
                    Fields.Add(field);
                }

                field.Value = value.ToString();
            }
        }

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