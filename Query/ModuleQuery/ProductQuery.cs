namespace ParatureSDK.Query.ModuleQuery
{
    /// <summary>
    /// Instantiate this class to build all the properties needed to get a list of Products.
    /// The properties include the number of items per page, the page number, what custom fields to include in the list,
    /// as well as any filtering you need to do.
    /// </summary>
    public class ProductQuery : ParaEntityQuery
    {
        protected override void BuildModuleSpecificFilter()
        {}

        /// <summary>
        /// Contains all the static properties you will need when filtering by static fields.
        /// </summary>
        public static class ProductStaticFields
        {
            public readonly static string Name = "Name";
            public readonly static string Date_Created = "Date_Created";
            public readonly static string Date_Updated = "Date_Updated";
            public readonly static string Folder = "Folder_id_";
            public readonly static string Visible  = "Visible ";
            public readonly static string Instock  = "Instock ";
            public readonly static string Sku = "Sku";
            public readonly static string Price  = "Price ";
            public readonly static string Shortdesc  = "Shortdesc ";
            public readonly static string Longdesc  = "Longdesc ";
        }
    }
}