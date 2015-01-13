using System;
using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public class Asset : ParaEntity
    {
        /// <summary>
        /// The account that owns the asset, if any.
        /// </summary>
        public Account Account_Owner = new Account();

        /// <summary>
        /// The CSR that created the asset.
        /// </summary>
        public Csr Created_By = new Csr();

        /// <summary>
        /// The customer that owns the asset, if any.
        /// </summary>
        public Customer Customer_Owner = new Customer();

        /// <summary>
        /// The CSR that last modified the asset.
        /// </summary>
        public Csr Modified_By = new Csr();

        /// <summary>
        /// The name of the Asset.
        /// </summary>
        public string Name = "";

        /// <summary>
        /// The product this asset is derived from.
        /// </summary>
        public Product Product = new Product();

        public string Serial_Number = "";

        /// <summary>
        /// The status of the Asset.
        /// </summary>           
        public AssetStatus Status = new AssetStatus();

        public string Date_Created = "";
        public string Date_Updated = "";


        /// <summary>
        /// The list, if any exists, of all the available actions that can be run agains this ticket.
        /// Only the id and the name of the action
        /// </summary>
        public List<Action> AvailableActions = new List<Action>();

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
            Account_Owner = new Account(asset.Account_Owner);
            Created_By = new Csr(asset.Created_By);
            Customer_Owner = new Customer(asset.Customer_Owner);
            Modified_By = new Csr(asset.Modified_By);
            Name = asset.Name;
            Product = new Product(asset.Product);
            Status = new AssetStatus(asset.Status);
            Date_Created = asset.Date_Created;
            Date_Updated = asset.Date_Updated;
            AvailableActions = new List<Action>(asset.AvailableActions);
            Serial_Number = asset.Serial_Number;
        }


        public override string GetReadableName()
        {
            return Name;
        }
    }
}