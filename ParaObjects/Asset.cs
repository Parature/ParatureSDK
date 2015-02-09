using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ParatureSDK.Fields;
using ParatureSDK.ParaObjects.EntityReferences;

namespace ParatureSDK.ParaObjects
{
    public class Asset : ParaEntity
    {
        /// <summary>
        /// The account that owns the asset, if any.
        /// </summary>
        public AccountReference Account_Owner
        {
            get
            {
                return GetFieldValue<AccountReference>("Account_Owner");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Account_Owner");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Account_Owner",
                        FieldDataType = ParaEnums.FieldDataType.EntityReference
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The CSR that created the asset.
        /// </summary>
        public CsrReference Created_By
        {
            get
            {
                return GetFieldValue<CsrReference>("Created_By");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Created_By");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Created_By",
                        FieldDataType = ParaEnums.FieldDataType.EntityReference
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The customer that owns the asset, if any.
        /// </summary>
        public CustomerReference Customer_Owner
        {
            get
            {
                return GetFieldValue<CustomerReference>("Customer_Owner");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Customer_Owner");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Customer_Owner",
                        FieldDataType = ParaEnums.FieldDataType.EntityReference
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The CSR that last modified the asset.
        /// </summary>
        public CsrReference Modified_By
        {
            get
            {
                return GetFieldValue<CsrReference>("Modified_By");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Modified_By");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Modified_By",
                        FieldDataType = ParaEnums.FieldDataType.EntityReference
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The name of the Asset.
        /// </summary>
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
        /// The product this asset is derived from.
        /// </summary>
        public ProductReference Product
        {
            get
            {
                return GetFieldValue<ProductReference>("Product");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Product");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Product",
                        FieldDataType = ParaEnums.FieldDataType.EntityReference
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public string Serial_Number
        {
            get
            {
                return GetFieldValue<string>("Serial_Number");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Serial_Number");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Serial_Number",
                        FieldDataType = ParaEnums.FieldDataType.String
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The status of the Asset.
        /// </summary>           
        public StatusReference Status
        {
            get
            {
                return GetFieldValue<StatusReference>("Status");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Status");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Status",
                        FieldDataType = ParaEnums.FieldDataType.Status
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


        /// <summary>
        /// The list, if any exists, of all the available actions that can be run agains this ticket.
        /// Only the id and the name of the action
        /// </summary>
        [XmlArray("Actions")]
        [XmlArrayItem("Action")]
        public List<Action> Actions
        {
            get
            {
                return GetFieldValue<List<Action>>("AvailableActions");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "AvailableActions");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "AvailableActions",
                        FieldDataType = ParaEnums.FieldDataType.Action
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        // No vendors for now.
        ///// <summary>
        ///// Only use this if you have the Vendor feature activated.
        ///// </summary>
        //public string Vendor = "";

        public Asset()
            : base()
        {
        }

        public Asset(Asset asset)
            : base(asset)
        {
            Id = asset.Id;
            Account_Owner = asset.Account_Owner;
            Created_By = asset.Created_By;
            Customer_Owner = asset.Customer_Owner;
            Modified_By = asset.Modified_By;
            Name = asset.Name;
            Product = asset.Product;
            Status = asset.Status;
            Date_Created = asset.Date_Created;
            Date_Updated = asset.Date_Updated;
            Actions = new List<Action>(asset.Actions);
            Serial_Number = asset.Serial_Number;
        }


        public override string GetReadableName()
        {
            return Name;
        }
    }
}