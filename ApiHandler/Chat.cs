using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
        private static ParaEnums.ParatureModule _module = ParaEnums.ParatureModule.Chat;

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

        public static ParaEntityList<ParaObjects.Chat> GetList(ParaCredentials creds, Boolean includeTranscripts, ChatQuery query)
        {
            return FillList(creds, includeTranscripts, query);
        }

        public static ParaEntityList<ParaObjects.Chat> GetList(ParaCredentials creds, Boolean includeTranscripts, Boolean includeHistory)
        {
            return FillList(creds, includeTranscripts, null);
        }

        private static ParaEntityList<ParaObjects.Chat> FillList(ParaCredentials creds, Boolean includeTranscripts, ChatQuery query)
        {
            if (query == null)
            {
                query = new ChatQuery();
            }
            ApiCallResponse ar;
            var chatList = new ParaEntityList<ParaObjects.Chat>();

            ar = ApiCallFactory.ObjectGetList(creds, ParaEnums.ParatureModule.Chat, query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                chatList = ParaEntityParser.FillList<ParaObjects.Chat>(ar.XmlReceived);
            }
            chatList.ApiCallResponse = ar;

            // Checking if the system needs to recursively call all of the data returned.
            if (query.RetrieveAllRecords && !ar.HasException)
            {
                // A flag variable to check if we need to make more calls
                if (query.OptimizeCalls)
                {
                    System.Threading.Thread t;
                    ThreadPool.ObjectList instance = null;
                    int callsRequired = (int)Math.Ceiling((double)(chatList.TotalItems / (double)chatList.PageSize));
                    for (int i = 2; i <= callsRequired; i++)
                    {
                        //ApiCallFactory.waitCheck(creds.Accountid);
                        query.PageNumber = i;
                        //implement semaphore right here (in the thread pool instance to control the generation of threads
                        instance = new ThreadPool.ObjectList(creds, ParaEnums.ParatureModule.Customer, query.BuildQueryArguments());
                        t = new System.Threading.Thread(delegate() { instance.Go(chatList, includeTranscripts); });
                        t.Start();
                    }

                    while (chatList.TotalItems > chatList.Data.Count)
                    {
                        Thread.Sleep(500);
                    }

                    chatList.ResultsReturned = chatList.Data.Count;
                    chatList.PageNumber = callsRequired;
                }
                else
                {
                    bool continueCalling = true;
                    while (continueCalling)
                    {

                        if (chatList.TotalItems > chatList.Data.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            query.PageNumber = query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(creds, ParaEnums.ParatureModule.Customer, query.BuildQueryArguments());
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
                }
            }

            return chatList;
        }

        static ParaObjects.Chat FillTranscriptDetails(Int64 chatid, ParaCredentials paraCredentials)
        {
            //Because chat transcripts return a Chat object with just a list messages, we'll deserialize the transcript into a chat object
            var chat = new ParaObjects.Chat();

            var ar = ApiCallFactory.ObjectGetDetail(paraCredentials, ParaEnums.ParatureEntity.ChatTranscript, chatid);
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
        static public List<ChatMessage> GetTranscript(Int64 chatId, ParaCredentials paraCredentials)
        {
            var chat = FillTranscriptDetails(chatId, paraCredentials);
            return chat.Transcript;
        }
    }
}