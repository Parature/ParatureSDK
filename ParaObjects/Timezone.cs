using System;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects
{
    public class Timezone
    {
        [XmlAttribute("id")]
        public Int64 Id;
        public string Name;
        public string Abbreviation;
        public Timezone()
        {
            Id = 0;
            Name = "";
            Abbreviation = "";
        }
        public Timezone(Timezone timezone)
        {
            Id = timezone.Id;
            Name = timezone.Name;
            Abbreviation = timezone.Abbreviation;
        }

        public Timezone(Int64 ID, string Name, string Abbreviation)
        {
            Id = ID;
            this.Name = Name;
            this.Abbreviation = Abbreviation;
        }
    }
}