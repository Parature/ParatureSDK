using ParatureSDK.ParaObjects;
using System;

namespace ParatureSDK.Query.EntityQuery
{
    public class DownloadFolderQuery : FolderQuery
    {
        internal override Type QueryTargetType
        {
            get
            {
                return typeof(DownloadFolder);
            }
        }

        protected override void BuildModuleSpecificFilter()
        {}

        /// <summary>
        /// Contains all the static properties you will need when filtering by static fields.
        /// </summary>
        public static class DownloadFolderStaticFields
        {}
    }
}