using System;

namespace ParatureAPI.ParaObjects
{
    public partial class Eula
    {
        // Specific properties for this module
        public string ShortTitle = "";
        public Int64 EulaID = 0;

        public Eula()
        {
        }

        public Eula(Eula eula)
        {
            this.ShortTitle = eula.ShortTitle;
            this.EulaID = eula.EulaID;
        }

    }
}