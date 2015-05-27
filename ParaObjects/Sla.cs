using System;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects
{
    public class Sla : ParaEntityBaseProperties
    {
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