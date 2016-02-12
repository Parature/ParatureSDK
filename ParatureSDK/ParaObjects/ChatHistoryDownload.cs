using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParatureSDK.ParaObjects
{
    /// <summary>
    /// The action target can sometimes be a "Download" from Fast Forward. 
    /// This would indicate that the portal user downloaded a link through the chat window
    /// 
    /// There is no GUID or any context provided besides the display name of the link.
    /// </summary>
    public class ChatHistoryDownload
    {
        public string Name;
    }
}
