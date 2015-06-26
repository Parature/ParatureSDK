using System;
using System.Xml;

namespace ParatureSDK.ParaObjects
{
    public class Eula : ParaEntityBaseProperties
    {
        public string ShortTitle { get; set; }
        
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