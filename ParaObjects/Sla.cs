using System;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects
{
    public class Sla
    {
        [XmlAttribute("id")]
        public Int64 Id = 0;
        public string Name = "";

        public Sla()
        {
        }

        public Sla(Sla sla)
        {
            Id = sla.Id;
            Name = sla.Name;
        }
    }
}