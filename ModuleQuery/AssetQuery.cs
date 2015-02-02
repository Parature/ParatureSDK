namespace ParatureAPI.ModuleQuery
{
    /// <summary>
    /// Instantiate this class to build all the properties needed to get a list of Assets.
    /// The properties include the number of items per page, the page number, what custom fields to include in the list,
    /// as well as any filtering you need to do.
    /// </summary>
    public partial class AssetQuery : ParaQueryModuleWithCustomField
    {
        protected override void buildModuleSpecificFilter()
        {
            //_QueryFilters.Add()

        }
    }
}