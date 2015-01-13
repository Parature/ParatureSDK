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
                var field = Fields.FirstOrDefault(f => f.Name == "Currency");
                var val = string.Empty;
                try
                {
                    val = field.Value;
                }
                catch (Exception e) { }

                return val;
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
                var field = Fields.FirstOrDefault(f => f.Name == "Date_Created");
                var val = DateTime.MinValue;
                try
                {
                    return DateTime.Parse(field.Value);
                }
                catch (Exception e) { }

                return val;
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
                var field = Fields.FirstOrDefault(f => f.Name == "Date_Updated");
                var val = DateTime.MinValue;
                try
                {
                    return DateTime.Parse(field.Value);
                }
                catch (Exception e) { }

                return val;
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
                var field = Fields.FirstOrDefault(f => f.Name == "Price");
                var val = string.Empty;
                try
                {
                    val = field.Value;
                }
                catch (Exception e) { }

                return val;
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
        public ProductFolder Folder = new ProductFolder();
        public bool InStock
        {
            get
            {
                var field = Fields.FirstOrDefault(f => f.Name == "InStock");
                var val = false;
                try
                {
                    val = Convert.ToBoolean(field.Value);
                }
                catch (Exception e) { }

                return val;
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "InStock");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "InStock",
                        DataType = ParaEnums.FieldDataType.boolean
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
                var field = Fields.FirstOrDefault(f => f.Name == "Longdesc");
                var val = string.Empty;
                try
                {
                    val = field.Value;
                }
                catch (Exception e) { }

                return val;
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
                var field = Fields.FirstOrDefault(f => f.Name == "Name");
                var val = string.Empty;
                try
                {
                    val = field.Value;
                }
                catch (Exception e) { }

                return val;
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
                var field = Fields.FirstOrDefault(f => f.Name == "Shortdesc");
                var val = string.Empty;
                try
                {
                    val = field.Value;
                }
                catch (Exception e) { }

                return val;
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
                var field = Fields.FirstOrDefault(f => f.Name == "Sku");
                var val = string.Empty;
                try
                {
                    val = field.Value;
                }
                catch (Exception e) { }

                return val;
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
                var field = Fields.FirstOrDefault(f => f.Name == "Visible");
                var val = false;
                try
                {
                    val = Convert.ToBoolean(field.Value);
                }
                catch (Exception e) { }

                return val;
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Visible");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Visible",
                        DataType = ParaEnums.FieldDataType.boolean
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