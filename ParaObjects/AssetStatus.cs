namespace ParatureAPI.ParaObjects
{
    public class AssetStatus : Status
    {
        // Specific properties for this module

        /// <summary>
        /// The status internal name, as CSRs see it.
        /// </summary>
        public string Text = "";
        /// <summary>
        /// The status name as customers see it on the portal and in the emails they receive.
        /// </summary>
        public string Description = "";

        public AssetStatus()
        {
        }

        public AssetStatus(AssetStatus assetStatus)
            : base(assetStatus)
        {
            Text = assetStatus.Text;
            Description = assetStatus.Description;
        }

    }
}