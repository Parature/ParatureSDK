using System;

namespace ParatureAPI.ParaObjects
{
    public partial class Timezone
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
            this.TimezoneID = timezone.TimezoneID;
            this.Name = timezone.Name;
            this.Abbreviation = timezone.Abbreviation;
        }

        public Timezone(Int64 ID, string Name, string Abbreviation)
        {
            this.TimezoneID = ID;
            this.Name = Name;
            this.Abbreviation = Abbreviation;
        }
    }
}