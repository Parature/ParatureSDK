using System;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects
{
    public abstract class ActionBase
    {
        [XmlAttribute("id")]
        public Int64 Id = 0;
    }
}