using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects
{
    [XmlRoot("Entities")]
    public class ParaEntityList<T> : PagedData.PagedData,IEnumerable
    {
        [XmlElement("Entities")]
        public List<T> Data = new List<T>();

        public IEnumerator GetEnumerator()
        {
            return Data.GetEnumerator();
        }
    }
}
