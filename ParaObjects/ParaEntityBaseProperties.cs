using System;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects
{
    /// <summary>
    /// Provides basic shared properties among all the objects.
    /// </summary>
    public abstract class ParaEntityBaseProperties
    {
        /// <summary>
        /// Id for the entity in Parature
        /// </summary>
        [XmlAttribute(AttributeName = "id")]
        public Int64 Id = 0;

        /// <summary>
        /// Indicates whether the object is fully loaded or not. If the object is returned as a second level object, this flag will indicate whether only the id property of the object is filled, or if all the properties have been loaded.
        /// </summary>
        public bool FullyLoaded;
        /// <summary>
        /// Contains all the information regarding the API Call that was made.
        /// </summary>
        public ApiCallResponse ApiCallResponse = new ApiCallResponse();

        /// <summary>
        /// The unique identifier of this object. This is mainly used to standardize the integration process for the 
        /// Parature Technical Services Team.
        /// </summary>
        public Int64 uniqueIdentifier;

        public string serviceDeskUri;

        public string uid;

        //public ObjectType type = ObjectType.Custom;

        private bool _isDirty = false;

        /// <summary>
        /// Indicate whether the object is dirty or not (means it needs to be updated, created or deleted).
        /// </summary>
        public bool IsDirty
        {
            get { return _isDirty; }
            set { _isDirty = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected ParaEntityBaseProperties()
        {
        }

        protected ParaEntityBaseProperties(ParaEntityBaseProperties objBP)
        {
            FullyLoaded = objBP.FullyLoaded;
            ApiCallResponse = new ApiCallResponse(objBP.ApiCallResponse);
            uniqueIdentifier = objBP.uniqueIdentifier;
            IsDirty = objBP.IsDirty;
            uid = objBP.uid;
        }

        /// <summary>
        /// Manages the "IsDirty" flag property of the object.
        /// </summary>
        /// <param name="isModified">Input Parameter, to indicate whether a change happened to the object or not</param>
        protected bool DirtyStateManager(bool isModified)
        {
            if (isModified)
            {
                _isDirty = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public ParaEnums.Operation operation = ParaEnums.Operation.Ignore;
    }
}