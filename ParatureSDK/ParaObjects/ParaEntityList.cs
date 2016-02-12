using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects
{
    [XmlRoot("Entities")]
    public class ParaEntityList<T> : PagedData.PagedData,IEnumerable<T>
    {
        [XmlElement("Entities")]
        public List<T> Data = new List<T>();

        public IEnumerator<T> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Add method is needed for serialization/deserialization
        /// </summary>
        /// <param name="obj"></param>
        public void Add(Object obj)
        {
            Data.Add((T)obj);
        }
    }
}
