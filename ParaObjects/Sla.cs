using System;

namespace ParatureSDK.ParaObjects
{
    public class Sla
    {
        public Int64 SlaID = 0;
        public string Name = "";

        public Sla()
        {
        }

        public Sla(Sla sla)
        {
            SlaID = sla.SlaID;
            Name = sla.Name;
        }
    }
}