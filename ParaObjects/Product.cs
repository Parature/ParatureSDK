using System;
using System.Linq;
using ParatureSDK.Fields;

namespace ParatureSDK.ParaObjects
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
                var field = StaticFields.FirstOrDefault(f => f.Name == "Currency");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Currency",
                        FieldDataType = ParaEnums.FieldDataType.String
                    };
                    StaticFields.Add(field);
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
                var field = StaticFields.FirstOrDefault(f => f.Name == "Date_Created");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Date_Created",
                        FieldDataType = ParaEnums.FieldDataType.DateTime
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
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
                var field = StaticFields.FirstOrDefault(f => f.Name == "Date_Updated");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Date_Updated",
                        FieldDataType = ParaEnums.FieldDataType.DateTime
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
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
                var field = StaticFields.FirstOrDefault(f => f.Name == "Price");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Price",
                        FieldDataType = ParaEnums.FieldDataType.String
                    };
                    StaticFields.Add(field);
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
                var field = StaticFields.FirstOrDefault(f => f.Name == "Folder");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Folder",
                        FieldDataType = ParaEnums.FieldDataType.Folder
                    };
                    StaticFields.Add(field);
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
                var field = StaticFields.FirstOrDefault(f => f.Name == "InStock");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "InStock",
                        FieldDataType = ParaEnums.FieldDataType.Boolean
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
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
                var field = StaticFields.FirstOrDefault(f => f.Name == "Longdesc");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Longdesc",
                        FieldDataType = ParaEnums.FieldDataType.String
                    };
                    StaticFields.Add(field);
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
                var field = StaticFields.FirstOrDefault(f => f.Name == "Name");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Name",
                        FieldDataType = ParaEnums.FieldDataType.String
                    };
                    StaticFields.Add(field);
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
                var field = StaticFields.FirstOrDefault(f => f.Name == "Shortdesc");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Shortdesc",
                        FieldDataType = ParaEnums.FieldDataType.String
                    };
                    StaticFields.Add(field);
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
                var field = StaticFields.FirstOrDefault(f => f.Name == "Sku");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Sku",
                        FieldDataType = ParaEnums.FieldDataType.String
                    };
                    StaticFields.Add(field);
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
                var field = StaticFields.FirstOrDefault(f => f.Name == "Visible");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Visible",
                        FieldDataType = ParaEnums.FieldDataType.Boolean
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
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