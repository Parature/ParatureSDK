using System;
using System.Collections.Generic;
using System.Linq;
using ParatureSDK.Fields;
using ParatureSDK.ParaObjects.EntityReferences;

namespace ParatureSDK.ParaObjects
{
    public class Product : ParaEntity, IMutableEntity
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
                        FieldType = "text",
                        DataType = "string"
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
                        FieldType = "usdate",
                        DataType = "date"
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
                        FieldType = "usdate",
                        DataType = "date"
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
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public ProductFolderReference Folder
        {
            get
            {
                return GetFieldValue<ProductFolderReference>("Folder");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Folder");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Folder",
                        FieldType = "dropdown",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public bool? InStock
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
                        FieldType = "checkbox",
                        DataType = "boolean"
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
                        FieldType = "text",
                        DataType = "string"
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
                        FieldType = "text",
                        DataType = "string"
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
                        FieldType = "text",
                        DataType = "string"
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
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public bool? Visible
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
                        FieldType = "checkbox",
                        DataType = "boolean"
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
            Folder = new ProductFolderReference()
            {
                ProductFolder = product.Folder.ProductFolder
            };
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