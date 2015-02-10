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
        [XmlIgnore]
        public bool FullyLoaded;
        /// <summary>
        /// Contains all the information regarding the API Call that was made.
        /// </summary>
        [XmlIgnore]
        public ApiCallResponse ApiCallResponse = new ApiCallResponse();

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("service-desk-uri")]
        public string ServiceDeskUri;

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("href")]
        public string Href;

        /// <summary>
        /// Indicate whether the object is dirty or not (means it needs to be updated, created or deleted).
        /// </summary>
        [XmlIgnore]
        public bool IsDirty { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected ParaEntityBaseProperties()
        {
            IsDirty = false;
        }

        protected ParaEntityBaseProperties(ParaEntityBaseProperties objBP)
        {
            FullyLoaded = objBP.FullyLoaded;
            ApiCallResponse = new ApiCallResponse(objBP.ApiCallResponse);
            IsDirty = objBP.IsDirty;
        }

        /// <summary>
        /// Manages the "IsDirty" flag property of the object.
        /// </summary>
        /// <param name="isModified">Input Parameter, to indicate whether a change happened to the object or not</param>
        protected bool DirtyStateManager(bool isModified)
        {
            if (isModified)
            {
                IsDirty = true;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}