using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using ParatureSDK.ApiHandler.ApiMethods;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Chat module.
    /// </summary>
    public class Chat : FirstLevelApiGetMethods<ParaObjects.Chat, ChatQuery>
    {
        /// <summary>
        /// Returns a Chat object with all the properties of a chat.
        /// </summary>
        /// <param name="chatid">
        ///The chat number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="creds">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        /// <param name="includeHistory">
        /// Whether to include the chat history (action history) for this particular chat
        /// </param>
        public static ParaObjects.Chat GetDetails(Int64 chatid, ParaCredentials creds, Boolean includeHistory)
        {
            var arl = new ArrayList();
            if (includeHistory)
            {
                arl.Add("_history_=true");
            }

            var ticket = GetDetails(chatid, creds, arl);
            return ticket;
        }

        /// <summary>
        /// Retrieve the list of chat entities, indicating whether to retrieve transcripts as well.
        /// Retrieving transcripts is an extra API request for each chat, and subsequently will take more time.
        /// </summary>
        /// <param name="creds">Parature credentials and department information</param>
        /// <param name="includeTranscripts">Boolean to indicate whether to iteratively retrieve transcripts or not.</param>
        /// <param name="query">Query string parameters object</param>
        /// <returns>List of Chat Entities</returns>
        public static ParaEntityList<ParaObjects.Chat> GetList(ParaCredentials creds, Boolean includeTranscripts,
            ChatQuery query)
        {
            return FillList(creds, includeTranscripts, query);
        }

        /// <summary>
        /// Retrieve the list of chat entities, indicating whether to retrieve transcripts as well.
        /// Retrieving transcripts is an extra API request for each chat, and subsequently will take more time.
        /// </summary>
        /// <param name="creds">Parature credentials and department information</param>
        /// <param name="includeTranscripts">Boolean to indicate whether to iteratively retrieve transcripts or not.</param>
        /// <param name="includeHistory">Boolean to indicate whether to include the history of each chat.</param>
        /// <returns>List of Chat Entities</returns>
        public static ParaEntityList<ParaObjects.Chat> GetList(ParaCredentials creds, Boolean includeTranscripts,
            Boolean includeHistory)
        {
            return FillList(creds, includeTranscripts, null);
        }

        private static ParaEntityList<ParaObjects.Chat> FillList(ParaCredentials creds, Boolean includeTranscripts,
            ChatQuery query)
        {
            if (query == null)
            {
                query = new ChatQuery();
            }
            var chatList = new ParaEntityList<ParaObjects.Chat>();

            // Checking if the system needs to recursively call all of the data returned.
            if (query.RetrieveAllRecords)
            {
                chatList = RetrieveAllEntitites(creds, query);
            }
            else
            {
                var ar = ApiCallFactory.ObjectGetList<ParaObjects.Chat>(creds, query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    chatList = ParaEntityParser.FillList<ParaObjects.Chat>(ar.XmlReceived);
                }
                chatList.ApiCallResponse = ar;
            }

            if (includeTranscripts)
            {
                //Fetch transcripts for each chat. Each request is another API call...
                foreach (var chat in chatList)
                {
                    chat.Transcript = GetTranscript(chat.Id, creds);
                }
            }

            return chatList;
        }

        private static ParaEntityList<ParaObjects.Chat> RetrieveAllEntitites(ParaCredentials creds, ChatQuery query)
        {
            var chatList = new ParaEntityList<ParaObjects.Chat>();
            var ar = ApiCallFactory.ObjectGetList<ParaObjects.Chat>(creds, query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                chatList = ParaEntityParser.FillList<ParaObjects.Chat>(ar.XmlReceived);
            }
            chatList.ApiCallResponse = ar;

            bool continueCalling = true;
            while (continueCalling)
            {
                if (chatList.TotalItems > chatList.Data.Count)
                {
                    // We still need to pull data

                    // Getting next page's data
                    query.PageNumber = query.PageNumber + 1;

                    ar = ApiCallFactory.ObjectGetList<ParaObjects.Chat>(creds, query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        chatList.Data.AddRange(ParaEntityParser.FillList<ParaObjects.Chat>(ar.XmlReceived).Data);
                        chatList.ResultsReturned = chatList.Data.Count;
                        chatList.PageNumber = query.PageNumber;
                    }
                    else
                    {
                        continueCalling = false;
                        chatList.ApiCallResponse = ar;
                        break;
                    }
                }
                else
                {
                    // That is it, pulled all the items.
                    continueCalling = false;
                    chatList.ApiCallResponse = ar;
                }
            }

            return chatList;
        }

        private static ParaObjects.Chat FillTranscriptDetails(Int64 chatid, ParaCredentials paraCredentials)
        {
            //Because chat transcripts return a Chat object with just a list messages, we'll deserialize the transcript into a chat object
            var chat = new ParaObjects.Chat();

            var ar = ApiCallFactory.ChatTranscriptGetDetail(paraCredentials, chatid);
            if (ar.HasException == false)
            {
                chat = ParaEntityParser.EntityFill<ParaObjects.Chat>(ar.XmlReceived);
                chat.FullyLoaded = true;
            }
            else
            {
                chat.FullyLoaded = false;
                chat.Id = 0;
            }
            chat.ApiCallResponse = ar;
            chat.IsDirty = false;
            return chat;
        }

        /// <summary>
        /// Retrieve the transcript for a particualr chat
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="paraCredentials"></param>
        /// <returns>A list of chat messages</returns>
        public static List<ChatMessage> GetTranscript(Int64 chatId, ParaCredentials paraCredentials)
        {
            var chat = FillTranscriptDetails(chatId, paraCredentials);
            return chat.Transcript;
        }
    }
}