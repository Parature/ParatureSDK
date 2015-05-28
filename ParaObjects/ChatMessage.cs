using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects
{
    /// <summary>
    /// A single message from a chat
    /// </summary>
    [XmlRoot("Message")]
    public class ChatMessage
    {
        public bool Internal { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public string Csr { get; set; }
        public string Customer { get; set; }
    }
}
