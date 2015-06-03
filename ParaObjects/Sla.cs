using System;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects
{
    public class Sla : ParaEntity
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

        public override string GetReadableName()
        {
            return Name;
        }
    }
}