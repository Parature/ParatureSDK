using System;

namespace ParatureSDK.ParaObjects
{
    public class Timezone
    {
        public Int64 TimezoneID;
        public string Name;
        public string Abbreviation;
        public Timezone()
        {
            TimezoneID = 0;
            Name = "";
            Abbreviation = "";
        }
        public Timezone(Timezone timezone)
        {
            TimezoneID = timezone.TimezoneID;
            Name = timezone.Name;
            Abbreviation = timezone.Abbreviation;
        }

        public Timezone(Int64 ID, string Name, string Abbreviation)
        {
            TimezoneID = ID;
            this.Name = Name;
            this.Abbreviation = Abbreviation;
        }
    }
}