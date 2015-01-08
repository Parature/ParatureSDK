using System;

namespace ParatureAPI.ParaObjects
{
    public partial class Sla
    {
        public Int64 SlaID = 0;
        public string Name = "";

        public Sla()
        {
        }

        public Sla(Sla sla)
        {
            this.SlaID = sla.SlaID;
            this.Name = sla.Name;
        }
    }
}