using System;

namespace ParatureAPI.ParaObjects
{
    public class Eula
    {
        // Specific properties for this module
        public string ShortTitle = "";
        public Int64 Id = 0;

        public Eula()
        {
        }

        public Eula(Eula eula)
        {
            ShortTitle = eula.ShortTitle;
            Id = eula.Id;
        }
    }
}