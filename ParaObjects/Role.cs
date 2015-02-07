using System;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects
{
    public class Role
    {
        [XmlAttribute("id")]
        public Int64 Id;
        public string Name;
        public string Description;
        public Role()
        {
            Id = 0;
            Name = "";
            Description = "";
        }
        public Role(Role role)
        {
            Id = role.Id;
            Name = role.Name;
            Description = role.Description;
        }
        public Role(Int64 id, string Name, string Description)
        {
            this.Id = id;
            this.Name = Name;
            this.Description = Description;
        }
    }
}