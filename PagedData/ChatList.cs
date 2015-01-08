using System.Collections.Generic;
using ParatureAPI.ParaObjects;

namespace ParatureAPI.PagedData
{
    /// <summary>
    /// Instantiate this class to hold the result set of a list call to APIs. Whenever you need to get a list of 
    /// Chats
    /// </summary>
    public class ChatList : PagedData
    {
        public List<ParaObjects.Chat> chats = new List<ParaObjects.Chat>();

        public ChatList()
        {
        }
    }
}