using System;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects
{
    public class Status : ParaEntityBaseProperties
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

        public string Name { get; set; }

        public Status()
        {
            Name = "";
            Id = 0;
        }

        public Status(Status status)
        {
            Id = status.Id;
            Name = status.Name;
        }

        public Status(Int64 ID, string Name)
        {
            Id = ID;
            this.Name = Name;
        }
    }
}